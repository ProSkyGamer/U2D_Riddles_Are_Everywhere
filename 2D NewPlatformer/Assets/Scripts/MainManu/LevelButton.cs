using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    private UnitySceneManager.Scenes sceneToLoad;

    public void ChangeLevelScene(UnitySceneManager.Scenes scene, bool resetGuides = false)
    {
        sceneToLoad = scene;
        Button button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        if (resetGuides)
            button.onClick.AddListener(() =>
            {
                GuidesReset.Instance.ResetAllGuides();
            });
        button.onClick.AddListener(() =>
        {
            UnitySceneManager.LoadScene(sceneToLoad);
        });
    }
}
