using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ChangeTrapLocation : InteractableItem
{
    public override event EventHandler OnAllInteractionsFinished;
    [Serializable]
    public class TrapLocations
    {
        public Transform trapToChangeLocations;
        public Transform[] locationPoints;
    }
    [SerializeField] private TrapLocations[] allTrapLocations;
    private int trapToChangeLocationIndex;

    public override void OnInteract(PlayerController player)
    {
        base.OnInteract(player);
        if(isChangeCameraFollowingObjectOnInteract)
        {
            trapToChangeLocationIndex = 0;

            CameraFollowing.Instance.OnCameraChangeFollower += CameraFollowing_OnCameraChangeFollower;

            ChangeFollowingObject(allTrapLocations[trapToChangeLocationIndex].trapToChangeLocations);

            StartCoroutine(WaitForChangeLocation());
        }
        else
        {
            foreach(TrapLocations trap in allTrapLocations)
            {
                for(int i = 0; i <  trap.locationPoints.Length; i++)
                {
                    int nextPoint;
                    if (trap.locationPoints[i].position == trap.trapToChangeLocations.position)
                    {
                        if (i == trap.locationPoints.Length - 1)
                            nextPoint = 0;
                        else
                            nextPoint = i + 1;

                        trap.trapToChangeLocations.position = trap.locationPoints[nextPoint].position;
                        isInteractable = true;
                        break;
                    }
                }
            }
        }
    }

    private void CameraFollowing_OnCameraChangeFollower(object sender, EventArgs e)
    {
        trapToChangeLocationIndex++;
        if (trapToChangeLocationIndex >= allTrapLocations.Length)
        {
            CameraFollowing.Instance.OnCameraChangeFollower -= CameraFollowing_OnCameraChangeFollower;
            isInteractable = true;
            OnAllInteractionsFinished?.Invoke(this, EventArgs.Empty);
            return;
        }

        ChangeFollowingObject(allTrapLocations[trapToChangeLocationIndex].trapToChangeLocations);

        StartCoroutine(WaitForChangeLocation());
    }

    private IEnumerator WaitForChangeLocation()
    {
        yield return new WaitForSeconds(cameraChangeDuration / 2);
        for (int i = 0; i < allTrapLocations[trapToChangeLocationIndex].locationPoints.Length; i++)
        {
            int nextPoint;
            if (allTrapLocations[trapToChangeLocationIndex].locationPoints[i].position ==
                allTrapLocations[trapToChangeLocationIndex].trapToChangeLocations.position)
            {
                if (i == allTrapLocations[trapToChangeLocationIndex].locationPoints.Length - 1)
                    nextPoint = 0;
                else
                    nextPoint = i + 1;

                allTrapLocations[trapToChangeLocationIndex].trapToChangeLocations.position =
                    allTrapLocations[trapToChangeLocationIndex].locationPoints[nextPoint].position;
                break;
            }
        }
    }

    protected override void ChangeFollowingObject(Transform toChange)
    {
        base.ChangeFollowingObject(toChange);
        CameraFollowing.Instance.TemporaryChangeFollowingObjectTo(toChange, cameraChangeDuration, interactedPlayer);
    }
}
