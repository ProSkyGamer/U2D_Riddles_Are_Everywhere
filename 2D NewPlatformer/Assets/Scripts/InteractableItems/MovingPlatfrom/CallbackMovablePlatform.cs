using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CallbackMovingPlatformVisual))]
public class CallbackMovablePlatform : InteractableItem
{
    [Header("Callback Platfrom Settings")]
    [SerializeField] private MovingPlatform[] movingPlatformsToCallbackArray;
    [SerializeField] private Transform[] callbackPointsArray;
    private List<MovingPlatform> movingPlatformsWaitingList = new List<MovingPlatform>();
    private List<Transform> movingPlatformWaitingCallbackPointList = new List<Transform>();
    private int movingPlatformsWaitingForArrival = 0;

    private CallbackMovingPlatformVisual callbackMovingPlatformVisual;

    protected override void Awake()
    {
        base.Awake();

        callbackMovingPlatformVisual = GetComponent<CallbackMovingPlatformVisual>();
    }


    //DELETE
    private void Start()
    {
        Input.Instance.OnTestingKeyAction += Instance_OnTestingKeyAction;
    }

    //DELETE
    private void Instance_OnTestingKeyAction(object sender, System.EventArgs e)
    {
        Debug.Log($"{isInteractable} {IsAnyPlatformNotMoving()}" +
            $" {IsAnyPlatformNotOnCallbackPointAndNotMoving()}");
    }

    protected override void Update()
    {
        if(IsAnyPlatformNotMoving() && IsAnyPlatformNotOnCallbackPointAndNotMoving())
            base.Update();
    }

    public override void OnInteract()
    {
        for (int i = 0; i < movingPlatformsToCallbackArray.Length; i++)
        {
            if (!movingPlatformsToCallbackArray[i].IsMoving() &&
                movingPlatformsToCallbackArray[i].transform.position != callbackPointsArray[i].position)
            {
                AddMovingPlatformWaitingForDeparture(movingPlatformsToCallbackArray[i], callbackPointsArray[i]);
            }
        }

        movingPlatformsWaitingForArrival = movingPlatformsWaitingList.Count;

        if (isChangeCameraFollowingObjectOnInteract)
        {
            if (movingPlatformsWaitingList.Count > 1)
                CameraFollowing.Instance.OnCameraChangeFollower += CameraFollower_OnCameraChangeFollower;

            StartPlatfromMoveTo(movingPlatformsWaitingList[0], movingPlatformWaitingCallbackPointList[0].position);
        }
        else
        {
            for(int i = 0; i < movingPlatformsWaitingList.Count; i++)
            {
                StartPlatfromMoveTo(movingPlatformsWaitingList[i], movingPlatformWaitingCallbackPointList[i].position);
            }
            movingPlatformsWaitingList.Clear();
            movingPlatformWaitingCallbackPointList.Clear();
        }

        callbackMovingPlatformVisual.ChangeCallbackMovingPlatformAnimationState();

        base.OnInteract();
    }

    private void CameraFollower_OnCameraChangeFollower(object sender, System.EventArgs e)
    {
        StartPlatfromMoveTo(movingPlatformsWaitingList[0], movingPlatformWaitingCallbackPointList[0].position);
        ChangeFollowingObject();

        if (movingPlatformWaitingCallbackPointList.Count == 0)
            CameraFollowing.Instance.OnCameraChangeFollower -= CameraFollower_OnCameraChangeFollower;
    }

    protected override void ChangeFollowingObject()
    {
        base.ChangeFollowingObject();

        CameraFollowing.Instance.TemporaryChangeFollowingObjectTo(movingPlatformsWaitingList[0].gameObject.transform,
                cameraChangeDuration, interactedPlayer);

        RemoveMovingPlatformWaitingForDeparture(0);
    }

    private bool IsAnyPlatformNotMoving()
    {
        for (int i = 0; i < movingPlatformsToCallbackArray.Length; i++)
        {
            if (!movingPlatformsToCallbackArray[i].IsMoving())
                return true;
        }
        return false;
    }

    private bool IsAnyPlatformNotOnCallbackPointAndNotMoving()
    {
        for (int i = 0; i < movingPlatformsToCallbackArray.Length; i++)
        {
            if (movingPlatformsToCallbackArray[i].transform.position != callbackPointsArray[i].position &&
                !movingPlatformsToCallbackArray[i].IsMoving())
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
}
