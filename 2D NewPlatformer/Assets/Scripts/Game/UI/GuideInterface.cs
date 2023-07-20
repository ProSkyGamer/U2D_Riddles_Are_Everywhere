using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GuideInterface : MonoBehaviour
{
    public static GuideInterface Instance { get; private set; }

    public enum GuideType
    {
        Default,
    }

    [SerializeField] private Button closeGuideButton;

    [SerializeField] private Image guideImage;
    [SerializeField] private TextMeshProUGUI guideText;
    [SerializeField] private Button nextGuide;
    [SerializeField] private Button previousGuide;
    private List<GuidesSO> guidesToShow = new();
    private int currentGuideIndex;

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

        Hide();
    }

    private void Start()
    {
        Input.Instance.OnNextGuideAction += Input_OnNextGuideAction;
        Input.Instance.OnPreviousGuideAction += Input_OnPreviousGuideAction;
    }

    private void Input_OnPreviousGuideAction(object sender, System.EventArgs e)
    {
        if (currentGuideIndex - 1 >= 0)
            ShowPrevGuide();
    }

    private void Input_OnNextGuideAction(object sender, System.EventArgs e)
    {
        if (currentGuideIndex + 1 < guidesToShow.Count)
            ShowNextGuide();
        else
            Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public bool IsShown()
    {
        return gameObject.activeSelf;
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ChangeGuideInterface(GuidesSO guideToSet)
    {
        Show();
        previousGuide.interactable = false;
        previousGuide.onClick.RemoveAllListeners();
        nextGuide.onClick.RemoveAllListeners();

        nextGuide.onClick.AddListener(() =>
        {
            Hide();
        });

        switch (guideToSet.guideType)
        {
            case GuideType.Default:
                guideImage.sprite = guideToSet.guideImage;
                guideText.text = TextTranslationManager.
                    GetTextFromTextTranslationSOByLanguage(
                    TextTranslationManager.GetCurrentLanguage(),
                    guideToSet.guideTextTranslations);
                break;
        }
    }

    public void ChangeGuideInterface(GuidesSO[] guideToSet)
    {
        Show();

        currentGuideIndex = 0;
        previousGuide.interactable = false;
        previousGuide.onClick.RemoveAllListeners();
        nextGuide.onClick.RemoveAllListeners();

        if (guideToSet.Length > 1)
        {
            nextGuide.onClick.AddListener(() =>
            {
                ShowNextGuide();
            });
        }
        else
        {
            nextGuide.onClick.AddListener(() =>
            {
                Hide();
            });
        }

        switch (guideToSet[0].guideType)
        {
            case GuideType.Default:
                guideImage.sprite = guideToSet[0].guideImage;
                guideText.text = TextTranslationManager.
                    GetTextFromTextTranslationSOByLanguage(
                    TextTranslationManager.GetCurrentLanguage(),
                    guideToSet[0].guideTextTranslations);
                break;
        }
        guidesToShow.Clear();
        guidesToShow.AddRange(guideToSet);
    }

    private void ShowNextGuide()
    {
        currentGuideIndex++;

        switch (guidesToShow[currentGuideIndex].guideType)
        {
            case GuideType.Default:
                guideImage.sprite = guidesToShow[currentGuideIndex].guideImage;
                guideText.text = TextTranslationManager.
                    GetTextFromTextTranslationSOByLanguage(
                    TextTranslationManager.GetCurrentLanguage(),
                    guidesToShow[currentGuideIndex].guideTextTranslations);
                break;
        }

        if (guidesToShow.Count > currentGuideIndex + 1)
        {
            previousGuide.onClick.RemoveAllListeners();
            nextGuide.onClick.RemoveAllListeners();

            previousGuide.interactable = true;
            previousGuide.onClick.AddListener(() =>
            {
                ShowPrevGuide();
            });
            nextGuide.onClick.AddListener(() =>
            {
                ShowNextGuide();
            });
        }
        else
        {
            previousGuide.onClick.RemoveAllListeners();
            nextGuide.onClick.RemoveAllListeners();
            previousGuide.interactable = true;
            previousGuide.onClick.AddListener(() =>
            {
                ShowPrevGuide();
            });
            nextGuide.onClick.AddListener(() =>
            {
                Hide();
            });
        }
    }

    private void ShowPrevGuide()
    {
        currentGuideIndex--;
        switch (guidesToShow[0].guideType)
        {
            case GuideType.Default:
                guideImage.sprite = guidesToShow[0].guideImage;
                guideText.text = TextTranslationManager.
                    GetTextFromTextTranslationSOByLanguage(
                    TextTranslationManager.GetCurrentLanguage(),
                    guidesToShow[0].guideTextTranslations);
                break;
        }

        if (currentGuideIndex < 1)
        {
            previousGuide.onClick.RemoveAllListeners();
            nextGuide.onClick.RemoveAllListeners();
            previousGuide.interactable = false;
            nextGuide.onClick.AddListener(() =>
            {
                ShowNextGuide();
            });
        }
    }
}
