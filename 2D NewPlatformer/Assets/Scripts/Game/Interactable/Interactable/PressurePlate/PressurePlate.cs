using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PressurePlateVisual))]
public class PressurePlate : MonoBehaviour
{
    public event EventHandler OnPressurePlateActivated;

    [Header("Base Settings")]

    protected MovableHead standingHead;
    private bool isInteracted = false;

    private float checkTime = 0.1f;
    private float checkTimer;

    //DELETE
    [Header("Temporary")]
    [SerializeField] protected BoxCollider2D collision;

    protected List<InteractableItem> interactableItems = new List<InteractableItem>();
    protected int lastInteractedItemIndex;

    protected ObjectVisual onInteractVisual;

    protected virtual void Awake()
    {
        collision = GetComponent<BoxCollider2D>();
        interactableItems.AddRange(GetComponents<InteractableItem>());
        onInteractVisual = GetComponent<ObjectVisual>();

        checkTimer = checkTime;
    }

    private void Update()
    {
        checkTimer -= Time.deltaTime;
        if (checkTimer <= 0)
        {
            checkTimer = checkTime;

            Vector3 rightCastPosition = transform.position + (Vector3)collision.
                offset + new Vector3(collision.size.x / 2, 0f, 0f);
            Vector3 leftCastPosition = transform.position + (Vector3)collision.
                offset + new Vector3(-collision.size.x / 2, 0f, 0f);
            float castDistance = 0.2f;

            MovableHead rightCollidedHead;
            MovableHead leftCollidedHead;
            RaycastHit2D[] leftRaycast = Physics2D.RaycastAll(leftCastPosition, Vector2.up, castDistance);
            RaycastHit2D[] rightRaycast = Physics2D.RaycastAll(rightCastPosition, Vector2.up, castDistance);
            for (int i = 0; i < leftRaycast.Length; i++)
            {
                for (int j = 0; j < rightRaycast.Length; j++)
                {
                    if (leftRaycast[i] && rightRaycast[j] && leftRaycast[i].collider.gameObject.
                        TryGetComponent<MovableHead>(out leftCollidedHead) && rightRaycast[j].
                        collider.gameObject.TryGetComponent<MovableHead>(out rightCollidedHead))
                    {
                        if (!isInteracted)
                            if (rightCollidedHead == leftCollidedHead)
                            {
                                standingHead = leftCollidedHead;
                                OnInteract();
                                isInteracted = true;
                            }
                        return;
                    }
                }
            }
            if (isInteracted)
            {
                OnInteract();
                isInteracted = false;
            }
        }
    }

    public virtual void OnInteract()
    {
        OnPressurePlateActivated?.Invoke(this, EventArgs.Empty);
        onInteractVisual.OnInteractChangeAnimationState();
        for (int i = 0; i < interactableItems.Count; i++)
        {
            if (interactableItems[i].IsCanInteract())
            {
                interactableItems[i].OnInteract(standingHead.GetInteractedPlayer());
                if (interactableItems[i].GetIsChangeCameraOnInteract())
                {
                    interactableItems[i].OnAllInteractionsFinished += InteractableItem_OnAllInteractionsFinished;
                    lastInteractedItemIndex = i;
                    break;
                }
            }
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
                interactableItems[i].OnInteract(standingHead.GetInteractedPlayer());
                if (interactableItems[i].GetIsChangeCameraOnInteract())
                {
                    interactableItems[i].OnAllInteractionsFinished += InteractableItem_OnAllInteractionsFinished;
                    lastInteractedItemIndex = i;
                    break;
                }
            }
        }
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
