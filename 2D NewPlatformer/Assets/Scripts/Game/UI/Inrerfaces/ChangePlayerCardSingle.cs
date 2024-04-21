using System;
using UnityEngine;
using UnityEngine.UI;

public class ChangePlayerCardSingle : MonoBehaviour
{
    public static event EventHandler<OnTryChangeActivePlayerCardEventArgs> OnTryChangeActivePlayerCard;

    public class OnTryChangeActivePlayerCardEventArgs : EventArgs
    {
        public PlayerSO playerSO;
    }

    [SerializeField] private Image playerCardImage;
    [SerializeField] private TextTranslationUI playerCardName;
    private Button playerCardButton;
    private PlayerSO cardPlayerSO;

    private bool isInitialized;

    private void Awake()
    {
        playerCardButton = GetComponent<Button>();
    }

    public void InitializePlayerCard(PlayerSO newPlayerSO, bool isAvailable)
    {
        if (isInitialized) return;

        isInitialized = true;
        cardPlayerSO = newPlayerSO;
        playerCardImage.sprite = newPlayerSO.playerSprite;
        playerCardName.ChangeTextTranslationSO(newPlayerSO.playerName);

        playerCardButton.onClick.AddListener(() =>
        {
            OnTryChangeActivePlayerCard?.Invoke(this, new OnTryChangeActivePlayerCardEventArgs
            {
                playerSO = cardPlayerSO
            });
        });

        if (!isAvailable)
            playerCardButton.interactable = false;
    }
}
