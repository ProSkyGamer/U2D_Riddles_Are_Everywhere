using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CheckpointVisual))]
public class Checkpoint : InteractableItem
{
    [SerializeField] private int waypointPriority = 0;
    private bool isWasPlacedAutomaticaly = false;
    private CheckpointVisual checkpointVisual;

    protected override void Awake()
    {
        base.Awake();

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

    protected override void Update()
    {
        if (CheckpointsController.Instance.GetCurrentCheckpoint() != this)
        {
            if (isInteractable && CheckpointsController.Instance.GetCurrentCheckpoint() != this)
            {
                Vector3 castPosition = collision.transform.position + (Vector3)collision.offset;
                Vector2 castCubeLenght = collision.size;
                float cubeRotation = 0f;
                Vector2 cubeDirection = Vector2.up;

                RaycastHit2D raycastHit = Physics2D.BoxCast(castPosition, castCubeLenght,
                    cubeRotation, cubeDirection, interactableHeight, playerLayer);
                if (raycastHit)
                    if (raycastHit.rigidbody.gameObject.TryGetComponent<PlayerController>(out interactedPlayer))
                    {
                        if (!isWasPlacedAutomaticaly)
                        {
                            if (!CheckpointsController.Instance.TryChangeCheckpoint(this))
                            {
                                if (!isHasButtonOnInterface)
                                    AddInteractButtonToInterafce();
                            }
                        }
                        return;
                    }

                if (isHasButtonOnInterface)
                    RemoveInteractButtonFromInterafce();
            }
        }
    }

    public override void OnInteract()
    {
        base.OnInteract();

        CheckpointsController.Instance.ChangeCheckpoint(this);

        isInteractable = true;
    }

    public int GetWaypointPriority()
    {
        return waypointPriority;
    }
}