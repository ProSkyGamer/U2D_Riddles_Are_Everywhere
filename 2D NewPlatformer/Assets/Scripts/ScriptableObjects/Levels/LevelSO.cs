using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LevelSO : ScriptableObject
{
    public UnitySceneManager.Scenes sceneToLoad;
    public bool resetGuides = false;
}
