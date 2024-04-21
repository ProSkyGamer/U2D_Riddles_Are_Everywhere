using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinInterface : MonoBehaviour
{
    public static WinInterface Instance { get; private set; }

    [SerializeField] private Button backToMenuButton;
    [SerializeField] private Button restartLevelButton;
    [SerializeField] private TextMeshProUGUI earnedCoinsText;

    private bool isFirstUpdate = true;

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
    }

    private void LateUpdate()
    {
        if (isFirstUpdate)
        {
            isFirstUpdate = false;
            Hide();
        }
    }

    public void Show(int coinsEarned)
    {
        gameObject.SetActive(true);
        earnedCoinsText.text = coinsEarned.ToString();
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
