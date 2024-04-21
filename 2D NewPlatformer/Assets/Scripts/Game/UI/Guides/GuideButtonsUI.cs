using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GuideButtonsUI : MonoBehaviour
{
    public static GuideButtonsUI Instance { get; private set; }

    private List<Button> allGuideButtonsList = new();
    private List<GuideButtonSO> allIGuideButtonsSOList = new();
    [SerializeField] private Transform guideButtonPrefab;
    [SerializeField] private Transform buttonsLayoutGroup;

    private void Awake()
    {
        if (Instance != null)
            Destroy(this.gameObject);
        else
            Instance = this;

        guideButtonPrefab.gameObject.SetActive(false);
    }

    public void AddGuideButtonToScreen(GuideButtonSO guideButtonSO)
    {
        Transform guideButtonTransform = Instantiate(guideButtonPrefab, buttonsLayoutGroup);
        guideButtonTransform.gameObject.SetActive(true);
        Button guideButton = guideButtonTransform.GetComponent<Button>();
        guideButtonTransform.GetComponentsInChildren<Image>()[1].sprite = guideButtonSO.objectSprite;

        guideButton.onClick.AddListener(() =>
        {
            GuideInterface.Instance.ShowGuide(guideButtonSO.guideSO);
        });

        allGuideButtonsList.Add(guideButton);
        allIGuideButtonsSOList.Add(guideButtonSO);
    }

    public void RemoveGuideButtonFromScreen(GuideButtonSO guideButtonSO)
    {
        for (int i = 0; i < allIGuideButtonsSOList.Count; i++)
        {
            if (allIGuideButtonsSOList[i] == guideButtonSO)
            {
                if(!allGuideButtonsList[i].IsDestroyed())
                    Destroy(allGuideButtonsList[i].gameObject);

                allGuideButtonsList.RemoveAt(i);
                allIGuideButtonsSOList.RemoveAt(i);
                break;
            }
        }
    }

    public bool IsCurrentGuideCreated(GuideButtonSO guideButton)
    {
        return allIGuideButtonsSOList.Contains(guideButton);
    }
}
