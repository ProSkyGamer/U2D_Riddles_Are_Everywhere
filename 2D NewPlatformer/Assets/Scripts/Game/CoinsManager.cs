using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoinsManager
{
    private static string COINS_PLAYER_PREFS = "PlayerCoins";

    public static bool IsEnoughCoins(int toBuy)
    {
        return PlayerPrefs.GetInt(COINS_PLAYER_PREFS) >= toBuy;
    }

    public static void SpendCoins(int toSpend)
    {
        PlayerPrefs.SetInt(COINS_PLAYER_PREFS, PlayerPrefs.GetInt(COINS_PLAYER_PREFS) - toSpend);
    }

    public static void AddCoins(int toAdd)
    {
        PlayerPrefs.SetInt(COINS_PLAYER_PREFS, PlayerPrefs.GetInt(COINS_PLAYER_PREFS) + toAdd);
    }
}
