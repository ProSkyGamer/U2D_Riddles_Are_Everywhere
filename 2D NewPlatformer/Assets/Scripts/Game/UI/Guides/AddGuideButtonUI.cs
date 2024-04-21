using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddGuideButtonUI : MonoBehaviour
{
    [Header("Base Settings")]
    [SerializeField] protected GuideButtonSO guideButtonSO;

    [SerializeField] protected float interactableDistance = 1f;
    [SerializeField] private bool isInteractDistanceShow = true;

    protected bool isHasButtonOnInterface = false;

    //DELETE
    [Header("Temporary")]
    [SerializeField] protected BoxCollider2D collision;

    protected virtual void Awake()
    {
        collision = GetComponent<BoxCollider2D>();
    }

    protected virtual void Update()
    {
        Vector3 castPosition = transform.position + (Vector3)collision.offset;
        Vector2 castCubeLenght = collision.size + new Vector2(interactableDistance, interactableDistance);
        float cubeRotation = 0f;
        Vector2 cubeDirection = Vector2.up;
        float distance = 0f;

        RaycastHit2D[] raycastHits = Physics2D.BoxCastAll(castPosition, castCubeLenght,
            cubeRotation, cubeDirection, distance);

        foreach (RaycastHit2D raycastHit in raycastHits)
        {
            if (raycastHit)
                if (raycastHit.collider.gameObject.TryGetComponent(out PlayerController interactedPlayer))
                {
                    if (!isHasButtonOnInterface)
                        AddGuideButtonToInterafce();
                    return;
                }

        }
        if (isHasButtonOnInterface)
            RemoveGuideButtonFromInterafce();
    }

    protected virtual void OnDrawGizmosSelected()
    {
        if (isInteractDistanceShow)
        {
            Gizmos.color = Color.blue;
            Vector3 castPosition = collision.transform.position + (Vector3)collision.offset;
            Vector2 castCubeLenght = collision.size + new Vector2(interactableDistance, interactableDistance);
            Gizmos.DrawCube(castPosition, castCubeLenght);
        }
    }

    protected virtual void AddGuideButtonToInterafce()
    {
        if (!GuideButtonsUI.Instance.IsCurrentGuideCreated(guideButtonSO))
        {
            GuideButtonsUI.Instance.AddGuideButtonToScreen(guideButtonSO);
            isHasButtonOnInterface = true;
        }
    }

    public virtual void RemoveGuideButtonFromInterafce()
    {
        GuideButtonsUI.Instance.RemoveGuideButtonFromScreen(guideButtonSO);
        isHasButtonOnInterface = false;
    }

    public virtual void OnInteract()
    {
        RemoveGuideButtonFromInterafce();
        GuideInterface.Instance.ShowGuide(guideButtonSO.guideSO);
    }

    private void OnDestroy()
    {
        if (isHasButtonOnInterface)
            RemoveGuideButtonFromInterafce();
    }
}
