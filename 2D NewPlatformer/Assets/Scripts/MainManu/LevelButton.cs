using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private UnitySceneManager.Scenes sceneToLoad;
    [SerializeField] private TextTranslationUI levelNameTextTranslationUI;
    [SerializeField] private Transform newLevelInfoTransform;

    private bool isInitialized;

    private void Awake()
    {
        if (sceneToLoad == UnitySceneManager.Scenes.Level0)
        {
            InitializedLevelButton(UnitySceneManager.Scenes.Level0, true);
            isInitialized = true;
        }
    }

    public void InitializedLevelButton(UnitySceneManager.Scenes scene, bool resetGuides = false)
    {
        if (isInitialized) return;

        isInitialized = true;
        sceneToLoad = scene;
        var button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        if (resetGuides)
            button.onClick.AddListener(() => { GuidesReset.Instance.ResetAllGuides(); });
        button.onClick.AddListener(() => { UnitySceneManager.LoadScene(sceneToLoad); });
        newLevelInfoTransform.gameObject.SetActive(false);

        if (scene != UnitySceneManager.Scenes.Level0)
            if (!LevelsCompletionController.CheckLevelCompletion(UnitySceneManager.Scenes.Level0))
                button.interactable = false;
    }

    public void InitializedLevelButton(LevelSO levelSO)
    {
        if (isInitialized) return;

        isInitialized = true;
        sceneToLoad = levelSO.sceneToLoad;
        var resetGuides = levelSO.resetGuides;
        var button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        if (resetGuides)
            button.onClick.AddListener(() => { GuidesReset.Instance.ResetAllGuides(); });
        button.onClick.AddListener(() => { UnitySceneManager.LoadScene(sceneToLoad); });
        levelNameTextTranslationUI.ChangeTextTranslationSO(levelSO.levelNameTextTranslationsSO);
        newLevelInfoTransform.gameObject.SetActive(levelSO.isLevelNew);

        if (sceneToLoad != UnitySceneManager.Scenes.Level0)
            if (!LevelsCompletionController.CheckLevelCompletion(UnitySceneManager.Scenes.Level0))
                button.interactable = false;
    }
}
