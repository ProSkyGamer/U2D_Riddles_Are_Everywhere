using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuInterface : MonoBehaviour
{
    public static MainMenuInterface Instance { get; private set; }

    [SerializeField] private Button chooseLevelButton;
    [SerializeField] private Button shopButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button developerButton;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        chooseLevelButton.onClick.AddListener(() =>
        {
            ChooseLevelInterface.Instance.Show();
            Hide();
        });

        shopButton.onClick.AddListener(() =>
        {
            ShopInterface.Instance.Show();
            Hide();
        });

        settingsButton.onClick.AddListener(() =>
        {
            SettingsMainMenuInterface.Instance.Show();
            Hide();
        });

        exitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        developerButton.onClick.AddListener(() =>
        {
            DeveloperInterface.Instance.Show();
            Hide();
        });
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
