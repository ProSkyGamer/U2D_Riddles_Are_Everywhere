using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoinsManager
{
    public static event EventHandler<OnCoinsValueChangeEventArgs> OnCoinsValueChange;
    public class OnCoinsValueChangeEventArgs : EventArgs
    {
        public int currentCoins;
    }

    private static string COINS_PLAYER_PREFS = "PlayerCoins";

    public static bool IsEnoughCoins(int toBuy)
    {
        return PlayerPrefs.GetInt(COINS_PLAYER_PREFS) >= toBuy;
    }

    public static void SpendCoins(int toSpend)
    {
        PlayerPrefs.SetInt(COINS_PLAYER_PREFS, PlayerPrefs.GetInt(COINS_PLAYER_PREFS) - toSpend);
        OnCoinsValueChange?.Invoke(null, new OnCoinsValueChangeEventArgs() {
            currentCoins = PlayerPrefs.GetInt(COINS_PLAYER_PREFS)
        });
    }

    public static void AddCoins(int toAdd)
    {
        PlayerPrefs.SetInt(COINS_PLAYER_PREFS, PlayerPrefs.GetInt(COINS_PLAYER_PREFS) + toAdd);
        OnCoinsValueChange?.Invoke(null, new OnCoinsValueChangeEventArgs()
        {
            currentCoins = PlayerPrefs.GetInt(COINS_PLAYER_PREFS)
        });
    }

    public static int GetCurrentCoins()
    {
        return PlayerPrefs.GetInt(COINS_PLAYER_PREFS);
    }

    public static void ResetStaticData()
    {
        OnCoinsValueChange = null;
    }
}
