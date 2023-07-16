using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ChangeLeverStateVisual))]
public class CallbackMovingPlatform : InteractableItem
{
    public override event EventHandler OnAllInteractionsFinished;

    [Header("Callback Platfrom Settings")]
    [SerializeField] private MovingPlatform[] movingPlatformsToCallbackArray;
    [SerializeField] private Transform[] callbackPointsArray;
    private List<MovingPlatform> movingPlatformsWaitingList = new List<MovingPlatform>();
    private List<Transform> movingPlatformWaitingCallbackPointList = new List<Transform>();
    private int movingPlatformsWaitingForArrival = 0;

    public override void OnInteract(PlayerController player)
    {
        base.OnInteract(player);

        for (int i = 0; i < movingPlatformsToCallbackArray.Length; i++)
        {
            if (!movingPlatformsToCallbackArray[i].GetIsMoving() &&
                movingPlatformsToCallbackArray[i].transform.position != callbackPointsArray[i].position
                && !movingPlatformsToCallbackArray[i].GetIsInteractionLockedState())
            {
                AddMovingPlatformWaitingForDeparture(movingPlatformsToCallbackArray[i], callbackPointsArray[i]);
            }
        }

        movingPlatformsWaitingForArrival = movingPlatformsWaitingList.Count;

        if (isChangeCameraFollowingObjectOnInteract)
        {
            CameraFollowing.Instance.OnCameraChangeFollower += CameraFollower_OnCameraChangeFollower;

            StartPlatfromMoveTo(movingPlatformsWaitingList[0], movingPlatformWaitingCallbackPointList[0].position);
            ChangeFollowingObject(movingPlatformsWaitingList[0].gameObject.transform);

            RemoveMovingPlatformWaitingForDeparture(0);
        }
        else
        {
            for (int i = 0; i < movingPlatformsWaitingList.Count; i++)
            {
                StartPlatfromMoveTo(movingPlatformsWaitingList[i], movingPlatformWaitingCallbackPointList[i].position);
            }
            movingPlatformsWaitingList.Clear();
            movingPlatformWaitingCallbackPointList.Clear();
        }
    }

    private void CameraFollower_OnCameraChangeFollower(object sender, System.EventArgs e)
    {
        if (movingPlatformWaitingCallbackPointList.Count == 0)
        {
            CameraFollowing.Instance.OnCameraChangeFollower -= CameraFollower_OnCameraChangeFollower;
            OnAllInteractionsFinished?.Invoke(this, EventArgs.Empty);
            return;
        }
        StartPlatfromMoveTo(movingPlatformsWaitingList[0], movingPlatformWaitingCallbackPointList[0].position);
        ChangeFollowingObject(movingPlatformsWaitingList[0].gameObject.transform);
    }

    protected override void ChangeFollowingObject(Transform toChange)
    {
        base.ChangeFollowingObject(toChange);

        CameraFollowing.Instance.TemporaryChangeFollowingObjectTo(toChange,
                cameraChangeDuration, interactedPlayer);
    }

    private bool IsCanCallbackAnyPlafrom()
    {
        for (int i = 0; i < movingPlatformsToCallbackArray.Length; i++)
        {
            if (movingPlatformsToCallbackArray[i].IsCanInteract() &&
                movingPlatformsToCallbackArray[i].transform.position != callbackPointsArray[i].position)
                return true;
        }

        return false;
    }

    private void StartPlatfromMoveTo(MovingPlatform movingPlatform, Vector3 moveTo)
    {
        movingPlatform.StartMoveTo(moveTo);
        movingPlatform.OnPlatformArrivalToDestination += MovingPlatform_OnPlatformArrivalToDestination;
    }

    private void MovingPlatform_OnPlatformArrivalToDestination(object sender, System.EventArgs e)
    {
        movingPlatformsWaitingForArrival--;
        MovingPlatform movingPlatform = (MovingPlatform)sender;
        movingPlatform.OnPlatformArrivalToDestination -= MovingPlatform_OnPlatformArrivalToDestination;

        if (movingPlatformsWaitingForArrival == 0)
            isInteractable = true;
    }

    private void AddMovingPlatformWaitingForDeparture(MovingPlatform movingPlatform, Transform platformDestination)
    {
        movingPlatformsWaitingList.Add(movingPlatform);
        movingPlatformWaitingCallbackPointList.Add(platformDestination);
    }

    private void RemoveMovingPlatformWaitingForDeparture(MovingPlatform movingPlatform, Transform platformDestination)
    {
        movingPlatformsWaitingList.Remove(movingPlatform);
        movingPlatformWaitingCallbackPointList.Remove(platformDestination);
    }

    private void RemoveMovingPlatformWaitingForDeparture(int index)
    {
        movingPlatformsWaitingList.RemoveAt(index);
        movingPlatformWaitingCallbackPointList.RemoveAt(index);
    }

    public override bool IsCanInteract()
    {
        return isInteractable && IsCanCallbackAnyPlafrom();
    }
}
