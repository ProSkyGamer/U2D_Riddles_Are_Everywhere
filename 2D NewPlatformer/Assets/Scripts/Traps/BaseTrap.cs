using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTrap : MonoBehaviour
{
    [SerializeField] protected int trapDamage = 1;
    [SerializeField] protected float dealDamageCooldown = 1.5f;
    protected float dealDamageTimer;
    protected List<PlayerController> playerToDamageList = new List<PlayerController>();

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController playerController;
        if (collision.gameObject.TryGetComponent<PlayerController>(out playerController))
        {
            playerToDamageList.Add(playerController);
            dealDamageTimer = 0;
        }
    }

    protected void OnCollisionExit2D(Collision2D collision)
    {
        PlayerController playerController;
        if (collision.gameObject.TryGetComponent<PlayerController>(out playerController))
        {
            playerToDamageList.Remove(playerController);
        }
    }

    protected void Awake()
    {
        dealDamageTimer = 0f;
    }

    protected void Update()
    {
        if (dealDamageTimer > 0)
            dealDamageTimer -= Time.deltaTime;
        else
        {
            if (playerToDamageList.Count > 0)
            {
                foreach (PlayerController playerToDamage in playerToDamageList)
                {
                    playerToDamage.TakeDamage(trapDamage);
                }
                dealDamageTimer = dealDamageCooldown;
            }
        }
    }
}
