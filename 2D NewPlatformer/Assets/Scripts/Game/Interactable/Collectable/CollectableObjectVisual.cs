using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObjectVisual : MonoBehaviour
{
    [SerializeField] private CollectableObjectAnimations fruitType = CollectableObjectAnimations.AppleIddle;

    private const string ANIMATOR_VALUE = "State";

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        ChangeAnimationState(fruitType);
    }

    public enum CollectableObjectAnimations
    {
        AppleIddle,
        BananasIddle,
        CherriesIddle,
        KiwiIddle,
        MelonIddle,
        OrangeIddle,
        PineappleIddle,
        StrawberryIddle,
        Collected,
    }

    public void ChangeAnimationState(CollectableObjectAnimations animation)
    {
        animator.SetInteger(ANIMATOR_VALUE, (int)animation);
    }
}
