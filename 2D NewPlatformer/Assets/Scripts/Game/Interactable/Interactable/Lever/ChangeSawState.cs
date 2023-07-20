using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChangeLeverStateVisual))]
public class ChangeSawState : InteractableItem
{
    public override event EventHandler OnAllInteractionsFinished;

    [SerializeField] private SawTrap[] sawTrapToChangeStateArray;
    private int sawTrapToChangeIndex;

    protected override void ChangeFollowingObject(Transform toChange)
    {
        base.ChangeFollowingObject(toChange);

        CameraFollowing.Instance.TemporaryChangeFollowingObjectTo(toChange, cameraChangeDuration, interactedPlayer);
    }

    public override void OnInteract(PlayerController player)
    {
        base.OnInteract(player);
        
        if (isChangeCameraFollowingObjectOnInteract)
        {
            CameraFollowing.Instance.OnCameraChangeFollower += CameraFollowing_OnCameraChangeFollower; ;

            sawTrapToChangeIndex = 0;

            ChangeFollowingObject(sawTrapToChangeStateArray
                [sawTrapToChangeIndex].gameObject.transform);
            StartCoroutine(WaitForChangeSawState());
        }
        else
        {
            foreach (var trap in sawTrapToChangeStateArray)
            {
                trap.ChangeSawTrapState(!trap.GetSawTrapCurrentState());
            }
            isInteractable = true;
        }
    }

    private void CameraFollowing_OnCameraChangeFollower(object sender, System.EventArgs e)
    {
        sawTrapToChangeIndex++;
        if (sawTrapToChangeIndex >= sawTrapToChangeStateArray.Length)
        {
            CameraFollowing.Instance.OnCameraChangeFollower -= CameraFollowing_OnCameraChangeFollower;
            isInteractable = true;
            OnAllInteractionsFinished?.Invoke(this, EventArgs.Empty);
            return;
        }

        ChangeFollowingObject(sawTrapToChangeStateArray
            [sawTrapToChangeIndex].gameObject.transform);

        StartCoroutine(WaitForChangeSawState());
    }

    private IEnumerator WaitForChangeSawState()
    {
        yield return new WaitForSeconds(cameraChangeDuration / 3);
        sawTrapToChangeStateArray[sawTrapToChangeIndex].ChangeSawTrapState(
            !sawTrapToChangeStateArray[sawTrapToChangeIndex].GetSawTrapCurrentState());
    }
}
