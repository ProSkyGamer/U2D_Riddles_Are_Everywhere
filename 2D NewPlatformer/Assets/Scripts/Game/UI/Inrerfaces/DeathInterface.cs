using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathInterface : MonoBehaviour
{
    public static DeathInterface Instance { get; private set; }

    [SerializeField] private Button levelRestartButton;
    [SerializeField] private Button mainMenuButton;

    private bool isFirstUpdate = true;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        levelRestartButton.onClick.AddListener(() =>
        {
            UnitySceneManager.LoadScene(UnitySceneManager.GetCurrentScene());
        });

        mainMenuButton.onClick.AddListener(() =>
        {
            UnitySceneManager.LoadScene(UnitySceneManager.Scenes.MainMenu);
        });
    }

    private void LateUpdate()
    {
        if(isFirstUpdate)
        {
            isFirstUpdate = false;
            Hide();
        }
    }

    private void Start()
    {
        PlayerController.OnPlayerDie += PlayerController_OnPlayerDie;
    }

    private void PlayerController_OnPlayerDie(object sender, System.EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public bool IsShown()
    {
        return gameObject.activeSelf;
    }
}
