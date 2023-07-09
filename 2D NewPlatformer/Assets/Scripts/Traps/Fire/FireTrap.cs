using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FireTrapVisual))]
public class FireTrap : BaseTrap
{
    [Header("Fire Trap Settings")]
    [SerializeField] private float activationTime = 1f;
    private float activationTimer;

    private FireTrapVisual fireTrapVisual;

    protected override void Awake()
    {
        base.Awake();

        fireTrapVisual = GetComponent<FireTrapVisual>();
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController player;
        if (collision.gameObject.TryGetComponent<PlayerController>(out player))
        {
            playerToDamageList.Add(player);
        }
    }

    protected override void OnCollisionExit2D(Collision2D collision)
    {
        PlayerController player;
        if (collision.gameObject.TryGetComponent<PlayerController>(out player))
        {
            playerToDamageList.Remove(player);
        }
    }

    protected override void Update()
    {
        if(playerToDamageList.Count > 0)
        {
            if (activationTimer < activationTime)
            {
                if(fireTrapVisual.GetCurrentAnimationState() != FireTrapVisual.AnimationStates.TurningOn)
                    fireTrapVisual.ChangeAnimationState(FireTrapVisual.AnimationStates.TurningOn, activationTime - activationTimer);
                activationTimer += Time.deltaTime;
            }
            else
            {
                if (fireTrapVisual.GetCurrentAnimationState() != FireTrapVisual.AnimationStates.On)
                    fireTrapVisual.ChangeAnimationState(FireTrapVisual.AnimationStates.On);

                if (dealDamageTimer > 0)
                    dealDamageTimer -= Time.deltaTime;
                else
                {
                    foreach (PlayerController playerToDamage in playerToDamageList)
                    {
                        if(IsCurrentPlayerInteractable(PlayerChangeController.Instance.GetCurrentPlayerSO()))
                            playerToDamage.TakeDamage(trapDamage);
                    }

                    dealDamageTimer = dealDamageCooldown;
                }
            }
        }
        else
        {
            if (fireTrapVisual.GetCurrentAnimationState() != FireTrapVisual.AnimationStates.TurningOff ||
                fireTrapVisual.GetCurrentAnimationState() != FireTrapVisual.AnimationStates.Off)
                fireTrapVisual.ChangeAnimationState(FireTrapVisual.AnimationStates.TurningOff, activationTime - activationTimer);

            if (activationTimer > 0)
                 activationTimer -= Time.deltaTime;
            else
                fireTrapVisual.ChangeAnimationState(FireTrapVisual.AnimationStates.Off);

        }
    }
}
