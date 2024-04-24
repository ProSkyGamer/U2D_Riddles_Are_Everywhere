using UnityEngine.SceneManagement;

public static class UnitySceneManager
{
    public enum Scenes
    {
        MainMenu,
        Level0,
        Level1,
        Level2,
        Level3,
        Level4
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
