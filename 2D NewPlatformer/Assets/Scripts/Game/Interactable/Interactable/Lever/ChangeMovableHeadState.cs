using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChangeMovableHeadState))]
[RequireComponent(typeof(AddInteractButtonUI))]
public class ChangeMovableHeadState : InteractableItem
{
    public override event EventHandler OnAllInteractionsFinished;
    [Header("Callback Platfrom Settings")]
    [SerializeField] private MovableHead[] movableHeadToChangeState;
    private int movableHeadToChangeStateCurrentIndex;

    public override void OnInteract(PlayerController player)
    {
        base.OnInteract(player);

        if (isChangeCameraFollowingObjectOnInteract)
        {
            CameraFollowing.Instance.OnCameraChangeFollower += CameraFollower_OnCameraChangeFollower;

            movableHeadToChangeStateCurrentIndex = 0;

            ChangeFollowingObject(movableHeadToChangeState
                [movableHeadToChangeStateCurrentIndex].gameObject.transform);
            StartCoroutine(WaitForChangeMovableHeadState());
        }
        else
        {
            foreach (MovableHead movableHead in movableHeadToChangeState)
            {
                movableHead.ChangeHeadState(!movableHead.GetIsCanDamage());
            }
            isInteractable = true;
        }
    }

    private void CameraFollower_OnCameraChangeFollower(object sender, System.EventArgs e)
    {
        movableHeadToChangeStateCurrentIndex++;
        if (movableHeadToChangeStateCurrentIndex >= movableHeadToChangeState.Length)
        {
            CameraFollowing.Instance.OnCameraChangeFollower -= CameraFollower_OnCameraChangeFollower;
            isInteractable = true;
            OnAllInteractionsFinished?.Invoke(this, EventArgs.Empty);
            return;
        }

        ChangeFollowingObject(movableHeadToChangeState
            [movableHeadToChangeStateCurrentIndex].gameObject.transform);

        StartCoroutine(WaitForChangeMovableHeadState());
    }

    private IEnumerator WaitForChangeMovableHeadState()
    {
        yield return new WaitForSeconds(cameraChangeDuration / 2);

        movableHeadToChangeState[movableHeadToChangeStateCurrentIndex].
            ChangeHeadState(!movableHeadToChangeState[movableHeadToChangeStateCurrentIndex].GetIsCanDamage());
    }

    protected override void ChangeFollowingObject(Transform toChange)
    {
        base.ChangeFollowingObject(toChange);

        CameraFollowing.Instance.TemporaryChangeFollowingObjectTo(toChange, cameraChangeDuration, interactedPlayer);
    }
}
