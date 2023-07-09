using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TrampolineVisual))]
public class Trampoline : MonoBehaviour
{
    [SerializeField] private float trampolineJumpForce = 9f;

    private TrampolineVisual trampolineVisual;

    private void Awake()
    {
        trampolineVisual = GetComponent<TrampolineVisual>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovement player;
        if(collision.gameObject.TryGetComponent<PlayerMovement>(out player))
        {
            player.Jump(trampolineJumpForce);

            trampolineVisual.ChangeAnimationState(true);
        }    
    }
}
