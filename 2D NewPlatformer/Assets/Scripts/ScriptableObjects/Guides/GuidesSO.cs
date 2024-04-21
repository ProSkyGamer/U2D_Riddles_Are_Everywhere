using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class GuidesSO : ScriptableObject
{
    public GuideInterface.GuideType guideType;
    public Sprite guideImage;
    public TextTranslationsSO guideTextTranslations;
    public List<Input.Binding> requiredButtonToPress = new List<Input.Binding>();
}
