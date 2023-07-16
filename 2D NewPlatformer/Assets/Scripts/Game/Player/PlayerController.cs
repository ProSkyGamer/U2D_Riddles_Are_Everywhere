using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerAnimations))]
public class PlayerController : MonoBehaviour, ICanTakeDamage
{
    [Header("Live")]
    [SerializeField] private int maxHearts = 3;
    private int currentHearts;

    [Header("Movement")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private int additionalJumps = 1;

    [Header("Checkpoints")]
    [SerializeField] private float timeBetwenBacksToCheckpoint = 5f;
    private float timerBetwenBacksToCheckpoints;
    private int additionalJumpsLeft;
    private bool isMovementEnabled = true;

    public static event EventHandler OnPlayerDie;
    public static event EventHandler<OnPlayerHealthChangeEventArgs> OnPlayerHealthChange;
    public class OnPlayerHealthChangeEventArgs : EventArgs
    {
        public int currentHealth;
        public int maxHealth;
    }

    private PlayerMovement playerMovement;
    private PlayerAnimations playerAnimations;

    private bool isFirstUpdate = true;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimations = GetComponent<PlayerAnimations>();

        additionalJumpsLeft = additionalJumps;
        currentHearts = maxHearts;
        timerBetwenBacksToCheckpoints = timeBetwenBacksToCheckpoint;
    }

    private void Start()
    {
        Input.Instance.OnJumpAction += Input_OnJumpAction;
        Input.Instance.OnReturnToCheckpointKeyAction += Input_OnReturnToCheckpointKeyAction;
    }

    private void OnDestroy()
    {
        Input.Instance.OnJumpAction -= Input_OnJumpAction;
        Input.Instance.OnReturnToCheckpointKeyAction -= Input_OnReturnToCheckpointKeyAction;
    }

    private void Input_OnReturnToCheckpointKeyAction(object sender, EventArgs e)
    {
        if(timerBetwenBacksToCheckpoints <= 0)
        {
            transform.position =  CheckpointsController.Instance.GetCurrentCheckpoint().transform.position;
            timerBetwenBacksToCheckpoints = timeBetwenBacksToCheckpoint;
        }
    }

    private void Input_OnJumpAction(object sender, System.EventArgs e)
    {
        if (isMovementEnabled)
        {
            if (playerMovement.IsGrounded())
            {
                playerMovement.Jump(jumpForce);

                playerAnimations.ChangeAnimation(PlayerAnimations.Animations.Jump);
            }
            else if (additionalJumpsLeft > 0)
            {
                additionalJumpsLeft--;

                playerMovement.Jump(jumpForce);

                playerAnimations.ChangeAnimation(PlayerAnimations.Animations.DoubleJump);
            }
        }
    }

    private void Update()
    {
        if(isFirstUpdate)
        {
            transform.position = CheckpointsController.Instance.GetCurrentCheckpoint().transform.position;
            isFirstUpdate = false;

            RegenerateHearts(0);
        }

        if (isMovementEnabled)
        {
            bool isMoving = false;

            Vector2 inputVector = Input.Instance.GetMovementVector();
            if (inputVector != Vector2.zero)
            {
                Vector3 toMoveVector = inputVector * speed * Time.deltaTime;
                playerMovement.Move(toMoveVector);
                isMoving = true;
                playerAnimations.FlipPlayerSprite(inputVector.x < 0);
            }
            if (playerMovement.IsGrounded())
                if (additionalJumpsLeft != additionalJumps)
                    additionalJumpsLeft = additionalJumps;

            ChangeAnimationState(isMoving);
        }
        if (timerBetwenBacksToCheckpoints > 0)
            timerBetwenBacksToCheckpoints -= Time.deltaTime;
    }

    private void ChangeAnimationState(bool isMoving)
    {
        PlayerAnimations.Animations animationState = PlayerAnimations.Animations.Iddle;

        if (playerAnimations.GetCurrentAnimationState() == PlayerAnimations.Animations.Hit ||
            playerAnimations.GetCurrentAnimationState() == PlayerAnimations.Animations.Die)
            return;

        if (isMoving)
        {
            animationState = PlayerAnimations.Animations.Run;
        }
        if (!playerMovement.IsGrounded() || playerMovement.IsAscending())
        {
            if (playerMovement.IsDescending())
                animationState = PlayerAnimations.Animations.Fall;
            else
                animationState = playerAnimations.GetCurrentAnimationState();
        }

        playerAnimations.ChangeAnimation(animationState);
    }

    public void TakeDamage(int damage)
    {
        int minimumHearts = 0;
        currentHearts = Mathf.Clamp(currentHearts - damage, minimumHearts, maxHearts);

        OnPlayerHealthChange?.Invoke(this, new OnPlayerHealthChangeEventArgs
        {
            currentHealth = currentHearts,
            maxHealth = maxHearts
        }) ;

        if (currentHearts > minimumHearts)
            playerAnimations.ChangeAnimation(PlayerAnimations.Animations.Hit);
        else
            Die();
    }

    public void RegenerateHearts(int toRegenerate)
    {
        int minimumHearts = 0;
        currentHearts =  Mathf.Clamp(currentHearts + toRegenerate, minimumHearts, maxHearts);

        OnPlayerHealthChange?.Invoke(this, new OnPlayerHealthChangeEventArgs
        {
            currentHealth = currentHearts,
            maxHealth = maxHearts
        });
    }

    public void ChangeAllMoventSLowDown(float slowDownScale)
    {
        playerMovement.ChangeSlowDownMovement(slowDownScale);
        playerMovement.ChangeSlowDownGravity(slowDownScale);
    }

    private void Die()
    {
        playerAnimations.ChangeAnimation(PlayerAnimations.Animations.Die);
        DisableMovement();
        OnPlayerDie?.Invoke(this, EventArgs.Empty);
    }

    public void OnAnimationDie()
    {
        Destroy(gameObject);
    }

    public void DisableMovement()
    {
        isMovementEnabled = false;
    }

    public void EnableMovement()
    {
        isMovementEnabled = true;
    }

    public static void ResetStaticData()
    {
        OnPlayerDie = null;
        OnPlayerHealthChange = null;
    }

    public int GetMaxHearts()
    {
        return maxHearts;
    }

    public void ChangeCurrentHearts(int toChange)
    {
        currentHearts = toChange;
    }
}
