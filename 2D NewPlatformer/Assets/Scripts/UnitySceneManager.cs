using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class UnitySceneManager
{
    public enum Scenes
    {
        GameScene,
    }

    public static void LoadScene(Scenes sceneToLoad)
    {
        ReserStaticData.ResetStaticData();
        SceneManager.LoadScene((int)sceneToLoad);
    }

    public static Scenes GetCurrentScene()
    {
        return (Scenes)SceneManager.GetActiveScene().buildIndex;
    }
}
