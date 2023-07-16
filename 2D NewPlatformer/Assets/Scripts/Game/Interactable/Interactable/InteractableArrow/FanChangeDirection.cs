using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(AddInteractButtonUI))]
public class FanChangeDirection : InteractableItem
{
    public override event EventHandler OnAllInteractionsFinished;

    [Serializable]
    public class FansToChangeDirections
    {
        public FanTrap fanTrap;
        public Vector2[] blowingDirections;
        public Quaternion[] fanRotation;
        public Vector3[] additionalMovement;
    }
    [SerializeField] private FansToChangeDirections[] allFansToChangeDirectionsArray;
    private int fanTrapToChangeIndex;

    public override void OnInteract(PlayerController player)
    {
        base.OnInteract(player);

        if (isChangeCameraFollowingObjectOnInteract)
        {
            CameraFollowing.Instance.OnCameraChangeFollower += CameraFollowing_OnCameraChangeFollower;

            fanTrapToChangeIndex = 0;

            ChangeFollowingObject(allFansToChangeDirectionsArray
                [fanTrapToChangeIndex].fanTrap.gameObject.transform);
            StartCoroutine(WaitForChangeFanBlowingDirection());
        }
        else
        {
            foreach (var trap in allFansToChangeDirectionsArray)
            {
                for(int i = 0; i < trap.blowingDirections.Length; i++)
                {
                    if(trap.fanTrap.GetCurrentBlowingDirection() == trap.blowingDirections[i])
                    {
                        int indexToChange = i + 1;
                        if (indexToChange == trap.blowingDirections.Length)
                            indexToChange = 0;

                        trap.fanTrap.ChangeCurrentBlowingDirection(trap.blowingDirections
                            [indexToChange], trap.fanRotation[indexToChange],
                            trap.additionalMovement[indexToChange]);

                        break;
                    }
                }
            }
            isInteractable = true;
        }
    }

    protected override void ChangeFollowingObject(Transform toChange)
    {
        base.ChangeFollowingObject(toChange);

        CameraFollowing.Instance.TemporaryChangeFollowingObjectTo(toChange, cameraChangeDuration, interactedPlayer);
    }

    private IEnumerator WaitForChangeFanBlowingDirection()
    {
        yield return new WaitForSeconds(cameraChangeDuration / 2);
        for (int i = 0; i < allFansToChangeDirectionsArray[fanTrapToChangeIndex].blowingDirections.Length; i++)
        {
            if (allFansToChangeDirectionsArray[fanTrapToChangeIndex].fanTrap.GetCurrentBlowingDirection() ==
                allFansToChangeDirectionsArray[fanTrapToChangeIndex].blowingDirections[i])
            {
                int indexToChange = i + 1;
                if (indexToChange == allFansToChangeDirectionsArray[fanTrapToChangeIndex].blowingDirections.Length)
                    indexToChange = 0;
                allFansToChangeDirectionsArray[fanTrapToChangeIndex].fanTrap.ChangeCurrentBlowingDirection(
                    allFansToChangeDirectionsArray[fanTrapToChangeIndex].blowingDirections[indexToChange],
                    allFansToChangeDirectionsArray[fanTrapToChangeIndex].fanRotation[indexToChange],
                    allFansToChangeDirectionsArray[fanTrapToChangeIndex].additionalMovement[indexToChange]);

                break;
            }
        }
    }

    private void CameraFollowing_OnCameraChangeFollower(object sender, System.EventArgs e)
    {
        fanTrapToChangeIndex++;
        if (fanTrapToChangeIndex >= allFansToChangeDirectionsArray.Length)
        {
            CameraFollowing.Instance.OnCameraChangeFollower -= CameraFollowing_OnCameraChangeFollower;
            isInteractable = true;
            OnAllInteractionsFinished?.Invoke(this, EventArgs.Empty);
            return;
        }

        ChangeFollowingObject(allFansToChangeDirectionsArray
                [fanTrapToChangeIndex].fanTrap.gameObject.transform);

        StartCoroutine(WaitForChangeFanBlowingDirection());
    }
}
