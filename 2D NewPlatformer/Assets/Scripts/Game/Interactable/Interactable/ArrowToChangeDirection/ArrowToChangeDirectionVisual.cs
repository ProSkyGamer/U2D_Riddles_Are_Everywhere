using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowToChangeDirectionVisual : ObjectVisual
{
    private const string ANIMATOR_STATE = "State";

    public void ChangeAnimationState(bool state)
    {
        animator.SetBool(ANIMATOR_STATE, state);
    }

    public void OnAnimationHitEnds()
    {
        ChangeAnimationState(false);
    }

    public override void OnInteractChangeAnimationState()
    {
        ChangeAnimationState(true);
    }
}
