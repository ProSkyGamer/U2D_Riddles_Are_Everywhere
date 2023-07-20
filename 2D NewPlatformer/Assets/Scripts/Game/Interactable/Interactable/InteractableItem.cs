using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class InteractableItem : MonoBehaviour
{
    public virtual event EventHandler OnAllInteractionsFinished;
    [SerializeField] protected bool isInteractable = true;

    [Header("Change View Settings")]
    [SerializeField] protected bool isChangeCameraFollowingObjectOnInteract = false;
    [SerializeField] protected float cameraChangeDuration = 1.5f;

    protected PlayerController interactedPlayer;

    public virtual void OnInteract(PlayerController player)
    {
        interactedPlayer = player;
        isInteractable = false;
    }

    protected virtual void ChangeFollowingObject(Transform toChange)
    {
        interactedPlayer.DisableMovement();
    }

    public virtual bool IsCanInteract()
    {
        return isInteractable;
    }

    public bool GetIsChangeCameraOnInteract()
    {
        return isChangeCameraFollowingObjectOnInteract;
    }
}
