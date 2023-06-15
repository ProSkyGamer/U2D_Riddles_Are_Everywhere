using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableItem : MonoBehaviour
{
    [Header("Base Settings")]
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected string buttonText = "Interact";
    
    [SerializeField] protected float interactableHeight = 1f;
    protected PlayerController interactedPlayer;

    protected bool isInteractable = true;
    protected bool isHasButtonOnInterface = false;

    //DELETE
    [Header("Temporary")]
    [SerializeField] protected BoxCollider2D collision;

    [Header("Change View Settings")]
    [SerializeField] protected bool isChangeCameraFollowingObjectOnInteract = false;
    [SerializeField] protected float cameraChangeDuration = 1.5f;

    protected virtual void Awake()
    {
        collision = GetComponent<BoxCollider2D>();
    }

    protected virtual void Update()
    {
        if (isInteractable)
        {
            Vector3 castPosition = collision.transform.position + (Vector3)collision.offset;
            Vector2 castCubeLenght = collision.size;
            float cubeRotation = 0f;
            Vector2 cubeDirection = Vector2.up;

            RaycastHit2D raycastHit = Physics2D.BoxCast(castPosition, castCubeLenght,
                cubeRotation, cubeDirection, interactableHeight, playerLayer);
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
    }

    public virtual void OnInteract()
    {
        isInteractable = false;
        isHasButtonOnInterface = false;

        if (isChangeCameraFollowingObjectOnInteract)
        {
            ChangeFollowingObject();
        }
    }

    protected virtual void ChangeFollowingObject()
    {
        interactedPlayer.DisableMovement();
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 castPosition = collision.transform.position + (Vector3)collision.offset;
        Vector2 castCubeLenght = collision.size + new Vector2(interactableHeight, interactableHeight);
        Gizmos.DrawCube(castPosition, castCubeLenght);
    }

    protected void AddInteractButtonToInterafce()
    {
        InteractInterface.Instance.AddButtonInteractToScreen(this, buttonText);
        isHasButtonOnInterface = true;
    }

    protected void RemoveInteractButtonFromInterafce()
    {
        InteractInterface.Instance.RemoveButtonInteractToScreen(this);
        isHasButtonOnInterface = false;
    }

}
