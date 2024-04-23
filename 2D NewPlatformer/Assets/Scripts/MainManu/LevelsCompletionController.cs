using UnityEngine;

public static class LevelsCompletionController
{
    public static void SelectLevelAsCompleted(UnitySceneManager.Scenes levelScene)
    {
        PlayerPrefs.SetInt(GetLevelPlayerPrefsString(levelScene), 1);
    }

    private static string GetLevelPlayerPrefsString(UnitySceneManager.Scenes levelScene)
    {
        return levelScene + "CompletionPlayerPrefs";
    }

    public static bool CheckLevelCompletion(UnitySceneManager.Scenes levelScene)
    {
        return PlayerPrefs.GetInt(GetLevelPlayerPrefsString(levelScene), 0) == 1;
    }
}
