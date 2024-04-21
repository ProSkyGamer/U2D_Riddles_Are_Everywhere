using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseInterafce : MonoBehaviour
{
    public static PauseInterafce Instance { get; private set; }

    [SerializeField] private Button unpauseButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button mainMenuButton;

    private float storedTimeScale = 1f;

    private bool isFirstUpdate = true;

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
    }

    private void LateUpdate()
    {
        if (isFirstUpdate)
        {
            isFirstUpdate = false;
            Hide();
            GameStageManager.ChangeGameStage(GameStageManager.GameStages.Playing);
        }
    }

    private void Input_OnPauseGameAction(object sender, EventArgs e)
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
            storedTimeScale = Time.timeScale;
            Time.timeScale = 0f;
        }
        else
        {
            Hide();
            Time.timeScale = storedTimeScale;
        }
    }
}
