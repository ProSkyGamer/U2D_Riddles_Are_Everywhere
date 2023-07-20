using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectedShopItemUI : MonoBehaviour
{
    public static SelectedShopItemUI Instance { get; private set; }

    [SerializeField] private Image selectedCardImage;
    [SerializeField] private TextMeshProUGUI selectedCardCostText;
    [SerializeField] private TextMeshProUGUI selectedCardDescriptionText;
    [SerializeField] private Button selectedCardButton;

    private ShopItemSO currentShopItem;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void ChangeSelectedCard(ShopItemSO shopItem)
    {
        currentShopItem = shopItem;
        selectedCardButton.interactable = !ShopManager.IsCurrentItemBought(shopItem) && CoinsManager.IsEnoughCoins(shopItem.playerCost);
        selectedCardButton.onClick.RemoveAllListeners();
        selectedCardButton.onClick.AddListener(() =>
        {
            ShopManager.AddBoughtItem(shopItem);
            CoinsManager.SpendCoins(shopItem.playerCost);
        });
        ChangeSelectedCardVisual();
    }

    private void ChangeSelectedCardVisual()
    {
        selectedCardImage.sprite = currentShopItem.playerToBuy.playerSprite;
        selectedCardCostText.text = currentShopItem.playerCost.ToString();
        selectedCardDescriptionText.text = TextTranslationManager.GetTextFromTextTranslationSOByLanguage(
            TextTranslationManager.GetCurrentLanguage(), currentShopItem.playerToBuy.playerDescriptionText);
    }
}
