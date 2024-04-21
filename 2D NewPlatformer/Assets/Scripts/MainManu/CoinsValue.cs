using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinsValue : MonoBehaviour
{
    private TextMeshProUGUI coinsText;

    private void Awake()
    {
        coinsText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        CoinsManager.OnCoinsValueChange += CoinsManager_OnCoinsValueChange;

        ChangeCoinsText(CoinsManager.GetCurrentCoins());
    }

    private void CoinsManager_OnCoinsValueChange(object sender, CoinsManager.OnCoinsValueChangeEventArgs e)
    {
        ChangeCoinsText(e.currentCoins);
    }

    private void ChangeCoinsText(int coinsValue)
    {
        coinsText.text = coinsValue.ToString();
    }
}
