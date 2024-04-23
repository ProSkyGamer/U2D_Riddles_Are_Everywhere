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

        foreach (var levelSO in allLevelsSO.allLevelsSO)
        {
            var levelButton = Instantiate(levelButtonPrefab, levelButtonGrid);
            levelButton.GetComponent<LevelButton>().InitializedLevelButton(levelSO);
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
