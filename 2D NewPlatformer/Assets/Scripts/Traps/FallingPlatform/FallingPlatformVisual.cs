using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FallingPlatformVisual : MonoBehaviour
{
    private const string ANIMATOR_TRIGGER = "State";

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void TriggerAnimation()
    {
        animator.SetTrigger(ANIMATOR_TRIGGER);
    }
}
