using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeveloperInterface : MonoBehaviour
{
    public static DeveloperInterface Instance { get; private set; }

    [SerializeField] private Button returnToMainMenuButton;

    private bool isFirstUpdate = true;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        returnToMainMenuButton.onClick.AddListener(() =>
        {
            MainMenuInterface.Instance.Show();
            Hide();
        });
    }

    private void Update()
    {
        if (isFirstUpdate)
        {
            isFirstUpdate = false;
            Hide();
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public bool IsShown()
    {
        return gameObject.activeSelf;
    }
}
