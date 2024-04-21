using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSingleItemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemCostText;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private Image itemImage;
    [SerializeField] private Button itemButton;
    [SerializeField] private Transform lockedImage;
    [SerializeField] private Transform soldOutTransform;

    private ShopItemSO shopItem;

    private bool isFirstUpdate = true;

    private void Update()
    {
        if(isFirstUpdate)
        {
            isFirstUpdate = false;

            if (!ShopManager.IsCurrentItemBought(shopItem))
            {
                soldOutTransform.gameObject.SetActive(false);
                if (CoinsManager.IsEnoughCoins(shopItem.playerCost))
                    lockedImage.gameObject.SetActive(false);

                CoinsManager.OnCoinsValueChange += CoinsManager_OnCoinsValueChange;
                ShopManager.OnPlayerBought += ShopManager_OnPlayerBought;
            }
        }
    }

    private void ShopManager_OnPlayerBought(object sender, ShopManager.OnPlayerBoughtEventArgs e)
    {
        if(e.shopItem == shopItem)
        {
            lockedImage.gameObject.SetActive(true);
            soldOutTransform.gameObject.SetActive(true);
            CoinsManager.OnCoinsValueChange -= CoinsManager_OnCoinsValueChange;
            ShopManager.OnPlayerBought -= ShopManager_OnPlayerBought;
        }
    }

    private void CoinsManager_OnCoinsValueChange(object sender, CoinsManager.OnCoinsValueChangeEventArgs e)
    {
        if(CoinsManager.IsEnoughCoins(shopItem.playerCost))
            lockedImage.gameObject.SetActive(false);
        else
            lockedImage.gameObject.SetActive(true);
    }

    public void ChangeShopItem(ShopItemSO shopItem)
    {
        this.shopItem = shopItem;
        itemButton.onClick.RemoveAllListeners();
        itemButton.onClick.AddListener(() =>
        {
            SelectedShopItemUI.Instance.ChangeSelectedCard(this.shopItem);
        });
        ChangeItemVisual();
    }

    private void ChangeItemVisual()
    {
        itemCostText.text = shopItem.playerCost.ToString();
        itemNameText.GetComponent<TextTranslationUI>().ChangeTextTranslationSO(shopItem.playerToBuy.playerName);
        itemImage.sprite = shopItem.playerToBuy.playerSprite;
    }
}
