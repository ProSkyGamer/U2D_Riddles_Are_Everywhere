using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChangeLeverStateVisual))]
public class ChangeFanState : InteractableItem
{
    public override event EventHandler OnAllInteractionsFinished;

    [SerializeField] private FanTrap[] fansTrapToChangeStateArray;
    private int fanTrapToChangeIndex;

    public override void OnInteract(PlayerController player)
    {
        base.OnInteract(player);

        if (isChangeCameraFollowingObjectOnInteract)
        {
            CameraFollowing.Instance.OnCameraChangeFollower += CameraFollowing_OnCameraChangeFollower;

            fanTrapToChangeIndex = 0;

            ChangeFollowingObject(fansTrapToChangeStateArray
                [fanTrapToChangeIndex].gameObject.transform);
            StartCoroutine(WaitForChangeFanState());
        }
        else
        {
            foreach(var trap in fansTrapToChangeStateArray)
            {
                trap.ChangeFanEnabledToggle(!trap.GetFanTrapCurrentState());
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
        fansTrapToChangeStateArray[fanTrapToChangeIndex].ChangeFanEnabledToggle(
            !fansTrapToChangeStateArray[fanTrapToChangeIndex].GetFanTrapCurrentState());
    }

    private void CameraFollowing_OnCameraChangeFollower(object sender, System.EventArgs e)
    {
        fanTrapToChangeIndex++;
        if (fanTrapToChangeIndex >= fansTrapToChangeStateArray.Length)
        {
            CameraFollowing.Instance.OnCameraChangeFollower -= CameraFollowing_OnCameraChangeFollower;
            isInteractable = true;
            OnAllInteractionsFinished?.Invoke(this, EventArgs.Empty);
            return;
        }

        ChangeFollowingObject(fansTrapToChangeStateArray
            [fanTrapToChangeIndex].gameObject.transform);

        StartCoroutine(WaitForChangeFanState());
    }
}
