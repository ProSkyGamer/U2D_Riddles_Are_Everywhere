using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseInterafce : MonoBehaviour
{
    public static PauseInterafce Instance { get; private set; }

    [SerializeField] private Button unpauseButton;

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
    }

    private void Start()
    {
        Input.Instance.OnPauseGameAction += Input_OnPauseGameAction;

        Hide();
    }

    private void Input_OnPauseGameAction(object sender, System.EventArgs e)
    {
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

    private void Show()
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
