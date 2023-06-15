using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimations : MonoBehaviour
{
    private const string ANIMATOR_PARAMETER = "Animation";

    public enum Animations
    {
        Iddle,
        Run,
        Jump,
        Fall,
        DoubleJump,
        WallJump,
        Hit,
        Die,
    }


    private Animator animator;
    private SpriteRenderer playerSprite;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerSprite = GetComponentInChildren<SpriteRenderer>();
    }

    public void ChangeAnimation(Animations animationToPlay)
    {
        animator.SetInteger(ANIMATOR_PARAMETER,(int)animationToPlay);
    }

    public void FlipPlayerSprite(bool isFlipped)
    {
        if(playerSprite.flipX != isFlipped)
            playerSprite.flipX = isFlipped;
    }

    public Animations GetCurrentAnimationState()
    {
        return (Animations)animator.GetInteger(ANIMATOR_PARAMETER);
    }
}
