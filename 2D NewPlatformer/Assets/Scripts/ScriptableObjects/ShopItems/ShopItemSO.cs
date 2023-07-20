using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ShopItemSO : ScriptableObject
{
    public PlayerSO playerToBuy;
    public int playerCost;
}
