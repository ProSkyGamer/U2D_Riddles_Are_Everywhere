using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinInterface : MonoBehaviour
{
    public static WinInterface Instance { get; private set; }

    [SerializeField] private Button backToMenuButton;
    [SerializeField] private Button restartLevelButton;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        backToMenuButton.onClick.AddListener(() =>
        {
            UnitySceneManager.LoadScene(UnitySceneManager.Scenes.MainMenu);
        });

        restartLevelButton.onClick.AddListener(() =>
        {
            UnitySceneManager.LoadScene(UnitySceneManager.GetCurrentScene());
        });

        Hide();
    }

    public void Show()
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
