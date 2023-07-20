using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinishVisual : ObjectVisual
{
    private const string ANIMATOR_STATE = "State";

    public override void OnInteractChangeAnimationState()
    {
        animator.SetTrigger(ANIMATOR_STATE);
    }
}
