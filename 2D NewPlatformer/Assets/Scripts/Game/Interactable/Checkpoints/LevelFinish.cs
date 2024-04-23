using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(AddInteractButtonUI))]
public class LevelFinish : InteractableItem
{
    [SerializeField] private float pointsToCoinsMultiplayer = 2;
    [SerializeField] private UnitySceneManager.Scenes levelScene;
    [SerializeField] private TextMeshProUGUI coinsMultiplierValueText;

    private void Awake()
    {
        if (LevelsCompletionController.CheckLevelCompletion(levelScene))
            pointsToCoinsMultiplayer = 1;

        coinsMultiplierValueText.text = $"x{pointsToCoinsMultiplayer}";
    }

    public override void OnInteract(PlayerController player)
    {
        base.OnInteract(player);

        player.DisableMovement();

        var earnedCoins = (int)(PointsCollectedController.Instance.GetCollectedPoints() * pointsToCoinsMultiplayer);
        
        WinInterface.Instance.Show(earnedCoins);

        CoinsManager.AddCoins(earnedCoins);
        
        LevelsCompletionController.SelectLevelAsCompleted(levelScene);
    }

    public override bool IsCanInteract()
    {
        return PointsCollectedController.Instance.IsEnoughPointsToFinish();
    }
}
