using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class MovableHeadVisual : MonoBehaviour
{
    private const string ANIMATOR_STATE = "State";

    public enum AnimationStates
    {
        RockHeadIdle,
        RockHeadBlink,
        RockHeadBottomHit,
        RockHeadLeftHit,
        RockHeadRightHit,
        RockHeadTopHit,
        SpikeHeadIdle,
        SpikeHeadBlink,
        SpikeHeadBottomHit,
        SpikeHeadLeftHit,
        SpikeHeadRightHit,
        SpikeHeadTopHit,
        RockHeadMove,
        SpikeHeadMove,
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
