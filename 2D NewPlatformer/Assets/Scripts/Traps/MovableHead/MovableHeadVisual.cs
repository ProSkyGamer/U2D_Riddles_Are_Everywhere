using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MovableHeadVisual : MonoBehaviour
{
    private const string ANIMATOR_STATE = "State";

    public enum AnimationStates
    {
        RockHeadIddle,
        RockHeadBlink,
        RockHeadBottomHit,
        RockHeadLeftHit,
        RockHeadRightHit,
        RockHeadTopHit,
        SpikeHeadIddle,
        SpikeHeadBlink,
        SpikeHeadBottomHit,
        SpikeHeadLeftHit,
        SpikeHeadRightHit,
        SpikeHeadTopHit,
    }

    private Animator animator;
    private AnimationStates currentAnimationState;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeAnimationState(AnimationStates animationStates)
    {
        currentAnimationState = animationStates;

        animator.SetInteger(ANIMATOR_STATE, (int)animationStates);
    }

    public AnimationStates GetCurrentAnimationState()
    {
        return currentAnimationState;
    }
}
