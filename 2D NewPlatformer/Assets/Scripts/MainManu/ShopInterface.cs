using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopInterface : MonoBehaviour
{
    public static ShopInterface Instance { get; private set; }

    [SerializeField] private Button returnToMainMenuButton;

    [SerializeField] private AllShopItemsSO allShopItems;
    [SerializeField] private Transform shopItemsGrid;
    [SerializeField] private Transform shopItemPrefab;

    private bool isFirstUpdate = true;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        returnToMainMenuButton.onClick.AddListener(() =>
        {
            MainMenuInterface.Instance.Show();
            Hide();
        });

        for (int i = 0; i < allShopItems.allShopItems.Length; i++)
        {
            var shopItem = Instantiate(shopItemPrefab, shopItemsGrid);

            shopItem.GetComponent<ShopSingleItemUI>().ChangeShopItem(allShopItems.allShopItems[i]);
        }

        SelectedShopItemUI.Instance.ChangeSelectedCard(allShopItems.allShopItems[0]);

        shopItemPrefab.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isFirstUpdate)
        {
            isFirstUpdate = false;
            Hide();
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public bool IsShown()
    {
        return gameObject.activeSelf;
    }
}
