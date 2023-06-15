using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathInterface : MonoBehaviour
{
    [SerializeField] private Button levelRestartButton;

    private void Awake()
    {
        levelRestartButton.onClick.AddListener(() =>
        {
            UnitySceneManager.LoadScene(UnitySceneManager.GetCurrentScene());
        });
    }

    private void Start()
    {
        PlayerController.OnPlayerDie += PlayerController_OnPlayerDie;

        Hide();
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
}
