using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FallingPlatformVisual))]
public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private BoxCollider2D platformCollision;

    [SerializeField] private float timeBeforeFalling = 1.5f;
    private float playerStandingBeforeFallingTimer;
    private bool isPlayerStanding;

    [SerializeField] protected PlayerSO[] notInteractablePlayerSOArray;

    [SerializeField] private float fallingDistance = 5f;
    [SerializeField] private float fallingTime = 3f;
    private float fallingTimer;

    private float checkableHeight = 0.2f;

    private bool isFallingTriggered;

    private FallingPlatformVisual fallingPlatformVisual;

    private void Awake()
    {
        fallingPlatformVisual = GetComponent<FallingPlatformVisual>();

        playerStandingBeforeFallingTimer = timeBeforeFalling;
        fallingTimer = fallingTime;
    }

    private void Update()
    {
        if(isPlayerStanding && !isFallingTriggered)
        {
            playerStandingBeforeFallingTimer -= Time.deltaTime;
            if (playerStandingBeforeFallingTimer <= 0)
            {
                isFallingTriggered = true;
                fallingPlatformVisual.TriggerAnimation();
            }
        }
        else if (isFallingTriggered)
        {
            fallingTimer -= Time.deltaTime;

            transform.position -= new Vector3(0f, fallingDistance * Time.deltaTime / fallingTime, 0f);

            if (fallingTimer <= 0)
                Destroy(gameObject);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 castPosition = collision.transform.position + (Vector3)platformCollision.offset;
        Vector2 castCubeLenght = platformCollision.size;
        float cubeRotation = 0f;
        Vector2 cubeDirection = Vector2.up;

        RaycastHit2D raycastHit = Physics2D.BoxCast(castPosition, castCubeLenght,
            cubeRotation, cubeDirection, checkableHeight);
        if (raycastHit)
        {
            PlayerController interactedPlayer;
            if (collision.gameObject.TryGetComponent<PlayerController>(out interactedPlayer))
            {
                if(IsCurrentPlayerInteractable(PlayerChangeController.Instance.GetCurrentPlayerSO()))
                    isPlayerStanding = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        PlayerController interactedPlayer;
        if (collision.gameObject.TryGetComponent<PlayerController>(out interactedPlayer))
        {
            isPlayerStanding = false;
        }
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 castPosition = platformCollision.transform.position + (Vector3)platformCollision.offset;
        Vector2 castCubeLenght = platformCollision.size + new Vector2(checkableHeight, checkableHeight);
        Gizmos.DrawCube(castPosition, castCubeLenght); //Область проверки на игрока

        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0f, -fallingDistance, 0f)); //Линия падения платформы
    }

    private bool IsCurrentPlayerInteractable(PlayerSO player)
    {
        foreach (PlayerSO playerSO in notInteractablePlayerSOArray)
        {
            if (playerSO == player)
                return false;
        }
        return true;
    }
}
