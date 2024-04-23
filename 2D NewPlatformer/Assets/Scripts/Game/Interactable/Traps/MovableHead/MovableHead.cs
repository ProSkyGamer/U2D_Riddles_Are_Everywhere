using System;
using System.Collections.Generic;
using UnityEngine;

// ReSharper disable CompareOfFloatsByEqualityOperator

[RequireComponent(typeof(MovableHeadVisual))]
public class MovableHead : InteractableItem
{
    #region Events

    public static event EventHandler<OnTimerChangeEventArgs> OnTimerChange;

    public class OnTimerChangeEventArgs : EventArgs
    {
        public float currentTime;
        public float maxTime;
    }

    #endregion

    #region Enums

    private enum Directions
    {
        Right,
        Left,
        Up,
        Down
    }

    #endregion

    #region Variables & References

    [SerializeField] private List<Transform> movableHeadDestinationPoints;
    private int currentDestinationPointIndex;
    [SerializeField] private float distanceToMoveForHit = 1f;
    [SerializeField] private float standingTimeForStartMoving = 0.5f;
    [SerializeField] private bool isCanDamage;
    private bool isCanBeMoved = true;
    private float standingTimerForStartMoving;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask terrainLayer;

    [SerializeField] private BoxCollider2D collision;

    private readonly List<Directions> availableDirections = new();
    private readonly float timeBetweenBlink = 3f;
    private float timerBetweenBlink;
    private bool isMoving;
    private PlayerController lastInteractedPlayer;

    private MovableHeadVisual movableHeadVisual;

    [SerializeField] protected int trapDamage = 1;
    [SerializeField] protected float dealDamageCooldown = 1.5f;
    private float dealDamageTimer;
    private readonly List<PlayerController> playerToDamageList = new();

    #endregion

    #region Initialization

    protected void Awake()
    {
        standingTimerForStartMoving = standingTimeForStartMoving;
        timerBetweenBlink = timeBetweenBlink;
        movableHeadVisual = GetComponent<MovableHeadVisual>();
    }

    private void Start()
    {
        Input.Instance.OnTestingKeyAction += Instance_OnTestingKeyAction;
    }

    private void Instance_OnTestingKeyAction(object sender, EventArgs e)
    {
        foreach (var direction in availableDirections) print(direction);
    }

    #endregion

    #region Update & Connected

    protected void Update()
    {
        if (isCanDamage)
        {
            if (dealDamageTimer > 0)
            {
                dealDamageTimer -= Time.deltaTime;
            }
            else
            {
                if (playerToDamageList.Count > 0)
                {
                    foreach (var playerToDamage in playerToDamageList) playerToDamage.TakeDamage(trapDamage);
                    dealDamageTimer = dealDamageCooldown;
                }
            }
        }

        if (!isMoving && isCanBeMoved)
        {
            timerBetweenBlink -= Time.deltaTime;
            if (timerBetweenBlink <= 0)
            {
                movableHeadVisual.ChangeAnimationState(!isCanDamage
                    ? MovableHeadVisual.AnimationStates.RockHeadBlink
                    : MovableHeadVisual.AnimationStates.SpikeHeadBlink);

                timerBetweenBlink = timeBetweenBlink;
            }

            //CheckAvailableDirections();

            //TryMoveHead(); //Moving by touching from side
        }
    }

    public override void OnInteract(PlayerController player)
    {
        base.OnInteract(player);

        lastInteractedPlayer = player;
        StartCubeMove(Vector2.zero);
        if (!isMoving)
            isInteractable = true;
    }

    private void TryMoveHead()
    {
        var cubeRotation = 0f;
        var cubeDirection = Vector2.up;
        var interactableHeight = 0f;
        var collisionSize = collision.size;

        var baseCastPosition = transform.position + (Vector3)collision.offset;
        Vector2 leftRightCastCubeLenght = new(collisionSize.x / 4, collisionSize.y * 0.75f);

        if (availableDirections.Contains(Directions.Right))
        {
            var leftCastPosition = baseCastPosition +
                                   new Vector3(-collision.size.x / 2 - leftRightCastCubeLenght.x / 2, 0f, 0f);

            var leftRaycastHit = Physics2D.BoxCast(leftCastPosition, leftRightCastCubeLenght,
                cubeRotation, cubeDirection, interactableHeight, playerLayer);

            if (leftRaycastHit)
            {
                if (leftRaycastHit.rigidbody.gameObject.TryGetComponent(out PlayerController interactedPlayer))
                {
                    lastInteractedPlayer = interactedPlayer;
                    standingTimerForStartMoving -= Time.deltaTime;
                    OnTimerChange?.Invoke(this, new OnTimerChangeEventArgs
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

        if (availableDirections.Contains(Directions.Left))
        {
            var rightCastPosition = baseCastPosition +
                                    new Vector3(collision.size.x / 2 + leftRightCastCubeLenght.x / 2, 0f, 0f);

            var rightRaycastHit = Physics2D.BoxCast(rightCastPosition, leftRightCastCubeLenght,
                cubeRotation, cubeDirection, interactableHeight, playerLayer);
            if (rightRaycastHit)
            {
                if (rightRaycastHit.rigidbody.gameObject.TryGetComponent(out interactedPlayer))
                {
                    lastInteractedPlayer = interactedPlayer;
                    standingTimerForStartMoving -= Time.deltaTime;
                    OnTimerChange?.Invoke(this, new OnTimerChangeEventArgs
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

        Vector2 topBottomCastCubeLenght = new(collisionSize.x * 0.75f, collisionSize.y / 2);

        if (availableDirections.Contains(Directions.Down))
        {
            var topCastPosition = baseCastPosition +
                                  new Vector3(0f, collision.size.y / 2 + topBottomCastCubeLenght.y / 2, 0f);

            var topRaycastHit = Physics2D.BoxCast(topCastPosition, topBottomCastCubeLenght,
                cubeRotation, cubeDirection, interactableHeight, playerLayer);
            if (topRaycastHit)
            {
                if (topRaycastHit.rigidbody.gameObject.TryGetComponent(out interactedPlayer))
                {
                    lastInteractedPlayer = interactedPlayer;
                    standingTimerForStartMoving -= Time.deltaTime;
                    OnTimerChange?.Invoke(this, new OnTimerChangeEventArgs
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

        if (availableDirections.Contains(Directions.Up))
        {
            var bottomCastPosition = baseCastPosition +
                                     new Vector3(0f, -collision.size.y / 2 - topBottomCastCubeLenght.y / 2, 0f);

            var bottomRaycastHit = Physics2D.BoxCast(bottomCastPosition, topBottomCastCubeLenght,
                cubeRotation, cubeDirection, interactableHeight, playerLayer);
            if (bottomRaycastHit)
            {
                if (bottomRaycastHit.rigidbody.gameObject.TryGetComponent(out interactedPlayer))
                {
                    lastInteractedPlayer = interactedPlayer;
                    standingTimerForStartMoving -= Time.deltaTime;
                    OnTimerChange?.Invoke(this, new OnTimerChangeEventArgs
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

    private void CheckAvailableDirections()
    {
        if (movableHeadVisual.GetCurrentAnimationState() == MovableHeadVisual.AnimationStates.RockHeadIdle ||
            movableHeadVisual.GetCurrentAnimationState() == MovableHeadVisual.AnimationStates.SpikeHeadIdle)
            isMoving = false;

        availableDirections.Clear();

        var cubeRotation = 0f;
        var interactableHeight = 0f;

        var baseCastPosition = transform.position;
        var castCubeLenght = collision.size * 0.99f;

        var leftCastPosition = baseCastPosition + new Vector3(-distanceToMoveForHit, 0f, 0f);
        var leftRaycastHit = Physics2D.BoxCastAll(leftCastPosition, castCubeLenght,
            cubeRotation, Vector2.left, interactableHeight, terrainLayer);
        if (leftRaycastHit.Length == 1)
            availableDirections.Add(Directions.Left);

        var rightCastPosition = baseCastPosition + new Vector3(distanceToMoveForHit, 0f, 0f);
        var rightRaycastHit = Physics2D.BoxCastAll(rightCastPosition, castCubeLenght,
            cubeRotation, Vector2.right, interactableHeight, terrainLayer);
        if (rightRaycastHit.Length == 1)
            availableDirections.Add(Directions.Right);

        var topCastPosition = baseCastPosition + new Vector3(0f, distanceToMoveForHit, 0f);

        var topRaycastHit = Physics2D.BoxCastAll(topCastPosition, castCubeLenght,
            cubeRotation, Vector2.up, interactableHeight, terrainLayer);

        if (topRaycastHit.Length == 1)
            availableDirections.Add(Directions.Up);

        var bottomCastPosition = baseCastPosition + new Vector3(0f, -distanceToMoveForHit, 0f);

        var bottomRaycastHit = Physics2D.BoxCastAll(bottomCastPosition, castCubeLenght,
            cubeRotation, Vector2.down, interactableHeight, terrainLayer);

        if (bottomRaycastHit.Length == 1)
            availableDirections.Add(Directions.Down);
    }

    #endregion

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        var collisionSize = collision.size;
        var baseCastPosition = transform.position + (Vector3)collision.offset;
        Vector2 leftRightCastCubeLenght = new(collisionSize.x / 4, collisionSize.y * 0.75f);
        Vector2 topBottomCastCubeLenght = new(collisionSize.x * 0.75f, collisionSize.y / 4);

        var leftCastPosition =
            baseCastPosition + new Vector3(-collisionSize.x / 2 - leftRightCastCubeLenght.x / 2, 0f, 0f);
        Gizmos.DrawCube(leftCastPosition, leftRightCastCubeLenght);

        var rightCastPosition =
            baseCastPosition + new Vector3(collision.size.x / 2 + leftRightCastCubeLenght.x / 2, 0f, 0f);
        Gizmos.DrawCube(rightCastPosition, leftRightCastCubeLenght);

        var topCastPosition =
            baseCastPosition + new Vector3(0f, collision.size.y / 2 + topBottomCastCubeLenght.y / 2, 0f);
        Gizmos.DrawCube(topCastPosition, topBottomCastCubeLenght);

        var bottomCastPosition =
            baseCastPosition + new Vector3(0f, -collision.size.y / 2 - topBottomCastCubeLenght.y / 2, 0f);
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

    #region Movement

    private void StartCubeMove(Vector2 direction)
    {
        if (movableHeadDestinationPoints.Count <= 1) return;
        if (!isCanBeMoved) return;

        var nextDestinationPointIndex = GetNextDestinationPointIndex();
        var nextDestinationPoint = movableHeadDestinationPoints[nextDestinationPointIndex];
        var nextDestinationPointAvailability = CheckNextDestinationPointAvailability(nextDestinationPoint);

        if (nextDestinationPointAvailability)
        {
            currentDestinationPointIndex = nextDestinationPointIndex;
            isMoving = true;
            movableHeadVisual.ChangeAnimationState(isCanDamage
                ? MovableHeadVisual.AnimationStates.SpikeHeadMove
                : MovableHeadVisual.AnimationStates.RockHeadMove);
        }


        /*standingTimerForStartMoving = standingTimeForStartMoving;
        isMoving = true;

        if (!isCanDamage)
        {
            if (direction.x == 1f)
            {
                if (availableDirections.Contains(Directions.Right))
                {
                    movableHeadVisual.ChangeAnimationState(MovableHeadVisual.AnimationStates.RockHeadRightHit);
                    return;
                }
            }
            else if (direction.x == -1f)
            {
                if (availableDirections.Contains(Directions.Left))
                {
                    movableHeadVisual.ChangeAnimationState(MovableHeadVisual.AnimationStates.RockHeadLeftHit);
                    return;
                }
            }
            else if (direction.y == 1f)
            {
                if (availableDirections.Contains(Directions.Up))
                {
                    movableHeadVisual.ChangeAnimationState(MovableHeadVisual.AnimationStates.RockHeadTopHit);
                    return;
                }
            }
            else if (direction.y == -1f)
            {
                if (availableDirections.Contains(Directions.Down))
                {
                    movableHeadVisual.ChangeAnimationState(MovableHeadVisual.AnimationStates.RockHeadBottomHit);
                    return;
                }
            }
            else
            {
                Debug.Log("�Ѩ �����! MovableHead MoveCube without direction");
            }
        }
        else
        {
            if (direction.x == 1f)
            {
                if (availableDirections.Contains(Directions.Right))
                {
                    movableHeadVisual.ChangeAnimationState(MovableHeadVisual.AnimationStates.SpikeHeadRightHit);
                    return;
                }
            }
            else if (direction.x == -1f)
            {
                if (availableDirections.Contains(Directions.Left))
                {
                    movableHeadVisual.ChangeAnimationState(MovableHeadVisual.AnimationStates.SpikeHeadLeftHit);
                    return;
                }
            }
            else if (direction.y == 1f)
            {
                if (availableDirections.Contains(Directions.Up))
                {
                    movableHeadVisual.ChangeAnimationState(MovableHeadVisual.AnimationStates.SpikeHeadTopHit);
                    return;
                }
            }
            else if (direction.y == -1f)
            {
                if (availableDirections.Contains(Directions.Down))
                {
                    movableHeadVisual.ChangeAnimationState(MovableHeadVisual.AnimationStates.SpikeHeadBottomHit);
                    return;
                }
            }
            else
            {
                Debug.Log("�Ѩ �����! MovableHead MoveCube without direction");
            }
        }

        isMoving = false;*/
    }

    private int GetNextDestinationPointIndex()
    {
        return currentDestinationPointIndex >= movableHeadDestinationPoints.Count - 1
            ? 0
            : currentDestinationPointIndex + 1;
    }

    private bool CheckNextDestinationPointAvailability(Transform destinationPoint)
    {
        var cubeRotation = 0f;
        var interactableHeight = 0f;

        var castPosition = destinationPoint.position;
        var castCubeLenght = collision.size * 0.99f;

        var raycastHits = Physics2D.BoxCastAll(castPosition, castCubeLenght,
            cubeRotation, Vector2.left, interactableHeight, terrainLayer);

        return raycastHits.Length <= 1;
    }

    public void OnAnimationMoveMiddle()
    {
        /*var direction = Vector2.zero;
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
            direction = Vector2.down;*/

        transform.position = movableHeadDestinationPoints[currentDestinationPointIndex].position;
    }

    public void OnAnimationMoveStop()
    {
        movableHeadVisual.ChangeAnimationState(!isCanDamage
            ? MovableHeadVisual.AnimationStates.RockHeadIdle
            : MovableHeadVisual.AnimationStates.SpikeHeadIdle);

        isInteractable = true;
        isMoving = false;
    }

    #endregion

    #region Head States

    public void ChangeHeadState(bool isDamageble)
    {
        isCanDamage = isDamageble;

        movableHeadVisual.ChangeAnimationState(!isDamageble
            ? MovableHeadVisual.AnimationStates.RockHeadIdle
            : MovableHeadVisual.AnimationStates.SpikeHeadIdle);
    }

    public void ChangeIsCanBeMovedState(bool newState)
    {
        isCanBeMoved = newState;
    }

    #endregion

    #region Damage

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out var playerController))
        {
            playerToDamageList.Add(playerController);
            dealDamageTimer = 0;
        }
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out var playerController))
            playerToDamageList.Remove(playerController);
    }

    #endregion

    #region Get Head Data

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

    public override bool IsCanInteract()
    {
        return isInteractable && isCanBeMoved;
    }

    #endregion
}
