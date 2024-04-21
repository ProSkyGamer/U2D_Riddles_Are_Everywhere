using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AddInteractButtonUI))]
public class LevelFinish : InteractableItem
{
    [SerializeField] private int pointsToCoinsMultiplayer = 2;

    public override void OnInteract(PlayerController player)
    {
        base.OnInteract(player);

        player.DisableMovement();

        WinInterface.Instance.Show(PointsCollectedController.Instance.GetCollectedPoints() * pointsToCoinsMultiplayer);

        CoinsManager.AddCoins(PointsCollectedController.Instance.GetCollectedPoints() * pointsToCoinsMultiplayer);
    }

    public override bool IsCanInteract()
    {
        return PointsCollectedController.Instance.IsEnoughPointsToFinish();
    }
}
