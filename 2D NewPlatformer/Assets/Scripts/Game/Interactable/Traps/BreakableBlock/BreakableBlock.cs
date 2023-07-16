using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BreakableBlockVisual))]
[RequireComponent(typeof(BreakableBlockGravity))]
public class BreakableBlock : InteractableItem
{
    [SerializeField] private float distanceToMoveForHit = 1f;
    [SerializeField] private float standingTimeForStartMoving = 0.5f;
    private float standingTimerForStartMoving;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask terrainLayer;

    [SerializeField] private BoxCollider2D collision;

    [SerializeField] private float timeToDissapersAfterBreak = 2f;
    private bool isBroken = false;
    [SerializeField] private Transform itemAppearedAfterBreak;

    public enum Directions
    {
        right,
        left,
    }

    private List<Directions> availibleDirections = new();
    private bool isMoving = false;
    private bool isNowStadingOnMovingPlatform = false;
    private MovingPlatform movingPlatformStandingOn;
    private bool isStoredInMovingPlatform;

    private BreakableBlockVisual breakableBlockVisual;
    private BreakableBlockGravity breakableBlockGravity;

    private void Awake()
    {
        standingTimerForStartMoving = standingTimeForStartMoving;
        breakableBlockVisual = GetComponent<BreakableBlockVisual>();
        breakableBlockGravity = GetComponent<BreakableBlockGravity>();

        StartCoroutine(CheckAvailibleDirections());
    }

    private void Start()
    {
        breakableBlockGravity.OnBreakableBlockDeathGroundHit += BreakableBlockGravity_OnBreakableBlockDeathGroundHit;
        Input.Instance.OnTestingKeyAction += Instance_OnTestingKeyAction;
    }

    private void Instance_OnTestingKeyAction(object sender, System.EventArgs e)
    {
        foreach (var direction in availibleDirections)
        {
            Debug.Log(direction);
        }
    }

    private void BreakableBlockGravity_OnBreakableBlockDeathGroundHit(object sender, System.EventArgs e)
    {
        isMoving = true;
        breakableBlockGravity.OnBreakableBlockGroundHit -= BreakableBlockGravity_OnBreakableBlockGroundHit;
        collision.isTrigger = true;
        breakableBlockVisual.ChangeAnimationState(BreakableBlockVisual.AnimationStates.DestroyBreakableBlock);
        breakableBlockGravity.OnBreakableBlockDeathGroundHit -= BreakableBlockGravity_OnBreakableBlockDeathGroundHit;
    }

    public void OnAnimationBreakEnd()
    {
        if (itemAppearedAfterBreak != null)
            Instantiate(itemAppearedAfterBreak, transform.position, Quaternion.identity);

        isBroken = true;
    }

    private void Update()
    {
        if (isBroken)
        {
            timeToDissapersAfterBreak -= Time.deltaTime;
            if (timeToDissapersAfterBreak <= 0)
                Destroy(gameObject);
            return;
        }

        if (!isMoving && breakableBlockGravity.IsGrounded() && !isStoredInMovingPlatform)
        {
            float cubeRotation = 0f;
            Vector2 cubeDirection = Vector2.up;
            float interactableHeight = 0f;

            Vector3 baseCastPosition = transform.position + (Vector3)collision.offset;
            Vector2 leftRightCastCubeLenght = new(collision.size.x / 2, collision.size.y);

            Vector3 leftCastPosition = baseCastPosition + new Vector3(-collision.size.x / 2 - leftRightCastCubeLenght.x / 2, 0f, 0f);

            RaycastHit2D leftRaycastHit = Physics2D.BoxCast(leftCastPosition, leftRightCastCubeLenght,
                cubeRotation, cubeDirection, interactableHeight, playerLayer);

            PlayerController currentInteractedPlayer;
            if (leftRaycastHit)
            {
                if (leftRaycastHit.rigidbody.gameObject.TryGetComponent<PlayerController>(out currentInteractedPlayer))
                {
                    if(isNowStadingOnMovingPlatform)
                        standingTimerForStartMoving -= Time.deltaTime / 5;
                    else
                        standingTimerForStartMoving -= Time.deltaTime;
                    if (standingTimerForStartMoving <= 0)
                        StartCubeMove(Vector2.right);
                }
                return;
            }

            Vector3 rightCastPosition = baseCastPosition + new Vector3(collision.size.x / 2 + leftRightCastCubeLenght.x / 2, 0f, 0f);

            RaycastHit2D rightRaycastHit = Physics2D.BoxCast(rightCastPosition, leftRightCastCubeLenght,
                cubeRotation, cubeDirection, interactableHeight, playerLayer);
            if (rightRaycastHit)
            {
                if (rightRaycastHit.rigidbody.gameObject.TryGetComponent<PlayerController>(out currentInteractedPlayer))
                {
                    if (isNowStadingOnMovingPlatform)
                        standingTimerForStartMoving -= Time.deltaTime / 5;
                    else
                        standingTimerForStartMoving -= Time.deltaTime;
                    if (standingTimerForStartMoving <= 0)
                        StartCubeMove(Vector2.left);
                }
                return;
            }

            standingTimerForStartMoving = standingTimeForStartMoving;
        }
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 baseCastPosition = transform.position + (Vector3)collision.offset;
        Vector2 castCubeLenght = collision.size * 0.98f;

        Vector3 leftCastPosition = baseCastPosition + new Vector3(-distanceToMoveForHit, 0f, 0f);
        Gizmos.DrawCube(leftCastPosition, castCubeLenght);

        Vector3 rightCastPosition = baseCastPosition + new Vector3(distanceToMoveForHit, 0f, 0f);
        Gizmos.DrawCube(rightCastPosition, castCubeLenght);

        Gizmos.color = Color.blue;

        Vector2 leftRightCastCubeLenght = new(collision.size.x / 4, collision.size.y);

        leftCastPosition = baseCastPosition + new Vector3(-collision.size.x / 2 - leftRightCastCubeLenght.x / 2, 0f, 0f);
        Gizmos.DrawCube(leftCastPosition, leftRightCastCubeLenght);

        rightCastPosition = baseCastPosition + new Vector3(collision.size.x / 2 + leftRightCastCubeLenght.x / 2, 0f, 0f);
        Gizmos.DrawCube(rightCastPosition, leftRightCastCubeLenght);
    }

    private IEnumerator CheckAvailibleDirections()
    {
        availibleDirections.Clear();
        yield return new WaitForSeconds(0.1f);

        if (breakableBlockGravity.IsGrounded())
        {
            float cubeRotation = 0f;
            float interactableHeight = 0f;

            Vector3 baseCastPosition = transform.position;
            Vector2 castCubeLenght = collision.size * 0.98f;

            Vector3 leftCastPosition = baseCastPosition + new Vector3(-distanceToMoveForHit, 0f, 0f);
            RaycastHit2D[] leftRaycastHit = Physics2D.BoxCastAll(leftCastPosition, castCubeLenght,
                cubeRotation, Vector2.left, interactableHeight, terrainLayer);

            bool isFound = false;
            foreach (var hit in leftRaycastHit)
            {
                isFound = false;
                if (!hit.collider.isTrigger && hit.collider != collision)
                {
                    isFound = true;
                    break;
                }
            }
            if (!isFound)
                availibleDirections.Add(Directions.left);

            Vector3 rightCastPosition = baseCastPosition + new Vector3(distanceToMoveForHit, 0f, 0f);
            RaycastHit2D[] rightRaycastHit = Physics2D.BoxCastAll(rightCastPosition, castCubeLenght,
                cubeRotation, Vector2.right, interactableHeight, terrainLayer);

            foreach (var hit in rightRaycastHit)
            {
                isFound = false;
                if (!hit.collider.isTrigger && hit.collider != collision)
                {
                    isFound = true;
                    break;
                }
            }
            if (!isFound)
                availibleDirections.Add(Directions.right);
        }
        else
        {
            breakableBlockGravity.OnBreakableBlockGroundHit += BreakableBlockGravity_OnBreakableBlockGroundHit;
        }
    }

    private void BreakableBlockGravity_OnBreakableBlockGroundHit(object sender, System.EventArgs e)
    {
        StartCoroutine(CheckAvailibleDirections());

        breakableBlockGravity.OnBreakableBlockGroundHit -= BreakableBlockGravity_OnBreakableBlockGroundHit;
    }

    private void StartCubeMove(Vector2 direction)
    {
        standingTimerForStartMoving = standingTimeForStartMoving;
        isMoving = true;
        ChangeIsNowStandingOnMovingPlatform(false, null);

        if (direction.x == 1f)
        {
            if (availibleDirections.Contains(Directions.right))
            {
                breakableBlockVisual.ChangeAnimationState(BreakableBlockVisual.AnimationStates.MoveRight);
                return;
            }
        }
        else if (direction.x == -1f)
        {
            if (availibleDirections.Contains(Directions.left))
            {
                breakableBlockVisual.ChangeAnimationState(BreakableBlockVisual.AnimationStates.MoveLeft);
                return;
            }
        }
        else
            Debug.Log("ÂÑ¨ ÏÈÇÄÀ! MovableHead MoveCube without direction");

        isMoving = false;
    }

    public void OnAnimationHitStop()
    {
        isMoving = false;

        Vector2 direction = Vector2.zero;
        if (breakableBlockVisual.GetCurrentAnimationState() == BreakableBlockVisual.AnimationStates.MoveLeft)
            direction = Vector2.left;
        else if (breakableBlockVisual.GetCurrentAnimationState() == BreakableBlockVisual.AnimationStates.MoveRight)
            direction = Vector2.right;

        breakableBlockVisual.ChangeAnimationState(BreakableBlockVisual.AnimationStates.Iddle);

        transform.position += (Vector3)direction * distanceToMoveForHit;

        StartCoroutine(CheckAvailibleDirections());
    }

    private void ChangeBreakableBlockStoredState(bool isStored)
    {
        isStoredInMovingPlatform = isStored;
        if (isStoredInMovingPlatform)
        {
            movingPlatformStandingOn.AddStoredBreakableBlock(this);
            collision.isTrigger = true;
        }
        else
        {
            collision.isTrigger = false;
            movingPlatformStandingOn.RemoveStoredBreakableBlock();
            StartCoroutine(CheckAvailibleDirections());
        }
    }

    public void ChangeIsNowStandingOnMovingPlatform(bool isNowStanding, MovingPlatform platformStandingOn)
    {
        isNowStadingOnMovingPlatform = isNowStanding;

        if (isNowStadingOnMovingPlatform)
        {
            movingPlatformStandingOn = platformStandingOn;
        }
    }

    public bool GetIsStandingOnMovingPlatform()
    {
        return isNowStadingOnMovingPlatform;
    }

    public void Move(Vector2 toMove)
    {
        transform.position += (Vector3)toMove;
    }

    public override bool IsCanInteract()
    {
        return isNowStadingOnMovingPlatform || isStoredInMovingPlatform;
    }

    public override void OnInteract(PlayerController player)
    {
        base.OnInteract(player);

        ChangeBreakableBlockStoredState(!isStoredInMovingPlatform);
    }
}
