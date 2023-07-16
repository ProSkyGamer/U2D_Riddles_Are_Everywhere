using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CheckpointVisual))]
[RequireComponent(typeof(CheckpointAddInteractButton))]
public class Checkpoint : InteractableItem
{
    [SerializeField] private int checkpointPriority = 0;
    private bool isWasPlacedAutomaticaly = false;
    private CheckpointVisual checkpointVisual;

    private void Awake()
    {
        checkpointVisual = GetComponent<CheckpointVisual>();
    }

    protected virtual void Start()
    {
        CheckpointsController.Instance.OnCurrentCheckpointChange += CheckpointController_OnCurrentCheckpointChange;
    }

    private void CheckpointController_OnCurrentCheckpointChange(object sender, System.EventArgs e)
    {
        if (CheckpointsController.Instance.GetCurrentCheckpoint() == this)
            checkpointVisual.ChangeCheckpointVisualState(CheckpointVisual.CheckpointVisualStates.On);
        else
            checkpointVisual.ChangeCheckpointVisualState(CheckpointVisual.CheckpointVisualStates.Off);
    }

    public override void OnInteract(PlayerController player)
    {
        base.OnInteract(player);

        CheckpointsController.Instance.ChangeCheckpoint(this);

        isInteractable = true;
    }

    public bool GetIsWasPlacedAutomaticaly()
    {
        return isWasPlacedAutomaticaly;
    }

    public void PlaceCheckpointAutomaticaly(PlayerController interactedPlayer)
    {
        isWasPlacedAutomaticaly = true;
        OnInteract(interactedPlayer);
    }

    public int GetCheckpointPriority()
    {
        return checkpointPriority;
    }

    public override bool IsCanInteract()
    {
        return isInteractable && CheckpointsController.Instance.GetCurrentCheckpoint() != this;
    }
}