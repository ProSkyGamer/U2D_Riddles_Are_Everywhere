using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseInterafce : MonoBehaviour
{
    public static PauseInterafce Instance { get; private set; }

    [SerializeField] private Button unpauseButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        unpauseButton.onClick.AddListener(() =>
        {
            GameStageManager.ChangeGameStage(GameStageManager.GameStages.Playing);
        });
        settingsButton.onClick.AddListener(() =>
        {
            SettingsInterface.Instance.Show();
            Hide();
        });
        mainMenuButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            UnitySceneManager.LoadScene(UnitySceneManager.Scenes.MainMenu);
        });
    }

    private void Start()
    {
        Input.Instance.OnPauseGameAction += Input_OnPauseGameAction;

        Hide();
    }

    private void Input_OnPauseGameAction(object sender, System.EventArgs e)
    {
        if (GuideInterface.Instance.IsShown() || WinInterface.Instance.IsShown() ||
            DeathInterface.Instance.IsShown())
            return;

        if (SettingsInterface.Instance.IsShown())
        {
            Show();
            SettingsInterface.Instance.Hide();
            return;
        }

        switch (GameStageManager.GetCurrentStage())
        {
            case GameStageManager.GameStages.MainMenu:
                break;
            case GameStageManager.GameStages.Playing:
                GameStageManager.ChangeGameStage(GameStageManager.GameStages.Pause);
                break;
            case GameStageManager.GameStages.Pause:
                GameStageManager.ChangeGameStage(GameStageManager.GameStages.Playing);
                break;
            case GameStageManager.GameStages.ChoosingCharacter:
                GameStageManager.ChangeGameStage(GameStageManager.GameStages.Playing);
                break;
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void ChangePauseToggle(bool isPaused)
    {
        if (isPaused)
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
