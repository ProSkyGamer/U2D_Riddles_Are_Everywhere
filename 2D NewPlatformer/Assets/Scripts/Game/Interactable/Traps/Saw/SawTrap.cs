using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SawTrapVisual))]
public class SawTrap : BaseTrap
{
    [SerializeField] private bool isTurnedOn = true;

    private SawTrapVisual sawTrapVisual;

    private new void Awake()
    {
        base.Awake();
        sawTrapVisual = GetComponent<SawTrapVisual>();

        ChangeSawTrapState(isTurnedOn);
    }

    public void ChangeSawTrapState(bool state)
    {
        isTurnedOn = state;

        sawTrapVisual.ChangeSawStateVisual(isTurnedOn);
    }

    public bool GetSawTrapCurrentState()
    {
        return isTurnedOn;
    }
}
