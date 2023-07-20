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

    private ShopItemSO shopItem;

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
        itemNameText.text = TextTranslationManager.GetTextFromTextTranslationSOByLanguage(
            TextTranslationManager.GetCurrentLanguage(), shopItem.playerToBuy.playerName);
        itemImage.sprite = shopItem.playerToBuy.playerSprite;
    }
}
