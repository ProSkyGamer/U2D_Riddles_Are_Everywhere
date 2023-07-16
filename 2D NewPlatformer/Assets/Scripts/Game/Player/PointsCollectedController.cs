using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsCollectedController : MonoBehaviour
{
    public static PointsCollectedController Instance { get; private set; }

    public event EventHandler<OnPointsCollectedChangeEventArgs> OnPointsCollectedChange;
    public class OnPointsCollectedChangeEventArgs : EventArgs
    {
        public int currentPoints;
    }
    private int currentCollectedPoints;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void AddPoints(int toAdd)
    {
        currentCollectedPoints += toAdd;
        OnPointsCollectedChange?.Invoke(this, new OnPointsCollectedChangeEventArgs()
        { 
            currentPoints = currentCollectedPoints 
        });
    }
}
