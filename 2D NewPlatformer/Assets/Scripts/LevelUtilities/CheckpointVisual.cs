using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CheckpointVisual : MonoBehaviour
{
    public enum CheckpointVisualStates
    {
        Off,
        On,
        Iddle,
    }

    private const string CHECKPOINT_STATE_VISUAL = "State";

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeCheckpointVisualState(CheckpointVisualStates checkpointVisualStates)
    {
        animator.SetInteger(CHECKPOINT_STATE_VISUAL, (int)checkpointVisualStates);
    }
}
