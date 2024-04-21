using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstCollectableObject : MonoBehaviour
{
    [SerializeField] private int pointsPerCollection = 1;
    private bool isCollected = false;
    public event EventHandler OnCollect;

    private CollectableObjectVisual objectVisual;

    private void Awake()
    {
        objectVisual = GetComponent<CollectableObjectVisual>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isCollected)
        {
            if (collision.gameObject.TryGetComponent(out PlayerController player))
            {
                OnCollect?.Invoke(this, EventArgs.Empty);
                isCollected = true;

                PointsCollectedController.Instance.AddPoints(pointsPerCollection);

                objectVisual.ChangeAnimationState(CollectableObjectVisual.CollectableObjectAnimations.Collected);
            }
        }
    }

    public void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}
