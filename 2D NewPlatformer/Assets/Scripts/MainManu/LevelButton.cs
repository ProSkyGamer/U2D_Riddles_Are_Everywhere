using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    private UnitySceneManager.Scenes sceneToLoad;

    public void ChangeLevelScene(UnitySceneManager.Scenes scene)
    {
        sceneToLoad = scene;
        Button button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            UnitySceneManager.LoadScene(sceneToLoad);
        });
    }
}
