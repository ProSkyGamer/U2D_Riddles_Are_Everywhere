using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStart : Checkpoint
{
    public static LevelStart Instance { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    protected override void Update() {}
    protected override void OnDrawGizmosSelected() {}
    protected override void Start() {}
}
