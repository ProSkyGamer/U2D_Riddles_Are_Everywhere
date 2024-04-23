using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LevelSO : ScriptableObject
{
    public UnitySceneManager.Scenes sceneToLoad;
    public TextTranslationsSO levelNameTextTranslationsSO;
    public bool resetGuides = false;
    public bool isLevelNew;
}
