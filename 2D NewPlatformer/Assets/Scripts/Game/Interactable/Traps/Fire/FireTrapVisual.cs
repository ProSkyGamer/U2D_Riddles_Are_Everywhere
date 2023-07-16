using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FireTrapVisual : MonoBehaviour
{
    private const string ANIMATOR_STATE = "State";

    public enum AnimationStates
    {
        Off,
        TurningOn,
        TurningOff,
        On,
    }
    private AnimationStates currentAnimationState;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeAnimationState(AnimationStates state)
    {
        currentAnimationState = state;
        animator.SetInteger(ANIMATOR_STATE, (int)state);
        animator.speed = 1;
    }

    public void ChangeAnimationState(AnimationStates state, float timeForAnimation)
    {
        currentAnimationState = state;
        animator.SetInteger(ANIMATOR_STATE, (int)state);
        //Добавить изменение кадра в зависимости от передаваемового времени
    }

    public AnimationStates GetCurrentAnimationState()
    {
        return currentAnimationState;
    }
}
