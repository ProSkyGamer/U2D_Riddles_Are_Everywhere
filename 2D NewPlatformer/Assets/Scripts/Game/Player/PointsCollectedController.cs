using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsCollectedController : MonoBehaviour
{
    [SerializeField] private int neededPointToCompleteLevel;
    public static PointsCollectedController Instance { get; private set; }

    public event EventHandler<OnPointsCollectedChangeEventArgs> OnPointsCollectedChange;
    public class OnPointsCollectedChangeEventArgs : EventArgs
    {
        public int currentPoints;
        public int needePoints;
    }
    private int currentCollectedPoints;
    private bool isFirstUpdate = true;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Update()
    {
        if(isFirstUpdate)
        {
            isFirstUpdate = false;
            OnPointsCollectedChange?.Invoke(this, new OnPointsCollectedChangeEventArgs()
            {
                currentPoints = currentCollectedPoints,
                needePoints = neededPointToCompleteLevel
            });
        }
    }

    public void AddPoints(int toAdd)
    {
        currentCollectedPoints += toAdd;
        OnPointsCollectedChange?.Invoke(this, new OnPointsCollectedChangeEventArgs()
        { 
            currentPoints = currentCollectedPoints,
            needePoints = neededPointToCompleteLevel 
        });
    }

    public bool IsEnoughPointsToFinish()
    {
        return currentCollectedPoints >= neededPointToCompleteLevel;
    }

    public int GetCollectedPoints()
    {
        return currentCollectedPoints;
    }
}
