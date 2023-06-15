using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformVisual : MonoBehaviour
{
    private const string MOVING_PLATFORM_ANIMATOR = "State";

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeMovingPlatformAnimationState(bool isMoving)
    {
        animator.SetBool(MOVING_PLATFORM_ANIMATOR, isMoving);
    }
}
