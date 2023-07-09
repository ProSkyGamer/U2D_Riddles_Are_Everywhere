using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class TrampolineVisual : MonoBehaviour
{
    private const string ANIMATOR_STATE = "State";

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeAnimationState(bool state)
    {
        animator.SetBool(ANIMATOR_STATE, state);
    }

    public void SetIddleAnimation()
    {
        animator.SetBool(ANIMATOR_STATE, false);
    }
}
