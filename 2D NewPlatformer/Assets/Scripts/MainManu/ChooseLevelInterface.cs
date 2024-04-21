using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChooseLevelInterface : MonoBehaviour
{
    public static ChooseLevelInterface Instance { get; private set; }

    [SerializeField] private Button returnToMainMenuButton;
    [SerializeField] private AllLevelsSO allLevelsSO;
    [SerializeField] private Transform levelButtonPrefab;
    [SerializeField] private Transform levelButtonGrid;

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

        for(int i = 0; i < allLevelsSO.allLevelsSO.Length; i++)
        {
            var levelButton = Instantiate(levelButtonPrefab, levelButtonGrid);
            levelButton.GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();
            levelButton.GetComponent<LevelButton>().ChangeLevelScene(allLevelsSO.allLevelsSO[i].
                sceneToLoad, allLevelsSO.allLevelsSO[i].resetGuides);
        }
        levelButtonPrefab.gameObject.SetActive(false);
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
