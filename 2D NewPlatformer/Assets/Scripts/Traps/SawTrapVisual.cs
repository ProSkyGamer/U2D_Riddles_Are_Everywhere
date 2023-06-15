using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SawTrapVisual : MonoBehaviour
{
    private const string SAW_ANIMATION_WORK_STATE = "WorkState";

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeSawStateVisual(bool isTurnedOn)
    {
        animator.SetBool(SAW_ANIMATION_WORK_STATE, isTurnedOn);
    }
}
