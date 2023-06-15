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

    private int pointsCollected = 0;
    public static event EventHandler<OnPointsCollectedChangeEventArgs> OnPointsCollectedChange;
    public static event EventHandler OnPlayerDie;
    public class OnPointsCollectedChangeEventArgs : EventArgs
    {
        public int currentPoints;
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
            Vector3 jumpForceVector = new Vector3(0, jumpForce, 0);

            if (playerMovement.IsGrounded())
            {
                playerMovement.Jump(jumpForceVector);

                playerAnimations.ChangeAnimation(PlayerAnimations.Animations.Jump);
            }
            else if (additionalJumpsLeft > 0)
            {
                additionalJumpsLeft--;

                playerMovement.Jump(jumpForceVector);

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

    public void AddPoints(int toAdd)
    {
        pointsCollected += toAdd;

        OnPointsCollectedChange?.Invoke(this, new OnPointsCollectedChangeEventArgs
        {
            currentPoints = pointsCollected
        });
    }

    public void TakeDamage(int damage)
    {
        int minimumHearts = 0;
        currentHearts = Mathf.Clamp(currentHearts - damage, minimumHearts, maxHearts);

        if (currentHearts > minimumHearts)
            playerAnimations.ChangeAnimation(PlayerAnimations.Animations.Hit);
        else
            Die();
    }

    public void RegenerateHearts(int toRegenerate)
    {
        int minimumHearts = 0;
        currentHearts =  Mathf.Clamp(currentHearts + toRegenerate, minimumHearts, maxHearts);
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
        OnPointsCollectedChange = null;
        OnPlayerDie = null;
    }
}
