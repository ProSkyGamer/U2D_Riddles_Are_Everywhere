using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowGuide : MonoBehaviour
{
    [SerializeField] private GuidesSO[] guidesToShow;

    [Header("Base Settings")]
    [SerializeField] protected LayerMask playerLayer;

    [SerializeField] protected float interactableHeight = 1f;
    private bool isGuidesWasShown = false;

    protected BoxCollider2D collision;


    protected virtual void Awake()
    {
        collision = GetComponent<BoxCollider2D>();
    }

    protected virtual void Update()
    {
        if (!isGuidesWasShown)
        {
            Vector3 castPosition = transform.position + (Vector3)collision.offset;
            Vector2 castCubeLenght = collision.size + new Vector2(interactableHeight, interactableHeight);
            float cubeRotation = 0f;
            Vector2 cubeDirection = Vector2.up;
            float distance = 0f;

            RaycastHit2D raycastHit = Physics2D.BoxCast(castPosition, castCubeLenght,
                cubeRotation, cubeDirection, distance, playerLayer);
            if (raycastHit)
                if (raycastHit.collider.gameObject.TryGetComponent<PlayerController>(out PlayerController interactedPlayer))
                {
                    GuideInterface.Instance.ShowGuide(guidesToShow);
                    isGuidesWasShown = true;
                }
        }
    }
}
