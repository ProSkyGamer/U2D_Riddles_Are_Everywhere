using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(MovingPlatformVisual))]
public class MovingPlatform : InteractableItem
{
    [Serializable]
    public class MovingPlatformDestinationPoint
    {
        public Transform movingPlatformDestinationPoint;
        public Transform movingPlatformDropBreakableBlockPoint;
    }

    public event EventHandler OnPlatformArrivalToDestination;

    [Header("Moving Platfrom Settings")]
    [SerializeField] private MovingPlatformDestinationPoint[] allMovingPlatformDestinations;
    [SerializeField] private float timeToMoveBetwenDestinations = 3f;
    private float movingTimer;

    [SerializeField] private bool isInteractionLocked = false;

    private bool isMoving = false;
    private int currentMovingDestination = 0;
    private PlayerMovement movingPlayer;
    private BreakableBlock storedBreakableBlock;
    private MovingPlatformVisual movingPlatformVisual;
    private BoxCollider2D collision;
    private BreakableBlock standingBreakableBlock;

    private float timeBetwenChecks = 0.1f;
    private float timerBetwenChecks;

    private Vector3 startLocation;
    private Vector3 destinationLocation;

    private void Awake()
    {
        movingPlatformVisual = GetComponent<MovingPlatformVisual>();
        collision = GetComponent<BoxCollider2D>();
        timerBetwenChecks = timeBetwenChecks;
    }

    private void Start()
    {
        if (isInteractionLocked)
            movingPlatformVisual.ChangeMovingPlatformAnimationState(MovingPlatformVisual.AnimationStates.NotInteractable);
    }

    public override void OnInteract(PlayerController player)
    {
        base.OnInteract(player);

        currentMovingDestination++;
        if (currentMovingDestination >= allMovingPlatformDestinations.Length)
            currentMovingDestination = 0;

        StartMoveTo(allMovingPlatformDestinations[currentMovingDestination].movingPlatformDestinationPoint.position);
    }

    private void Update()
    {
        if (isMoving)
        {
            Vector3 deltaTransform = (destinationLocation - startLocation) *
                Time.deltaTime / timeToMoveBetwenDestinations;

            transform.position += deltaTransform;
            movingPlayer?.Move(deltaTransform);
            storedBreakableBlock?.Move(deltaTransform);

            movingTimer -= Time.deltaTime;

            if (movingTimer <= 0)
            {
                isMoving = false;
                isInteractable = true;

                transform.position = destinationLocation;
                movingPlatformVisual.ChangeMovingPlatformAnimationState(MovingPlatformVisual.AnimationStates.InteractableStanding);

                OnPlatformArrivalToDestination?.Invoke(this, EventArgs.Empty);
            }
        }
        else if(!IsHasStoredBreakableBlock())
        {
            timerBetwenChecks -= Time.deltaTime;
            if(timerBetwenChecks <= 0)
            {
                timerBetwenChecks = timeBetwenChecks;
                float additionalHeight = 0.5f;
                Vector3 castPosition = transform.position + new Vector3(0f, additionalHeight / 2, 0f);
                Vector2 castCubeLenght = new Vector2(collision.size.x, additionalHeight);
                float cubeRotation = 0f;
                Vector2 cubeDirection = Vector2.up;
                float additionCubeLength = 0f;

                RaycastHit2D[] raycastHit = Physics2D.BoxCastAll(castPosition, castCubeLenght,
                    cubeRotation, cubeDirection, additionCubeLength);

                bool isFound = false;
                foreach(var hit in raycastHit)
                {
                    if(hit.collider.gameObject.TryGetComponent<BreakableBlock>(out BreakableBlock block))
                    {
                        isFound = true;
                        if(!block.GetIsStandingOnMovingPlatform())
                        {
                            standingBreakableBlock = block;
                            standingBreakableBlock.ChangeIsNowStandingOnMovingPlatform(true, this);
                        }
                        break;
                    }
                }

                if(!isFound)
                {
                    if(standingBreakableBlock != null)
                    {
                        standingBreakableBlock.ChangeIsNowStandingOnMovingPlatform(false, null);
                        standingBreakableBlock = null;
                    }
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.TryGetComponent<PlayerMovement>(out movingPlayer);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerMovement>(out movingPlayer))
            movingPlayer = null;
    }

    public void StartMoveTo(Vector3 destinationPoint)
    {
        if (!isMoving)
        {
            for (int i = 0; i < allMovingPlatformDestinations.Length; i++)
            {
                if (destinationPoint == allMovingPlatformDestinations[i].movingPlatformDestinationPoint.position)
                    currentMovingDestination = i;
            }

            startLocation = transform.position;
            destinationLocation = destinationPoint;

            movingTimer = timeToMoveBetwenDestinations;
            isMoving = true;
        }
    }

    public bool GetIsMoving()
    {
        return isMoving;
    }

    public override bool IsCanInteract()
    {
        return isInteractable && !isMoving && !isInteractionLocked;
    }

    public void ChangeInteractionLockState(bool newState)
    {
        isInteractionLocked = newState;
        if (isInteractionLocked)
            movingPlatformVisual.ChangeMovingPlatformAnimationState(MovingPlatformVisual.AnimationStates.NotInteractable);
        else
            movingPlatformVisual.ChangeMovingPlatformAnimationState(MovingPlatformVisual.AnimationStates.InteractableStanding);
    }

    public void AddStoredBreakableBlock(BreakableBlock breakableBlock)
    {
        storedBreakableBlock = breakableBlock;
    }
    
    public void RemoveStoredBreakableBlock()
    {
        storedBreakableBlock.gameObject.transform.position = allMovingPlatformDestinations
            [currentMovingDestination].movingPlatformDropBreakableBlockPoint.position;
        storedBreakableBlock = null;
    }

    public bool GetIsInteractionLockedState()
    {
        return isInteractionLocked;
    }

    public bool IsHasStoredBreakableBlock()
    {
        return storedBreakableBlock != null;
    }
}
