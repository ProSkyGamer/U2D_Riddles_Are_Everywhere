using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlateVisual : ObjectVisual
{
    private const string ANIMTOR_STATE = "State";

    private bool isPressed = false;

    public void ChangePressurePlateState()
    {
        isPressed = !isPressed;
        animator.SetBool(ANIMTOR_STATE, isPressed);
    }

    public override void OnInteractChangeAnimationState()
    {
        ChangePressurePlateState();
    }
}
