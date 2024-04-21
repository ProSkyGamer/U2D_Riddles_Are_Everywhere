using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ShopManager
{
    public static event EventHandler<OnPlayerBoughtEventArgs> OnPlayerBought;
    public class OnPlayerBoughtEventArgs : EventArgs
    {
        public ShopItemSO shopItem;
    }

    private const string ADDITIONAL_PLAYER_PREFS_TEXT = "PlayerPrefs";

    public static bool IsCurrentItemBought(ShopItemSO itemSO)
    {
        return PlayerPrefs.GetInt(GetPlayerPrefsStringBaseOnShopItemSO(itemSO), 0) == 1;
    }

    public static bool IsCurrentPlayerBought(PlayerSO playerSO)
    {
        return PlayerPrefs.GetInt(GetPlayerPrefsStringBaseOnShopItemSO(playerSO), 0) == 1;
    }

    public static void AddBoughtItem(ShopItemSO itemSO)
    {
        PlayerPrefs.SetInt(GetPlayerPrefsStringBaseOnShopItemSO(itemSO), 1);
        OnPlayerBought?.Invoke(null, new OnPlayerBoughtEventArgs()
        {
            shopItem = itemSO
        });
    }

    private static string GetPlayerPrefsStringBaseOnShopItemSO(ShopItemSO itemSO)
    {
        return itemSO.playerToBuy.name + ADDITIONAL_PLAYER_PREFS_TEXT;
    }
    
    private static string GetPlayerPrefsStringBaseOnShopItemSO(PlayerSO playerSO)
    {
        return playerSO.name + ADDITIONAL_PLAYER_PREFS_TEXT;
    }
}
