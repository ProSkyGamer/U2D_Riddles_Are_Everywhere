using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformVisual : MonoBehaviour
{
    public enum AnimationStates
    {
        InteractableStanding,
        InteractableMoving,
        NotInteractable,
    }

    private AnimationStates currentState = AnimationStates.InteractableStanding;

    private const string MOVING_PLATFORM_ANIMATOR = "State";

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeMovingPlatformAnimationState(AnimationStates animationStates)
    {
        currentState = animationStates;
        animator.SetInteger(MOVING_PLATFORM_ANIMATOR, (int)currentState);
    }

    public AnimationStates GetCurrentAnimationState()
    {
        return currentState;
    }
}
