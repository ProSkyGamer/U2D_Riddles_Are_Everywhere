using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovingPlatformVisual))]
public class MovingPlatform : InteractableItem
{
    public event EventHandler OnPlatformArrivalToDestination;

    [Header("Moving Platfrom Settings")]
    [SerializeField] private Transform[] allDestinationTransforms;
    [SerializeField] private float timeToMoveBetwenDestinations = 3f;
    private float movingTimer;

    private bool isMoving = false;
    private int currentMovingDestination = 0;
    private PlayerMovement movingPlayer;
    private MovingPlatformVisual movingPlatformVisual;

    private Vector3 startLocation;
    private Vector3 destinationLocation;

    protected override void Awake()
    {
        base.Awake();

        movingPlatformVisual = GetComponent<MovingPlatformVisual>();
    }

    public override void OnInteract()
    {
        base.OnInteract();

        if (IsPlayerCanInteract(PlayerChangeController.Instance.GetCurrentPlayerSO()))
        {
            currentMovingDestination++;
            if (currentMovingDestination >= allDestinationTransforms.Length)
                currentMovingDestination = 0;

            StartMoveTo(allDestinationTransforms[currentMovingDestination].position);
        }
    }

    protected override void Update()
    {
        if (isInteractable)
        {
            if (movingPlatformVisual.GetCurrentAnimationState() != MovingPlatformVisual.AnimationStates.InteractableStanding)
                movingPlatformVisual.ChangeMovingPlatformAnimationState(MovingPlatformVisual.AnimationStates.InteractableStanding);

            Vector3 castPosition = transform.position + new Vector3(0f, interactableHeight / 2, 0f);
            Vector2 castCubeLenght = new Vector2(collision.size.x, interactableHeight);
            float cubeRotation = 0f;
            Vector2 cubeDirection = Vector2.up;
            float additionCubeLength = 0f;

            RaycastHit2D raycastHit = Physics2D.BoxCast(castPosition, castCubeLenght,
                cubeRotation, cubeDirection, additionCubeLength, playerLayer);
            if (raycastHit)
                if (raycastHit.rigidbody.gameObject.TryGetComponent<PlayerController>(out interactedPlayer))
                {
                    if (!isHasButtonOnInterface)
                        AddInteractButtonToInterafce();
                    return;
                }

            if (isHasButtonOnInterface)
                RemoveInteractButtonFromInterafce();
        }
        else
            if (movingPlatformVisual.GetCurrentAnimationState() != MovingPlatformVisual.AnimationStates.NotInteractable &&
                movingPlatformVisual.GetCurrentAnimationState() != MovingPlatformVisual.AnimationStates.InteractableMoving)
                movingPlatformVisual.ChangeMovingPlatformAnimationState(MovingPlatformVisual.AnimationStates.NotInteractable);

        if (isMoving)
        {
            Vector3 deltaTransform = (destinationLocation - startLocation) *
                Time.deltaTime / timeToMoveBetwenDestinations;

            transform.position += deltaTransform;
            if (movingPlayer != null)
                movingPlayer.Move(deltaTransform);

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
    }

    protected override void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 castPosition = transform.position + new Vector3(0f, interactableHeight / 2, 0f);
        Vector2 castCubeLenght = new Vector2(collision.size.x, interactableHeight);
        Gizmos.DrawCube(castPosition, castCubeLenght);
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
            for (int i = 0; i < allDestinationTransforms.Length; i++)
            {
                if (destinationPoint == allDestinationTransforms[i].position)
                    currentMovingDestination = i;
            }

            startLocation = transform.position;
            destinationLocation = destinationPoint;

            movingTimer = timeToMoveBetwenDestinations;
            isMoving = true;
            movingPlatformVisual.ChangeMovingPlatformAnimationState(MovingPlatformVisual.AnimationStates.InteractableMoving);
        }
    }

    public bool IsMoving()
    {
        return isMoving;
    }
}
