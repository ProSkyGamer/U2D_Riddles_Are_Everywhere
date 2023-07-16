using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BreakableBlockVisual : MonoBehaviour
{
    private const string ANIMATOR_STATE = "State";

    public enum AnimationStates
    {
        Iddle,
        MoveRight,
        MoveLeft,
        DestroyBreakableBlock,
    }

    private Animator animator;

    private AnimationStates currentAnimationState = AnimationStates.Iddle;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeAnimationState(AnimationStates newState)
    {
        currentAnimationState = newState;
        animator.SetInteger(ANIMATOR_STATE, (int)currentAnimationState);
    }

    public AnimationStates GetCurrentAnimationState()
    {
        return currentAnimationState;
    }
}
