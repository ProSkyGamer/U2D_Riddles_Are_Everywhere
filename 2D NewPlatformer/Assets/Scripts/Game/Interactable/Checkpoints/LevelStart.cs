using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStart : Checkpoint
{
    public static LevelStart Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    protected override void Start() {}

    public override bool IsCanInteract()
    {
        return false;
    }
}
