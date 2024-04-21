using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerAnimations))]
public class PlayerController : MonoBehaviour, ICanTakeDamage
{
    [Header("Live")]
    [SerializeField] private int maxHearts = 3;

    private int currentHearts;
    [SerializeField] private int immuneHits;
    private readonly float timeNotToHitBeforeTeleport = 1.5f;
    private float timerNotToHitBeforeTeleport;
    [SerializeField] private float deathYLocation = -16f;

    [Header("Movement")]
    [SerializeField] private float runSpeed = 4f;

    [SerializeField] private bool isCanSprint;
    [SerializeField] private float sprintSpeed = 4.75f;
    private float currentSpeed;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private int additionalJumps = 1;

    [Header("Checkpoints")]
    [SerializeField] private float timeBetwenBacksToCheckpoint = 0.25f;

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

    public static event EventHandler<OnPlayerImmnuneHitEventArgs> OnPlayerImmuneHit;

    public class OnPlayerImmnuneHitEventArgs : EventArgs
    {
        public int currentImmuneHits;
    }

    private readonly List<Input.Binding> lockedBindings = new();
    public static event EventHandler<OnLockedBindingChangeEventArgs> OnLockedBindingChange;

    public class OnLockedBindingChangeEventArgs : EventArgs
    {
        public List<Input.Binding> lockedBinding;
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
        timerNotToHitBeforeTeleport = timeNotToHitBeforeTeleport;

        currentSpeed = runSpeed;
    }

    private void Start()
    {
        Input.Instance.OnJumpAction += Input_OnJumpAction;
        Input.Instance.OnReturnToCheckpointKeyAction += Input_OnReturnToCheckpointKeyAction;

        Input.Instance.OnTestingKeyAction += Instance_OnTestingKeyAction;

        if (isCanSprint) Input.Instance.OnSprintAction += Input_OnSprintAction;
    }

    private void Instance_OnTestingKeyAction(object sender, EventArgs e)
    {
        Debug.Log(Input.Instance.GetIsSprinting());
    }

    private void Input_OnSprintAction(object sender, EventArgs e)
    {
        if (lockedBindings.Contains(Input.Binding.Sprint))
            return;

        currentSpeed = sprintSpeed;
    }

    private void OnDestroy()
    {
        Input.Instance.OnJumpAction -= Input_OnJumpAction;
        Input.Instance.OnReturnToCheckpointKeyAction -= Input_OnReturnToCheckpointKeyAction;
    }

    private void Input_OnReturnToCheckpointKeyAction(object sender, EventArgs e)
    {
        if (lockedBindings.Contains(Input.Binding.ReturnToCheckpoint))
            return;

        if (IsCanTeleportToCheckpoint()) TeleportToCurrentCheckpoint();
    }

    public void TeleportToCurrentCheckpoint()
    {
        LockAllBindings();
        TransitionsInterface.Instance.MakeTransition(TransitionsInterface.TransitionsTypes.Default, () =>
        {
            transform.position = CheckpointsController.Instance.GetCurrentCheckpoint().transform.position;
            timerBetwenBacksToCheckpoints = timeBetwenBacksToCheckpoint;
        });
        TransitionsInterface.Instance.OnTransitionFinished += TransitionsInterface_OnTransitionFinished;
    }

    private void TransitionsInterface_OnTransitionFinished(object sender, EventArgs e)
    {
        UnlockAllBindings();
    }

    public bool IsCanTeleportToCheckpoint()
    {
        return playerMovement.IsGrounded() && timerNotToHitBeforeTeleport <= 0 && timerBetwenBacksToCheckpoints <= 0;
    }

    private void Input_OnJumpAction(object sender, EventArgs e)
    {
        if (lockedBindings.Contains(Input.Binding.Jump))
            return;

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

    public void ForcedJump()
    {
        playerMovement.Jump(jumpForce);
        if (playerMovement.IsGrounded())
            playerAnimations.ChangeAnimation(PlayerAnimations.Animations.Jump);
        else
            playerAnimations.ChangeAnimation(PlayerAnimations.Animations.DoubleJump);
    }

    public void ForcedMoveLeft()
    {
        Vector3 toMoveVector = new Vector2(-1, 0) * currentSpeed * Time.deltaTime;
        playerMovement.Move(toMoveVector);
        playerAnimations.FlipPlayerSprite(true);
    }

    private void Update()
    {
        if (isFirstUpdate)
        {
            if (CheckpointsController.Instance != null)
                transform.position = CheckpointsController.Instance.GetCurrentCheckpoint().transform.position;
            isFirstUpdate = false;
        }

        if (isMovementEnabled)
        {
            if (isCanSprint)
            {
                if (!Input.Instance.GetIsSprinting() && currentSpeed != runSpeed)
                    currentSpeed = runSpeed;
                else if (Input.Instance.GetIsSprinting() && currentSpeed != sprintSpeed)
                    currentSpeed = sprintSpeed;
            }

            var isMoving = false;

            var inputVector = Input.Instance.GetMovementVector();
            if (inputVector != Vector2.zero)
                if (!(lockedBindings.Contains(Input.Binding.MoveLeft) && inputVector.x < 0) &&
                    !(lockedBindings.Contains(Input.Binding.MoveRight) && inputVector.x > 0))
                {
                    Vector3 toMoveVector = inputVector * currentSpeed * Time.deltaTime;
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
        if (timerNotToHitBeforeTeleport > 0)
            timerNotToHitBeforeTeleport -= Time.deltaTime;

        if (transform.position.y <= deathYLocation)
            Die();
    }

    private void ChangeAnimationState(bool isMoving)
    {
        var animationState = PlayerAnimations.Animations.Iddle;

        if (playerAnimations.GetCurrentAnimationState() == PlayerAnimations.Animations.Hit ||
            playerAnimations.GetCurrentAnimationState() == PlayerAnimations.Animations.Die)
            return;

        if (isMoving) animationState = PlayerAnimations.Animations.Run;
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
        if (immuneHits > 0)
        {
            immuneHits--;
            OnPlayerImmuneHit?.Invoke(this, new OnPlayerImmnuneHitEventArgs
            {
                currentImmuneHits = immuneHits
            });
            return;
        }

        timerNotToHitBeforeTeleport = timeNotToHitBeforeTeleport;

        var minimumHearts = 0;
        currentHearts = Mathf.Clamp(currentHearts - damage, minimumHearts, maxHearts);

        OnPlayerHealthChange?.Invoke(this, new OnPlayerHealthChangeEventArgs
        {
            currentHealth = currentHearts,
            maxHealth = maxHearts
        });

        if (currentHearts > minimumHearts)
            playerAnimations.ChangeAnimation(PlayerAnimations.Animations.Hit);
        else
            Die();
    }

    public void RegenerateHearts(int toRegenerate)
    {
        var minimumHearts = 0;
        currentHearts = Mathf.Clamp(currentHearts + toRegenerate, minimumHearts, maxHearts);

        OnPlayerHealthChange?.Invoke(this, new OnPlayerHealthChangeEventArgs
        {
            currentHealth = currentHearts,
            maxHealth = maxHearts
        });
    }

    public void SetImmuneHits(int newImmuneHitsValue)
    {
        immuneHits = newImmuneHitsValue;
    }

    public int GetImmuneHits()
    {
        return immuneHits;
    }

    public int GetCurrentHearts()
    {
        return currentHearts;
    }

    public int GetMaxHearts()
    {
        return maxHearts;
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
        OnPlayerImmuneHit = null;
        OnLockedBindingChange = null;
    }

    public void ChangeCurrentHearts(int toChange)
    {
        currentHearts = toChange;
    }

    public void LockAllBindings()
    {
        lockedBindings.Add(Input.Binding.Jump);
        lockedBindings.Add(Input.Binding.ReturnToCheckpoint);
        lockedBindings.Add(Input.Binding.Sprint);
        lockedBindings.Add(Input.Binding.MoveLeft);
        lockedBindings.Add(Input.Binding.MoveRight);

        OnLockedBindingChange?.Invoke(this, new OnLockedBindingChangeEventArgs
        {
            lockedBinding = lockedBindings
        });
    }

    public void UnlockBinding(Input.Binding bindingToUnlock)
    {
        if (lockedBindings.Contains(bindingToUnlock))
        {
            lockedBindings.Remove(bindingToUnlock);
            OnLockedBindingChange?.Invoke(this, new OnLockedBindingChangeEventArgs
            {
                lockedBinding = lockedBindings
            });
        }
    }

    public void UnlockAllBindings()
    {
        lockedBindings.Clear();

        OnLockedBindingChange?.Invoke(this, new OnLockedBindingChangeEventArgs
        {
            lockedBinding = lockedBindings
        });
    }
}
