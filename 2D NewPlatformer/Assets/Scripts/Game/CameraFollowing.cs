using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    public static CameraFollowing Instance { get; private set; }
    public event EventHandler OnCameraChangeFollower;

    [Header("Settings")]
    [SerializeField] private Transform followingPlayer;
    private Transform currentFollowingObject;
    [SerializeField] private bool isFollowing = true;
    private PlayerController playerToEnableMovement;
    [SerializeField] private float minYLocation = -14f;

    private float followingTimer;
    private bool isFirstUpdate = true;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        currentFollowingObject = followingPlayer;
    }

    private void Update()
    {
        if (isFirstUpdate)
        {
            transform.position = currentFollowingObject.position + new Vector3(0, 0, transform.position.z);
            isFirstUpdate = false;
        }

        if (isFollowing && currentFollowingObject != null)
        {
            Vector3 startPosition = transform.position;
            Vector3 endPosition = new Vector3(currentFollowingObject.position.x,
                currentFollowingObject.position.y, transform.position.z);
            float percentValue;
            if (currentFollowingObject == followingPlayer)
                percentValue = 0.15f;
            else
                percentValue = 0.05f;

            if (endPosition.y < minYLocation)
                endPosition.y = minYLocation;

            transform.position = Vector3.Lerp(startPosition, endPosition, percentValue);

            if (followingTimer > 0)
                followingTimer -= Time.deltaTime;
            else
            {
                currentFollowingObject = followingPlayer;
                OnCameraChangeFollower?.Invoke(this, EventArgs.Empty);
                if (playerToEnableMovement != null)
                {
                    playerToEnableMovement.EnableMovement();
                    playerToEnableMovement = null;
                }
            }
        }
    }

    public void TemporaryChangeFollowingObjectTo(Transform changeTo, float timeToFollow)
    {
        currentFollowingObject = changeTo;
        followingTimer = timeToFollow;
    }

    public void TemporaryChangeFollowingObjectTo(Transform changeTo, float timeToFollow, PlayerController enableMovementTo)
    {
        currentFollowingObject = changeTo;
        followingTimer = timeToFollow;
        playerToEnableMovement = enableMovementTo;
    }

    public void ChangeFollowingPlayer(Transform newFollowingPlayerTransform)
    {
        followingPlayer = newFollowingPlayerTransform;
    }
}
