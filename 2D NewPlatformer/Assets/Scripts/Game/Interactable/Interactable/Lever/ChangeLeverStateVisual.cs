using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLeverStateVisual : ObjectVisual
{
    private const string ANIMTOR_STATE = "State";

    private bool isTrunedOff = false;

    public void ChangeLeverState()
    {
        isTrunedOff = !isTrunedOff;
        animator.SetBool(ANIMTOR_STATE, isTrunedOff);
    }

    public override void OnInteractChangeAnimationState()
    {
        ChangeLeverState();
    }
}
