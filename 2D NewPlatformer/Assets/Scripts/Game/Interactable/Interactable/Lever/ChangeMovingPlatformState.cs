using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMovingPlatformState : InteractableItem
{
    public override event EventHandler OnAllInteractionsFinished;

    [SerializeField] private MovingPlatform[] movingPlatformToChangeStateArray;
    private int movingPlatformToChangeIndex;

    public override void OnInteract(PlayerController player)
    {
        base.OnInteract(player);

        if (isChangeCameraFollowingObjectOnInteract)
        {
            CameraFollowing.Instance.OnCameraChangeFollower += CameraFollowing_OnCameraChangeFollower;

            movingPlatformToChangeIndex = 0;

            ChangeFollowingObject(movingPlatformToChangeStateArray
                [movingPlatformToChangeIndex].gameObject.transform);
            StartCoroutine(WaitForChangeFanState());
        }
        else
        {
            foreach (var trap in movingPlatformToChangeStateArray)
            {
                trap.ChangeInteractionLockState(!trap.GetIsInteractionLockedState());
            }
            isInteractable = true;
        }
    }

    protected override void ChangeFollowingObject(Transform toChange)
    {
        base.ChangeFollowingObject(toChange);

        CameraFollowing.Instance.TemporaryChangeFollowingObjectTo(toChange, cameraChangeDuration, interactedPlayer);
    }

    private IEnumerator WaitForChangeFanState()
    {
        yield return new WaitForSeconds(cameraChangeDuration / 2);
        movingPlatformToChangeStateArray[movingPlatformToChangeIndex].ChangeInteractionLockState(
            !movingPlatformToChangeStateArray[movingPlatformToChangeIndex].GetIsInteractionLockedState());
    }

    private void CameraFollowing_OnCameraChangeFollower(object sender, System.EventArgs e)
    {
        movingPlatformToChangeIndex++;
        if (movingPlatformToChangeIndex >= movingPlatformToChangeStateArray.Length)
        {
            CameraFollowing.Instance.OnCameraChangeFollower -= CameraFollowing_OnCameraChangeFollower;
            isInteractable = true;
            OnAllInteractionsFinished?.Invoke(this, EventArgs.Empty);
            return;
        }

        ChangeFollowingObject(movingPlatformToChangeStateArray
            [movingPlatformToChangeIndex].gameObject.transform);

        StartCoroutine(WaitForChangeFanState());
    }
}
