using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FanTrapVisual : MonoBehaviour
{
    private const string ANIMATION_STATE = "State";

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeAnimationState(bool animationState)
    {
        animator.SetBool(ANIMATION_STATE, animationState);
    }
}
