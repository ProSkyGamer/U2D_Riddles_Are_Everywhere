using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextTranslationUI : MonoBehaviour
{
    [SerializeField] private TextTranslationsSO textTranslationsSO;

    private TextMeshProUGUI currentLabelText;

    private void Awake()
    {
        currentLabelText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        TextTranslationManager.OnLanguageChange += TextTranslationManager_OnLanguageChange;
    }

    private void TextTranslationManager_OnLanguageChange(object sender, TextTranslationManager.OnLanguageChangeEventArgs e)
    {
        currentLabelText.text = TextTranslationManager.GetTextFromTextTranslationSOByLanguage(e.language, textTranslationsSO);
    }

    public void ChangeTextTranslationSO(TextTranslationsSO textTranslationsSO)
    {
        this.textTranslationsSO = textTranslationsSO;
    }
}
