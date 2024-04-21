using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovableHeadVisual))]
public class MovableHead : BaseTrap
{
    public static event EventHandler<OnTimerChangeEventArgs> OnTimerChange;
    public class OnTimerChangeEventArgs : EventArgs
    {
        public float currentTime;
        public float maxTime;
    }

    [SerializeField] private float distanceToMoveForHit = 1f;
    [SerializeField] private float standingTimeForStartMoving = 0.5f;
    [SerializeField] private bool isCanDamage = false;
    private bool isCanBeMoved = true;
    private float standingTimerForStartMoving;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask terrainLayer;

    [SerializeField] private BoxCollider2D collision;

    public enum Directions
    {
        right,
        left,
        up,
        down,
    }

    private List<Directions> availibleDirections = new();
    private float timeBetwenBlink = 3f;
    private float timerBetwenBlink;
    private bool isMoving = false;
    private PlayerController lastInteractedPlayer;

    private MovableHeadVisual movableHeadVisual;

    protected override void Awake()
    {
        base.Awake();

        standingTimerForStartMoving = standingTimeForStartMoving;
        timerBetwenBlink = timeBetwenBlink;
        movableHeadVisual = GetComponent<MovableHeadVisual>();
    }

    private void Start()
    {
        Input.Instance.OnTestingKeyAction += Instance_OnTestingKeyAction;
    }

    private void Instance_OnTestingKeyAction(object sender, System.EventArgs e)
    {
        foreach(var direction in availibleDirections)
        {
            print(direction);
        }
    }

    protected override void Update()
    {
        if (isCanDamage)
        {
            if (dealDamageTimer > 0)
                dealDamageTimer -= Time.deltaTime;
            else
            {
                if (playerToDamageList.Count > 0)
                {
                    foreach (PlayerController playerToDamage in playerToDamageList)
                    {
                        playerToDamage.TakeDamage(trapDamage);
                    }
                    dealDamageTimer = dealDamageCooldown;
                }
            }
        }

        if (!isMoving && isCanBeMoved)
        {
            timerBetwenBlink -= Time.deltaTime;
            if (timerBetwenBlink <= 0)
            {
                if (!isCanDamage)
                    movableHeadVisual.ChangeAnimationState(MovableHeadVisual.AnimationStates.RockHeadBlink);
                else
                    movableHeadVisual.ChangeAnimationState(MovableHeadVisual.AnimationStates.SpikeHeadBlink);

                timerBetwenBlink = timeBetwenBlink;
            }

            CheckAvailibleDirections();

            PlayerController interactedPlayer;
            float cubeRotation = 0f;
            Vector2 cubeDirection = Vector2.up;
            float interactableHeight = 0f;

            Vector3 baseCastPosition = transform.position + (Vector3)collision.offset;
            Vector2 leftRightCastCubeLenght = new(collision.size.x / 4, collision.size.y * 0.75f);

            if (availibleDirections.Contains(Directions.right))
            {
                Vector3 leftCastPosition = baseCastPosition + new Vector3(-collision.size.x / 2 - leftRightCastCubeLenght.x / 2, 0f, 0f);

                RaycastHit2D leftRaycastHit = Physics2D.BoxCast(leftCastPosition, leftRightCastCubeLenght,
                    cubeRotation, cubeDirection, interactableHeight, playerLayer);

                if (leftRaycastHit)
                {
                    if (leftRaycastHit.rigidbody.gameObject.TryGetComponent<PlayerController>(out interactedPlayer))
                    {
                        lastInteractedPlayer = interactedPlayer;
                        standingTimerForStartMoving -= Time.deltaTime;
                        OnTimerChange?.Invoke(this, new OnTimerChangeEventArgs()
                        {
                            currentTime = standingTimeForStartMoving - standingTimerForStartMoving,
                            maxTime = standingTimeForStartMoving
                        });
                        if (standingTimerForStartMoving <= 0)
                            StartCubeMove(Vector2.right);
                    }
                    return;
                }
            }

            if (availibleDirections.Contains(Directions.left))
            {
                Vector3 rightCastPosition = baseCastPosition + new Vector3(collision.size.x / 2 + leftRightCastCubeLenght.x / 2, 0f, 0f);

                RaycastHit2D rightRaycastHit = Physics2D.BoxCast(rightCastPosition, leftRightCastCubeLenght,
                    cubeRotation, cubeDirection, interactableHeight, playerLayer);
                if (rightRaycastHit)
                {
                    if (rightRaycastHit.rigidbody.gameObject.TryGetComponent<PlayerController>(out interactedPlayer))
                    {
                        lastInteractedPlayer = interactedPlayer;
                        standingTimerForStartMoving -= Time.deltaTime;
                        OnTimerChange?.Invoke(this, new OnTimerChangeEventArgs()
                        {
                            currentTime = standingTimeForStartMoving - standingTimerForStartMoving,
                            maxTime = standingTimeForStartMoving
                        });
                        if (standingTimerForStartMoving <= 0)
                            StartCubeMove(Vector2.left);
                    }
                    return;
                }
            }

            Vector2 topBottomCastCubeLenght = new(collision.size.x * 0.75f, collision.size.y / 2);

            if (availibleDirections.Contains(Directions.down))
            {
                Vector3 topCastPosition = baseCastPosition + new Vector3(0f, collision.size.y / 2 + topBottomCastCubeLenght.y / 2, 0f);

                RaycastHit2D topRaycastHit = Physics2D.BoxCast(topCastPosition, topBottomCastCubeLenght,
                    cubeRotation, cubeDirection, interactableHeight, playerLayer);
                if (topRaycastHit)
                {
                    if (topRaycastHit.rigidbody.gameObject.TryGetComponent<PlayerController>(out interactedPlayer))
                    {
                        lastInteractedPlayer = interactedPlayer;
                        standingTimerForStartMoving -= Time.deltaTime;
                        OnTimerChange?.Invoke(this, new OnTimerChangeEventArgs()
                        {
                            currentTime = standingTimeForStartMoving - standingTimerForStartMoving,
                            maxTime = standingTimeForStartMoving
                        });
                        if (standingTimerForStartMoving <= 0)
                            StartCubeMove(Vector2.down);
                    }
                    return;
                }
            }

            if (availibleDirections.Contains(Directions.up))
            {
                Vector3 bottomCastPosition = baseCastPosition + new Vector3(0f, -collision.size.y / 2 - topBottomCastCubeLenght.y / 2, 0f);

                RaycastHit2D bottomRaycastHit = Physics2D.BoxCast(bottomCastPosition, topBottomCastCubeLenght,
                    cubeRotation, cubeDirection, interactableHeight, playerLayer);
                if (bottomRaycastHit)
                {
                    if (bottomRaycastHit.rigidbody.gameObject.TryGetComponent<PlayerController>(out interactedPlayer))
                    {
                        lastInteractedPlayer = interactedPlayer;
                        standingTimerForStartMoving -= Time.deltaTime;
                        OnTimerChange?.Invoke(this, new OnTimerChangeEventArgs()
                        {
                            currentTime = standingTimeForStartMoving - standingTimerForStartMoving,
                            maxTime = standingTimeForStartMoving
                        });
                        if (standingTimerForStartMoving <= 0)
                            StartCubeMove(Vector2.up);
                    }
                    return;
                }
            }

            standingTimerForStartMoving = standingTimeForStartMoving;
        }
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        Vector3 baseCastPosition = transform.position + (Vector3)collision.offset;
        Vector2 leftRightCastCubeLenght = new(collision.size.x / 4, collision.size.y * 0.75f);
        Vector2 topBottomCastCubeLenght = new(collision.size.x * 0.75f, collision.size.y / 4);

        Vector3 leftCastPosition = baseCastPosition + new Vector3(-collision.size.x / 2 - leftRightCastCubeLenght.x / 2, 0f, 0f);
        Gizmos.DrawCube(leftCastPosition, leftRightCastCubeLenght);

        Vector3 rightCastPosition = baseCastPosition + new Vector3(collision.size.x / 2 + leftRightCastCubeLenght.x / 2, 0f, 0f);
        Gizmos.DrawCube(rightCastPosition, leftRightCastCubeLenght);

        Vector3 topCastPosition = baseCastPosition + new Vector3(0f, collision.size.y / 2 + topBottomCastCubeLenght.y / 2, 0f);
        Gizmos.DrawCube(topCastPosition, topBottomCastCubeLenght);

        Vector3 bottomCastPosition = baseCastPosition + new Vector3(0f, -collision.size.y / 2 - topBottomCastCubeLenght.y / 2, 0f);
        Gizmos.DrawCube(bottomCastPosition, topBottomCastCubeLenght);

        /*Gizmos.color = Color.green;

        baseCastPosition = transform.position + (Vector3)collision.offset;
        Vector2 castCubeLenght = collision.size * 0.99f;

        leftCastPosition = baseCastPosition + new Vector3(-distanceToMoveForHit, 0f, 0f);
        Gizmos.DrawCube(leftCastPosition, castCubeLenght);

        rightCastPosition = baseCastPosition + new Vector3(distanceToMoveForHit, 0f, 0f);
        Gizmos.DrawCube(rightCastPosition, castCubeLenght);

        topCastPosition = baseCastPosition + new Vector3(0f, distanceToMoveForHit, 0f);
        Gizmos.DrawCube(topCastPosition, castCubeLenght);

        bottomCastPosition = baseCastPosition + new Vector3(0f, -distanceToMoveForHit, 0f);
        Gizmos.DrawCube(bottomCastPosition, castCubeLenght);*/
    }

    private void CheckAvailibleDirections()
    {
        if (movableHeadVisual.GetCurrentAnimationState() == MovableHeadVisual.AnimationStates.RockHeadIddle ||
            movableHeadVisual.GetCurrentAnimationState() == MovableHeadVisual.AnimationStates.SpikeHeadIddle)
            isMoving = false;

        availibleDirections.Clear();

        float cubeRotation = 0f;
        float interactableHeight = 0f;

        Vector3 baseCastPosition = transform.position;
        Vector2 castCubeLenght = collision.size * 0.99f;

        Vector3 leftCastPosition = baseCastPosition + new Vector3(-distanceToMoveForHit, 0f, 0f);
        RaycastHit2D[] leftRaycastHit = Physics2D.BoxCastAll(leftCastPosition, castCubeLenght,
            cubeRotation, Vector2.left, interactableHeight, terrainLayer);
        if (leftRaycastHit.Length == 1)
            availibleDirections.Add(Directions.left);

        Vector3 rightCastPosition = baseCastPosition + new Vector3(distanceToMoveForHit, 0f, 0f);
        RaycastHit2D[] rightRaycastHit = Physics2D.BoxCastAll(rightCastPosition, castCubeLenght,
            cubeRotation, Vector2.right, interactableHeight, terrainLayer);
        if (rightRaycastHit.Length == 1)
            availibleDirections.Add(Directions.right);

        Vector3 topCastPosition = baseCastPosition + new Vector3(0f, distanceToMoveForHit, 0f);

        RaycastHit2D[] topRaycastHit = Physics2D.BoxCastAll(topCastPosition, castCubeLenght,
            cubeRotation, Vector2.up, interactableHeight, terrainLayer);

        if (topRaycastHit.Length == 1)
            availibleDirections.Add(Directions.up);

        Vector3 bottomCastPosition = baseCastPosition + new Vector3(0f, -distanceToMoveForHit, 0f);

        RaycastHit2D[] bottomRaycastHit = Physics2D.BoxCastAll(bottomCastPosition, castCubeLenght,
            cubeRotation, Vector2.down, interactableHeight, terrainLayer);

        if (bottomRaycastHit.Length == 1)
            availibleDirections.Add(Directions.down);
    }

    private void StartCubeMove(Vector2 direction)
    {
        standingTimerForStartMoving = standingTimeForStartMoving;
        isMoving = true;

        if (!isCanDamage)
        {
            if (direction.x == 1f)
            {
                if (availibleDirections.Contains(Directions.right))
                {
                    movableHeadVisual.ChangeAnimationState(MovableHeadVisual.AnimationStates.RockHeadRightHit);
                    return;
                }
            }
            else if (direction.x == -1f)
            {
                if (availibleDirections.Contains(Directions.left))
                {
                    movableHeadVisual.ChangeAnimationState(MovableHeadVisual.AnimationStates.RockHeadLeftHit);
                    return;
                }
            }
            else if (direction.y == 1f)
            {
                if (availibleDirections.Contains(Directions.up))
                {
                    movableHeadVisual.ChangeAnimationState(MovableHeadVisual.AnimationStates.RockHeadTopHit);
                    return;
                }
            }
            else if (direction.y == -1f)
            {
                if (availibleDirections.Contains(Directions.down))
                {
                    movableHeadVisual.ChangeAnimationState(MovableHeadVisual.AnimationStates.RockHeadBottomHit);
                    return;
                }
            }
            else
                Debug.Log("ÂÑ¨ ÏÈÇÄÀ! MovableHead MoveCube without direction");
        }
        else
        {
            if (direction.x == 1f)
            {
                if (availibleDirections.Contains(Directions.right))
                {
                    movableHeadVisual.ChangeAnimationState(MovableHeadVisual.AnimationStates.SpikeHeadRightHit);
                    return;
                }
            }
            else if (direction.x == -1f)
            {
                if (availibleDirections.Contains(Directions.left))
                {
                    movableHeadVisual.ChangeAnimationState(MovableHeadVisual.AnimationStates.SpikeHeadLeftHit);
                    return;
                }
            }
            else if (direction.y == 1f)
            {
                if (availibleDirections.Contains(Directions.up))
                {
                    movableHeadVisual.ChangeAnimationState(MovableHeadVisual.AnimationStates.SpikeHeadTopHit);
                    return;
                }
            }
            else if (direction.y == -1f)
            {
                if (availibleDirections.Contains(Directions.down))
                {
                    movableHeadVisual.ChangeAnimationState(MovableHeadVisual.AnimationStates.SpikeHeadBottomHit);
                    return;
                }
            }
            else
                Debug.Log("ÂÑ¨ ÏÈÇÄÀ! MovableHead MoveCube without direction");
        }
        isMoving = false;
    }

    public void OnAnimationHitStop()
    {
        isMoving = false;
        Debug.Log($"{isMoving}");

        Vector2 direction = Vector2.zero;
        if (movableHeadVisual.GetCurrentAnimationState() == MovableHeadVisual.AnimationStates.RockHeadLeftHit ||
            movableHeadVisual.GetCurrentAnimationState() == MovableHeadVisual.AnimationStates.SpikeHeadLeftHit)
            direction = Vector2.left;
        else if (movableHeadVisual.GetCurrentAnimationState() == MovableHeadVisual.AnimationStates.RockHeadRightHit ||
            movableHeadVisual.GetCurrentAnimationState() == MovableHeadVisual.AnimationStates.SpikeHeadRightHit)
            direction = Vector2.right;
        else if (movableHeadVisual.GetCurrentAnimationState() == MovableHeadVisual.AnimationStates.RockHeadTopHit ||
            movableHeadVisual.GetCurrentAnimationState() == MovableHeadVisual.AnimationStates.SpikeHeadTopHit)
            direction = Vector2.up;
        else if (movableHeadVisual.GetCurrentAnimationState() == MovableHeadVisual.AnimationStates.RockHeadBottomHit ||
            movableHeadVisual.GetCurrentAnimationState() == MovableHeadVisual.AnimationStates.SpikeHeadBottomHit)
            direction = Vector2.down;

        if(!isCanDamage)
            movableHeadVisual.ChangeAnimationState(MovableHeadVisual.AnimationStates.RockHeadIddle);
        else
            movableHeadVisual.ChangeAnimationState(MovableHeadVisual.AnimationStates.SpikeHeadIddle);

        transform.position += (Vector3)direction * distanceToMoveForHit;
    }

    public void ChangeHeadState(bool isDamageble)
    {
        isCanDamage = isDamageble;

        if (!isDamageble)
            movableHeadVisual.ChangeAnimationState(MovableHeadVisual.AnimationStates.RockHeadIddle);
        else
            movableHeadVisual.ChangeAnimationState(MovableHeadVisual.AnimationStates.SpikeHeadIddle);
    }

    public void ChangeIsCanBeMovedState(bool newState)
    {
        isCanBeMoved = newState;
    }

    public bool GetIsCanDamage()
    {
        return isCanDamage;
    }

    public PlayerController GetInteractedPlayer()
    {
        return lastInteractedPlayer;
    }

    public static void ResetStaticData()
    {
        OnTimerChange = null;
    }
}
