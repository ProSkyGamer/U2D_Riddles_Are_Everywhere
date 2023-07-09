using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChangeMovableHeadState))]
public class ChangeMovableHeadState : InteractableItem
{
    [Header("Callback Platfrom Settings")]
    [SerializeField] private MovableHead[] movableHeadToChangeState;
    private int movableHeadToChangeStateCurrentIndex;

    private ChangeMovableHeadStateVisual changeMovableHeadStateVisual;

    protected override void Awake()
    {
        base.Awake();

        changeMovableHeadStateVisual = GetComponent<ChangeMovableHeadStateVisual>();
    }


    protected override void Update()
    {
        base.Update();
    }

    public override void OnInteract()
    {
        if (IsPlayerCanInteract(PlayerChangeController.Instance.GetCurrentPlayerSO()))
        { 
            if (isChangeCameraFollowingObjectOnInteract)
            {
                if (movableHeadToChangeState.Length > 1)
                    CameraFollowing.Instance.OnCameraChangeFollower += CameraFollower_OnCameraChangeFollower;

                movableHeadToChangeStateCurrentIndex = 0;

                movableHeadToChangeState[0].ChangeHeadState(movableHeadToChangeState[0].GetIsCanDamage());
            }
            else
            {
                foreach (MovableHead movableHead in movableHeadToChangeState)
                {
                    movableHead.ChangeHeadState(movableHead.GetIsCanDamage());
                }
                isInteractable = true;
            }

            base.OnInteract();
        }
    }

    private void CameraFollower_OnCameraChangeFollower(object sender, System.EventArgs e)
    {
        movableHeadToChangeStateCurrentIndex++;

        ChangeFollowingObject();

        movableHeadToChangeState[movableHeadToChangeStateCurrentIndex].
            ChangeHeadState(movableHeadToChangeState[movableHeadToChangeStateCurrentIndex].GetIsCanDamage());

        if (movableHeadToChangeStateCurrentIndex + 1 < movableHeadToChangeState.Length)
        {
            CameraFollowing.Instance.OnCameraChangeFollower -= CameraFollower_OnCameraChangeFollower;
            isInteractable = true;
        }
    }

    protected override void ChangeFollowingObject()
    {
        base.ChangeFollowingObject();

        CameraFollowing.Instance.TemporaryChangeFollowingObjectTo(movableHeadToChangeState
            [movableHeadToChangeStateCurrentIndex].gameObject.transform, cameraChangeDuration, interactedPlayer);
    }
}
