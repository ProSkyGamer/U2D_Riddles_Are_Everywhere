using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GuideInterface : MonoBehaviour
{
    public static GuideInterface Instance { get; private set; }

    public enum GuideType
    {
        Default,
        TextWithBackground,
        TextWithoutBackground,
        RequireButtonPress
    }

    public event EventHandler OnGuideClose;

    [SerializeField] private Button closeGuideButton;

    [SerializeField] private Image defaultGuideImage;
    [SerializeField] private TextMeshProUGUI defaultGuideText;
    [SerializeField] private Button defaultGuideShowNextButton;
    [SerializeField] private Button defaultGuideShowPreviousButton;
    [SerializeField] private Transform defaultGuideTransform;

    [SerializeField] private Transform textGuideWithBackgroundTransform;
    [SerializeField] private TextMeshProUGUI textGuideWithBackgroundText;
    [SerializeField] private OnSpikesDamage textGuideWithBackgroundNextButton;

    [SerializeField] private Transform textGuideWithoutBackgroundTransform;
    [SerializeField] private TextMeshProUGUI textGuideWithoutBackgroundText;

    [SerializeField] private Transform requireButtonPressGuideTransform;
    [SerializeField] private TextMeshProUGUI requireButtonPressGuideText;
    [SerializeField] private Transform requireButtonPressGuideRequiredButtonPrefab;
    [SerializeField] private Transform requireButtonPressGuideRequiredButtonGrid;
    private List<Input.Binding> requiredBindingsToPress = new List<Input.Binding>();

    private List<GuidesSO> guidesToShow = new();
    private int currentGuideIndex = -1;

    private bool isFirstUpdate = true;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        closeGuideButton.onClick.AddListener(() =>
        {
            Hide();
        });

        requireButtonPressGuideRequiredButtonPrefab.gameObject.SetActive(false);

        defaultGuideShowNextButton.onClick.AddListener(() =>
        {
            NextGuide();
        });

        defaultGuideShowPreviousButton.onClick.AddListener(() =>
        {
            PreviousGuide();
        });
    }

    private void LateUpdate()
    {
        if (isFirstUpdate)
        {
            isFirstUpdate = false;
            Hide();
        }
    }

    private void Update()
    {
        if (requiredBindingsToPress.Count > 0)
        {
            foreach (var binding in requiredBindingsToPress)
            {
                if (Input.Instance.GetButtonValue(binding) == 0)
                    return;
            }
            Hide();
        }
    }

    private void Start()
    {
        Input.Instance.OnNextGuideAction += Input_OnNextGuideAction;
        Input.Instance.OnPreviousGuideAction += Input_OnPreviousGuideAction;
    }

    private void Input_OnPreviousGuideAction(object sender, System.EventArgs e)
    {
        PreviousGuide();
    }

    private void Input_OnNextGuideAction(object sender, System.EventArgs e)
    {
        NextGuide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        HideAllGuides();
        guidesToShow.Clear();
        OnGuideClose?.Invoke(this, EventArgs.Empty);

        Time.timeScale = 1f;
    }

    private void HideAllGuides()
    {
        textGuideWithBackgroundTransform.gameObject.SetActive(false);
        defaultGuideTransform.gameObject.SetActive(false);
        textGuideWithoutBackgroundTransform.gameObject.SetActive(false);

        foreach (var binding in requireButtonPressGuideRequiredButtonGrid.GetComponentsInChildren<Transform>())
        {
            if (binding != requireButtonPressGuideRequiredButtonGrid && binding != requireButtonPressGuideRequiredButtonPrefab)
                Destroy(binding.gameObject);
        }
        requiredBindingsToPress.Clear();
        requireButtonPressGuideTransform.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public bool IsShown()
    {
        return gameObject.activeSelf;
    }

    public void ShowGuide(GuidesSO guideToSet)
    {
        if (guidesToShow.Count == 0)
        {
            Hide();
            Show();

            guidesToShow.Clear();
            guidesToShow.Add(guideToSet);

            currentGuideIndex = -1;
            NextGuide();
        }

    }

    public void ShowGuide(GuidesSO[] guideToSet)
    {
        if (guidesToShow.Count == 0)
        {
            Hide();
        Show();

        
            guidesToShow.Clear();
            guidesToShow.AddRange(guideToSet);

            currentGuideIndex = -1;
            NextGuide();
        }
    }

    private void NextGuide()
    {
        if (guidesToShow.Count > 0)
        {
            if (currentGuideIndex >= guidesToShow.Count - 1)
            {
                if (guidesToShow[guidesToShow.Count - 1].guideType == GuideType.Default)
                    Hide();
                else if (guidesToShow[guidesToShow.Count - 1].guideType == GuideType.TextWithBackground)
                    textGuideWithBackgroundNextButton.OnClick();

                return;
            }
            else
                currentGuideIndex++;
        }
        else
            return;

        DisplayThisGuide(guidesToShow[currentGuideIndex]);
    }

    private void PreviousGuide()
    {
        if (guidesToShow.Count > 0)
        {
            if (currentGuideIndex != 0)
                currentGuideIndex--;
            else
                return;
        }
        else
            return;

        DisplayThisGuide(guidesToShow[currentGuideIndex]);
    }

    private void DisplayThisGuide(GuidesSO guideToDiplay)
    {
        switch (guideToDiplay.guideType)
        {
            case GuideType.Default:
                Time.timeScale = 0f;
                defaultGuideTransform.gameObject.SetActive(true);

                if (currentGuideIndex == 0)
                    defaultGuideShowPreviousButton.interactable = false;
                else
                    defaultGuideShowPreviousButton.interactable = true;

                defaultGuideImage.sprite = guideToDiplay.guideImage;
                defaultGuideText.text = TextTranslationManager.
                    GetTextFromTextTranslationSOByLanguage(
                    TextTranslationManager.GetCurrentLanguage(),
                    guideToDiplay.guideTextTranslations);

                break;
            case GuideType.TextWithBackground:
                Time.timeScale = 0f;
                textGuideWithBackgroundTransform.gameObject.SetActive(true);

                textGuideWithBackgroundText.text = TextTranslationManager.
                    GetTextFromTextTranslationSOByLanguage(
                    TextTranslationManager.GetCurrentLanguage(),
                    guideToDiplay.guideTextTranslations);

                break;
            case GuideType.TextWithoutBackground:
                Time.timeScale = 1f;
                textGuideWithoutBackgroundTransform.gameObject.SetActive(true);

                textGuideWithoutBackgroundText.text = TextTranslationManager.
                    GetTextFromTextTranslationSOByLanguage(
                    TextTranslationManager.GetCurrentLanguage(),
                    guideToDiplay.guideTextTranslations);

                break;
            case GuideType.RequireButtonPress:
                Time.timeScale = 0f;

                if (guideToDiplay.requiredButtonToPress.Count > 0)
                {
                    requireButtonPressGuideTransform.gameObject.SetActive(true);
                    requireButtonPressGuideText.text = TextTranslationManager.
                        GetTextFromTextTranslationSOByLanguage(
                        TextTranslationManager.GetCurrentLanguage(),
                        guideToDiplay.guideTextTranslations);
                    foreach (Input.Binding binding in guideToDiplay.requiredButtonToPress)
                    {
                        var currentBinding = Instantiate(requireButtonPressGuideRequiredButtonPrefab, requireButtonPressGuideRequiredButtonGrid);
                        currentBinding.gameObject.SetActive(true);
                        currentBinding.GetComponentInChildren<TextMeshProUGUI>().text = Input.Instance.GetBindingText(binding);
                        requiredBindingsToPress.Add(binding);
                    }
                }

                break;
        }
    }
}
