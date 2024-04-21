using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SandMud : MonoBehaviour
{
    public event EventHandler OnPlayerSlowDown;

    [SerializeField, Range(0,1)] private float sandMudSlowDown = 0.9f;
    [SerializeField] private PlayerSO[] notInteractablePlayersSOArray;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsCurrentPlayerInteractable(PlayerChangeController.Instance.GetCurrentPlayerSO()))
        {
            PlayerController player;
            if (collision.gameObject.TryGetComponent<PlayerController>(out player))
            {
                player.ChangeAllMoventSLowDown(1f - sandMudSlowDown);
                OnPlayerSlowDown?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        PlayerController player;
        if (collision.gameObject.TryGetComponent<PlayerController>(out player))
            player.ChangeAllMoventSLowDown(1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsCurrentPlayerInteractable(PlayerChangeController.Instance.GetCurrentPlayerSO()))
        { 
            PlayerController player;
            if (collision.gameObject.TryGetComponent<PlayerController>(out player))
            {
                player.ChangeAllMoventSLowDown(1f - sandMudSlowDown);
                OnPlayerSlowDown?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerController player;
        if (collision.gameObject.TryGetComponent<PlayerController>(out player))
            player.ChangeAllMoventSLowDown(1f);
    }

    private bool IsCurrentPlayerInteractable(PlayerSO player)
    {
        foreach(PlayerSO playerSO in notInteractablePlayersSOArray)
        {
            if (playerSO == player)
                return false;
        }
        return true;
    }
}
