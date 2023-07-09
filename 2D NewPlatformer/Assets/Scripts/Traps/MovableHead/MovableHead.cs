using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovableHeadVisual))]
public class MovableHead : BaseTrap
{
    [SerializeField] private float distanceToMoveForHit = 1f;
    [SerializeField] private float standingTimeForStartMoving = 0.5f;
    [SerializeField] private bool isCanDamage = false;
    private float standingTimerForStartMoving;
    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private BoxCollider2D collision;

    private float timeBetwenBlink = 3f;
    private float timerBetwenBlink;
    private bool isMoving = false;

    private MovableHeadVisual movableHeadVisual;

    protected override void Awake()
    {
        base.Awake();

        standingTimerForStartMoving = standingTimeForStartMoving;
        timerBetwenBlink = timeBetwenBlink;
        movableHeadVisual = GetComponent<MovableHeadVisual>();
    }

    protected void Start()
    {
        //DELETE
        Input.Instance.OnTestingKeyAction += Input_OnTestingKeyAction;
    }

    private void Input_OnTestingKeyAction(object sender, System.EventArgs e)
    {
        ChangeHeadState(!isCanDamage);
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

        if (!isMoving)
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

            float cubeRotation = 0f;
            Vector2 cubeDirection = Vector2.up;
            float interactableHeight = 0f;

            Vector3 baseCastPosition = transform.position + (Vector3)collision.offset;
            Vector2 leftRightCastCubeLenght = new Vector2(collision.size.x / 2, collision.size.y);

            Vector3 leftCastPosition = baseCastPosition + new Vector3(-collision.size.x / 2 - leftRightCastCubeLenght.x / 2, 0f, 0f);

            RaycastHit2D leftRaycastHit = Physics2D.BoxCast(leftCastPosition, leftRightCastCubeLenght,
                cubeRotation, cubeDirection, interactableHeight, playerLayer);

            PlayerController interactedPlayer;
            if (leftRaycastHit)
            {
                if (leftRaycastHit.rigidbody.gameObject.TryGetComponent<PlayerController>(out interactedPlayer))
                {
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
                if (rightRaycastHit.rigidbody.gameObject.TryGetComponent<PlayerController>(out interactedPlayer))
                {
                    standingTimerForStartMoving -= Time.deltaTime;
                    if (standingTimerForStartMoving <= 0)
                        StartCubeMove(Vector2.left);
                }
                return;
            }

            Vector2 topBottomCastCubeLenght = new Vector2(collision.size.x, collision.size.y / 2);

            Vector3 topCastPosition = baseCastPosition + new Vector3(0f, collision.size.y / 2 + topBottomCastCubeLenght.y / 2, 0f);

            RaycastHit2D topRaycastHit = Physics2D.BoxCast(topCastPosition, topBottomCastCubeLenght,
                cubeRotation, cubeDirection, interactableHeight, playerLayer);
            if (topRaycastHit)
            {
                if (topRaycastHit.rigidbody.gameObject.TryGetComponent<PlayerController>(out interactedPlayer))
                {
                    standingTimerForStartMoving -= Time.deltaTime;
                    if (standingTimerForStartMoving <= 0)
                        StartCubeMove(Vector2.down);
                }
                return;
            }

            Vector3 bottomCastPosition = baseCastPosition + new Vector3(0f, -collision.size.y / 2 - topBottomCastCubeLenght.y / 2, 0f);

            RaycastHit2D bottomRaycastHit = Physics2D.BoxCast(bottomCastPosition, topBottomCastCubeLenght,
                cubeRotation, cubeDirection, interactableHeight, playerLayer);
            if (bottomRaycastHit)
            {
                if (bottomRaycastHit.rigidbody.gameObject.TryGetComponent<PlayerController>(out interactedPlayer))
                {
                    standingTimerForStartMoving -= Time.deltaTime;
                    if (standingTimerForStartMoving <= 0)
                        StartCubeMove(Vector2.up);
                }
                return;
            }

            standingTimerForStartMoving = standingTimeForStartMoving;
        }
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        Vector3 baseCastPosition = transform.position + (Vector3)collision.offset;
        Vector2 leftRightCastCubeLenght = new Vector2(collision.size.x / 4, collision.size.y);
        Vector2 topBottomCastCubeLenght = new Vector2(collision.size.x, collision.size.y / 4);

        Vector3 leftCastPosition = baseCastPosition + new Vector3(-collision.size.x / 2 - leftRightCastCubeLenght.x / 2, 0f, 0f);
        Gizmos.DrawCube(leftCastPosition, leftRightCastCubeLenght);

        Vector3 rightCastPosition = baseCastPosition + new Vector3(collision.size.x / 2 + leftRightCastCubeLenght.x / 2, 0f, 0f);
        Gizmos.DrawCube(rightCastPosition, leftRightCastCubeLenght);

        Vector3 topCastPosition = baseCastPosition + new Vector3(0f, collision.size.y / 2 + topBottomCastCubeLenght.y / 2, 0f);
        Gizmos.DrawCube(topCastPosition, topBottomCastCubeLenght);

        Vector3 bottomCastPosition = baseCastPosition + new Vector3(0f, -collision.size.y / 2 - topBottomCastCubeLenght.y / 2, 0f);
        Gizmos.DrawCube(bottomCastPosition, topBottomCastCubeLenght);
    }

    private void StartCubeMove(Vector2 direction)
    {
        standingTimerForStartMoving = standingTimeForStartMoving;
        isMoving = true;

        if (!isCanDamage)
        {
            if (direction.x == 1f)
                movableHeadVisual.ChangeAnimationState(MovableHeadVisual.AnimationStates.RockHeadRightHit);
            else if (direction.x == -1f)
                movableHeadVisual.ChangeAnimationState(MovableHeadVisual.AnimationStates.RockHeadLeftHit);
            else if (direction.x == 1f)
                movableHeadVisual.ChangeAnimationState(MovableHeadVisual.AnimationStates.RockHeadTopHit);
            else if (direction.x == -1f)
                movableHeadVisual.ChangeAnimationState(MovableHeadVisual.AnimationStates.RockHeadBottomHit);
            else
                Debug.Log("ÂÑ¨ ÏÈÇÄÀ! MovableHead MoveCube without direction");
        }
        else
        {
            if (direction.x == 1f)
                movableHeadVisual.ChangeAnimationState(MovableHeadVisual.AnimationStates.SpikeHeadRightHit);
            else if (direction.x == -1f)
                movableHeadVisual.ChangeAnimationState(MovableHeadVisual.AnimationStates.SpikeHeadLeftHit);
            else if (direction.x == 1f)
                movableHeadVisual.ChangeAnimationState(MovableHeadVisual.AnimationStates.SpikeHeadTopHit);
            else if (direction.x == -1f)
                movableHeadVisual.ChangeAnimationState(MovableHeadVisual.AnimationStates.SpikeHeadBottomHit);
            else
                Debug.Log("ÂÑ¨ ÏÈÇÄÀ! MovableHead MoveCube without direction");
        }
    }

    public void OnAnimationHitStop()
    {
        isMoving = false;

        Vector2 direction = Vector2.zero;
        if (movableHeadVisual.GetCurrentAnimationState() == MovableHeadVisual.AnimationStates.RockHeadLeftHit)
            direction = Vector2.left;
        else if (movableHeadVisual.GetCurrentAnimationState() == MovableHeadVisual.AnimationStates.RockHeadRightHit)
            direction = Vector2.right;
        else if (movableHeadVisual.GetCurrentAnimationState() == MovableHeadVisual.AnimationStates.RockHeadTopHit)
            direction = Vector2.up;
        else if (movableHeadVisual.GetCurrentAnimationState() == MovableHeadVisual.AnimationStates.RockHeadBottomHit)
            direction = Vector2.down;

        movableHeadVisual.ChangeAnimationState(MovableHeadVisual.AnimationStates.RockHeadIddle);

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

    public bool GetIsCanDamage()
    {
        return isCanDamage;
    }
}
