using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChangePlayerInterface : MonoBehaviour
{
    public static ChangePlayerInterface Instance { get; private set; }

    [SerializeField] private List<PlayerSO> alwaysAvailiblePlayers = new();
    [SerializeField] private Transform playerCardPrefab;
    [SerializeField] private Transform allPlayerCardsGrid;
    [SerializeField] SelectedPlayerCard selectedPlayerCard;
    [SerializeField] private Button changePlayerButton;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        changePlayerButton.onClick.AddListener(() =>
        {
            ChangeToggleVisibility(false);
        });
    }

    private void Start()
    {
        foreach (PlayerSO playerSO in PlayerChangeController.Instance.GetAllPlayersSO().allPlayersSO)
        {
            Transform playerCard = Instantiate(playerCardPrefab, allPlayerCardsGrid);
            Button playerCardButton = playerCard.GetComponent<Button>();
            playerCardButton.onClick.AddListener(() =>
            {
                selectedPlayerCard.TryChangeSelectedPlayerCard(playerSO);
            });
            playerCard.GetComponentInChildren<TextMeshProUGUI>().text = TextTranslationManager.GetTextFromTextTranslationSOByLanguage(
                TextTranslationManager.GetCurrentLanguage(), playerSO.playerName);
            playerCard.GetComponentsInChildren<Image>()[1].sprite = playerSO.playerSprite;

            if (!ShopManager.IsCurrentPlayerBought(playerSO) && !alwaysAvailiblePlayers.Contains(playerSO))
                playerCardButton.interactable = false;
        }

        playerCardPrefab.gameObject.SetActive(false);

        Input.Instance.OnChangePlayerAction += Input_OnChangePlayerAction;

        Hide();
    }

    private void Input_OnChangePlayerAction(object sender, System.EventArgs e)
    {
        switch (GameStageManager.GetCurrentStage())
        {
            case GameStageManager.GameStages.MainMenu:
                break;
            case GameStageManager.GameStages.Playing:
                GameStageManager.ChangeGameStage(GameStageManager.GameStages.ChoosingCharacter);
                break;
            case GameStageManager.GameStages.Pause:
                break;
            case GameStageManager.GameStages.ChoosingCharacter:
                GameStageManager.ChangeGameStage(GameStageManager.GameStages.Playing);
                break;
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);

        selectedPlayerCard.TryChangeSelectedPlayerCard(PlayerChangeController.Instance.GetAllPlayersSO().allPlayersSO[0]);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ChangeToggleVisibility(bool isVisible)
    {
        if (isVisible)
        {
            Show();
            Time.timeScale = 0f;
        }
        else
        {
            Hide();
            Time.timeScale = 1f;
        }
    }

    public List<PlayerSO> GetAlwaysAvailiblePlayers()
    {
        return alwaysAvailiblePlayers;
    }
}
