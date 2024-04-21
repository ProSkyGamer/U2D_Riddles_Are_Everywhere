using System;
using UnityEngine;
using UnityEngine.UI;

public class ChangePlayerInterface : MonoBehaviour
{
    public static ChangePlayerInterface Instance { get; private set; }

    [SerializeField] private Transform playerCardPrefab;
    [SerializeField] private Transform allPlayerCardsGrid;
    [SerializeField] private SelectedPlayerCard selectedPlayerCard;
    [SerializeField] private Button changePlayerButton;

    private bool isFirstUpdate = true;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        changePlayerButton.onClick.AddListener(() => { ChangeToggleVisibility(false); });
    }

    private void Start()
    {
        foreach (var playerSO in PlayerChangeController.Instance.GetAllPlayersSO().allPlayersSO)
        {
            var playerCard = Instantiate(playerCardPrefab, allPlayerCardsGrid);
            var isPlayerAvailable = ShopManager.IsCurrentPlayerBought(playerSO) ||
                                    PlayerChangeController.Instance.GetAlwaysAvailiblePlayers().Contains(playerSO);

            var playerCardSingle = playerCard.GetComponent<ChangePlayerCardSingle>();
            playerCardSingle.InitializePlayerCard(playerSO, isPlayerAvailable);
        }

        playerCardPrefab.gameObject.SetActive(false);

        Input.Instance.OnChangePlayerAction += Input_OnChangePlayerAction;
        ChangePlayerCardSingle.OnTryChangeActivePlayerCard += ChangePlayerCardSingle_OnTryChangeActivePlayerCard;
    }

    private void ChangePlayerCardSingle_OnTryChangeActivePlayerCard(object sender,
        ChangePlayerCardSingle.OnTryChangeActivePlayerCardEventArgs e)
    {
        selectedPlayerCard.TryChangeSelectedPlayerCard(e.playerSO);
    }

    private void LateUpdate()
    {
        if (isFirstUpdate)
        {
            isFirstUpdate = false;
            Hide();
        }
    }

    private void Input_OnChangePlayerAction(object sender, EventArgs e)
    {
        switch (GameStageManager.GetCurrentStage())
        {
            case GameStageManager.GameStages.Playing:
                GameStageManager.ChangeGameStage(GameStageManager.GameStages.ChoosingCharacter);
                break;
            case GameStageManager.GameStages.ChoosingCharacter:
                GameStageManager.ChangeGameStage(GameStageManager.GameStages.Playing);
                break;
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);

        selectedPlayerCard.TryChangeSelectedPlayerCard(
            PlayerChangeController.Instance.GetAllPlayersSO().allPlayersSO[0]);
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
}
