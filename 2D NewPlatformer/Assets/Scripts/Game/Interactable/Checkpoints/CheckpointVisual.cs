using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CheckpointVisual : ObjectVisual
{
    public enum CheckpointVisualStates
    {
        Off,
        On,
        Iddle,
    }

    private const string CHECKPOINT_STATE_VISUAL = "State";

    public void ChangeCheckpointVisualState(CheckpointVisualStates checkpointVisualStates)
    {
        animator.SetInteger(CHECKPOINT_STATE_VISUAL, (int)checkpointVisualStates);
    }

    public override void OnInteractChangeAnimationState()
    {
        ChangeCheckpointVisualState(CheckpointVisualStates.On);
    }
}
