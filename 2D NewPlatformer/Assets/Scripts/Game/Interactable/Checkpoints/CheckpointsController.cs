using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointsController : MonoBehaviour
{
    public static CheckpointsController Instance { get; private set; }

    public event EventHandler OnCurrentCheckpointChange;

    private Checkpoint currentCheckpoint;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        currentCheckpoint = LevelStart.Instance;
    }

    public Checkpoint GetCurrentCheckpoint()
    {
        return currentCheckpoint;
    }

    public bool TryChangeCheckpoint(Checkpoint checkpoint)
    {
        if (currentCheckpoint.GetCheckpointPriority() <= checkpoint.GetCheckpointPriority())
        {
            currentCheckpoint = checkpoint;
            OnCurrentCheckpointChange?.Invoke(this, EventArgs.Empty);
            return true;
        }

        return false;
    }

    public void ChangeCheckpoint(Checkpoint checkpoint)
    {
        currentCheckpoint = checkpoint;
        OnCurrentCheckpointChange?.Invoke(this, EventArgs.Empty);
    }
}
