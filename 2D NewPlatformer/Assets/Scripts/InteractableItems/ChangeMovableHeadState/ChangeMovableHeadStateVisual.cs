using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMovableHeadStateVisual : MonoBehaviour
{
    private const string CALLBACK_MOVING_PLATFORM_ANIMATOR = "State";

    private bool isTrunedOff = false;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeCallbackMovingPlatformAnimationState()
    {
        isTrunedOff = !isTrunedOff;
        animator.SetBool(CALLBACK_MOVING_PLATFORM_ANIMATOR, isTrunedOff);
    }
}
