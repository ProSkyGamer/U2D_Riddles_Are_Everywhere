using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddInteractButtonUI : MonoBehaviour
{
    public enum InteractCastForm
    {
        Default,
        MovingPlatform,
    }
    [SerializeField] protected InteractCastForm interactCastForm = InteractCastForm.Default;

    [Header("Base Settings")]
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected TextTranslationsSO buttonTextTranslationsSO;

    [SerializeField] protected float interactableHeight = 1f;
    protected PlayerController interactedPlayer;
    [SerializeField] private bool isInteractDistanceShow = true;

    [Header("Additional Player Interaction Settings")]
    [SerializeField] protected PlayerSO[] notInteractablePlayers;

    protected bool isHasButtonOnInterface = false;
    private bool isAllInteractionsFinished = true;

    //DELETE
    [Header("Temporary")]
    [SerializeField] protected BoxCollider2D collision;

    protected List<InteractableItem> interactableItems = new();
    protected int lastInteractedItemIndex;

    protected ObjectVisual onInteractVisual;

    protected virtual void Awake()
    {
        collision = GetComponent<BoxCollider2D>();
        interactableItems.AddRange(GetComponents<InteractableItem>());
        onInteractVisual = GetComponent<ObjectVisual>();
    }

    protected virtual void Update()
    {
        if (IsAnySourceInteractable() && isAllInteractionsFinished)
        {
            switch (interactCastForm)
            {
                case InteractCastForm.Default:
                    Vector3 castPosition = transform.position + (Vector3)collision.offset;
                    Vector2 castCubeLenght = collision.size + new Vector2(interactableHeight, interactableHeight);
                    float cubeRotation = 0f;
                    Vector2 cubeDirection = Vector2.up;
                    float distance = 0f;

                    RaycastHit2D raycastHit = Physics2D.BoxCast(castPosition, castCubeLenght,
                        cubeRotation, cubeDirection, distance, playerLayer);
                    if (raycastHit)
                        if (raycastHit.collider.gameObject.TryGetComponent<PlayerController>(out interactedPlayer))
                        {
                            if (!isHasButtonOnInterface)
                                AddInteractButtonToInterafce();
                            return;
                        }

                    if (isHasButtonOnInterface)
                        RemoveInteractButtonFromInterafce();
                    break;
                case InteractCastForm.MovingPlatform:
                    castPosition = transform.position + new Vector3(0f, interactableHeight / 2, 0f);
                    castCubeLenght = new Vector2(collision.size.x, interactableHeight);
                    cubeRotation = 0f;
                    cubeDirection = Vector2.up;
                    float additionCubeLength = 0f;

                    raycastHit = Physics2D.BoxCast(castPosition, castCubeLenght,
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
                    break;
            }
        }
        else if (isHasButtonOnInterface)
            RemoveInteractButtonFromInterafce();
    }

    protected virtual void OnDrawGizmosSelected()
    {
        if (isInteractDistanceShow)
        {
            Gizmos.color = Color.blue;
            switch (interactCastForm)
            {
                case InteractCastForm.Default:
                    Vector3 castPosition = collision.transform.position + (Vector3)collision.offset;
                    Vector2 castCubeLenght = collision.size + new Vector2(interactableHeight, interactableHeight);
                    Gizmos.DrawCube(castPosition, castCubeLenght);
                    break;
                case InteractCastForm.MovingPlatform:
                    castPosition = transform.position + new Vector3(0f, interactableHeight / 2, 0f);
                    castCubeLenght = new Vector2(collision.size.x, interactableHeight);
                    Gizmos.DrawCube(castPosition, castCubeLenght);
                    break;
            }
        }
    }

    protected virtual void AddInteractButtonToInterafce()
    {
        InteractInterface.Instance.AddButtonInteractToScreen(this, buttonTextTranslationsSO);
        isHasButtonOnInterface = true;
    }

    public virtual void RemoveInteractButtonFromInterafce()
    {
        InteractInterface.Instance.RemoveButtonInteractToScreen(this);
        isHasButtonOnInterface = false;
    }

    public virtual void OnInteract()
    {
        RemoveInteractButtonFromInterafce();
        if (IsPlayerCanInteract(PlayerChangeController.Instance.GetCurrentPlayerSO()))
        {
            isAllInteractionsFinished = false;

            onInteractVisual.OnInteractChangeAnimationState();
            for (int i = 0; i < interactableItems.Count; i++)
            {
                if (interactableItems[i].IsCanInteract())
                {
                    interactableItems[i].OnInteract(interactedPlayer);
                    if (interactableItems[i].GetIsChangeCameraOnInteract())
                    {
                        interactableItems[i].OnAllInteractionsFinished += InteractableItem_OnAllInteractionsFinished;
                        lastInteractedItemIndex = i;
                        return;
                    }
                }
            }

            isAllInteractionsFinished = true;
        }
        else
        {
            if (TextTranslationManager.GetCurrentLanguage() == TextTranslationManager.Languages.English)
                NottificationsUI.Instance.AddNotification("This player can't interact!");
            else
                NottificationsUI.Instance.AddNotification("Этот игрок не может взаимодейстовать!");
        }
    }

    protected virtual void InteractableItem_OnAllInteractionsFinished(object sender, System.EventArgs e)
    {
        InteractableItem item = sender as InteractableItem;
        item.OnAllInteractionsFinished -= InteractableItem_OnAllInteractionsFinished;

        for (int i = lastInteractedItemIndex + 1; i < interactableItems.Count; i++)
        {
            if (interactableItems[i].IsCanInteract())
            {
                interactableItems[i].OnInteract(interactedPlayer);
                if (interactableItems[i].GetIsChangeCameraOnInteract())
                {
                    interactableItems[i].OnAllInteractionsFinished += InteractableItem_OnAllInteractionsFinished;
                    lastInteractedItemIndex = i;
                    return;
                }
            }
        }
        isAllInteractionsFinished = true;
    }

    protected bool IsPlayerCanInteract(PlayerSO playerSO)
    {
        for (int i = 0; i < notInteractablePlayers.Length; i++)
        {
            if (notInteractablePlayers[i] == playerSO)
                return false;
        }

        return true;
    }

    protected bool IsAnySourceInteractable()
    {
        foreach (var interactableItem in interactableItems)
        {
            if (interactableItem.IsCanInteract())
                return true;
        }

        return false;
    }
}
