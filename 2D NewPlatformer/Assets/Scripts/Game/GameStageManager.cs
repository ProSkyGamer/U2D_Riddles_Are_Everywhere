using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStageManager
{
    public enum GameStages
    {
        MainMenu,
        Playing,
        Pause,
        ChoosingCharacter
    }

    private static GameStages currentGameStage = GameStages.Playing;

    public static void ChangeGameStage(GameStages newStage)
    {
        currentGameStage = newStage;

        switch (currentGameStage)
        {
            case (GameStages.MainMenu):
                break;
            case (GameStages.Playing):
                PauseInterafce.Instance.ChangePauseToggle(false);
                ChangePlayerInterface.Instance.ChangeToggleVisibility(false);
                break;
            case (GameStages.Pause):
                PauseInterafce.Instance.ChangePauseToggle(true);
                break;
            case (GameStages.ChoosingCharacter):
                ChangePlayerInterface.Instance.ChangeToggleVisibility(true);
                break;
        }
    }

    public static GameStages GetCurrentStage()
    {
        return currentGameStage;
    }
}
