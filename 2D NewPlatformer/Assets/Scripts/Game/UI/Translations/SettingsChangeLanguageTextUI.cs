using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingsChangeLanguageTextUI : MonoBehaviour
{
    private TMP_Dropdown languagesDropdown;

    private void Awake()
    {
        languagesDropdown = GetComponent<TMP_Dropdown>();

        languagesDropdown.options.Clear();
        languagesDropdown.options.Add(new TMP_Dropdown.OptionData(TextTranslationManager.Languages.English.ToString()));
        languagesDropdown.options.Add(new TMP_Dropdown.OptionData("Русский")); //Чтобы текст в Dropdown был на русском

        languagesDropdown.value = (int)TextTranslationManager.GetCurrentLanguage();
    }

    private void Start()
    {
        languagesDropdown.onValueChanged.AddListener(ChangeLanguage);
    }

    private void ChangeLanguage(int newValue)
    {
        TextTranslationManager.ChangeLanguage((TextTranslationManager.Languages)newValue);
    }
}
