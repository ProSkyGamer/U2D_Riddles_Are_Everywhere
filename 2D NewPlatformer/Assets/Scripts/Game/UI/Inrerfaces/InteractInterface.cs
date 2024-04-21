using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractInterface : MonoBehaviour
{
    public static InteractInterface Instance { get; private set; }
    
    private List<Button> allInteractButtonsList = new List<Button>();
    private List<AddInteractButtonUI> allInteractableItemButtonsList = new List<AddInteractButtonUI>();
    [SerializeField] private Transform interactButtonPrefab;
    [SerializeField] private Transform buttonsLayoutGroup;
    private Button activeButton;
    private float timeBetweenScrolls = 0.1f;
    float timerBetweenScrolls = 0f;

    private void Awake()
    {
        if (Instance != null)
            Destroy(this.gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        Hide();
    }

    public void Update()
    {
        if (allInteractButtonsList.Count > 0)
        {
            if (EventSystem.current.currentSelectedGameObject != activeButton.gameObject)
            {
                activeButton.Select();
            }

            if (allInteractButtonsList.Count > 1)
            {
                if (timerBetweenScrolls <= 0)
                {
                    if (Input.Instance.IsScrollingMouse(out bool isScrollUp))
                    {
                        for (int i = 0; i < allInteractButtonsList.Count; i++)
                        {
                            if (activeButton == allInteractButtonsList[i])
                            {
                                if (isScrollUp)
                                {
                                    if (i != allInteractButtonsList.Count - 1)
                                        activeButton = allInteractButtonsList[i + 1];
                                    else
                                        activeButton = allInteractButtonsList[0];
                                }
                                else
                                {
                                    if (i != 0)
                                        activeButton = allInteractButtonsList[i - 1];
                                    else
                                        activeButton = allInteractButtonsList[allInteractButtonsList.Count - 1];
                                }
                                timerBetweenScrolls = timeBetweenScrolls;
                                break;
                            }
                        }
                    }
                }
                else
                    timerBetweenScrolls -= Time.deltaTime;
            }
        }
    }

    public void AddButtonInteractToScreen(AddInteractButtonUI interactButtonUI, TextTranslationsSO textTranslationsSO)
    {
        if (allInteractButtonsList.Count == 0)
            Show();

        Transform interactableItemButtonTransform = Instantiate(interactButtonPrefab, buttonsLayoutGroup);
        Button interactableItemButton = interactableItemButtonTransform.GetComponent<Button>();
        interactableItemButtonTransform.GetComponentInChildren<TextTranslationUI>().ChangeTextTranslationSO(textTranslationsSO);
        interactableItemButtonTransform.GetComponentsInChildren<TextMeshProUGUI>()[0].text =
            TextTranslationManager.GetTextFromTextTranslationSOByLanguage(
                TextTranslationManager.GetCurrentLanguage(), textTranslationsSO);
        interactableItemButtonTransform.GetComponentsInChildren<TextMeshProUGUI>()[1].text =
            Input.Instance.GetBindingText(Input.Binding.Interact);

        interactableItemButton.onClick.AddListener(() =>
        {
            interactButtonUI.OnInteract();
        });

        if (activeButton == null)
            activeButton = interactableItemButton;

        allInteractButtonsList.Add(interactableItemButton);
        allInteractableItemButtonsList.Add(interactButtonUI);

        if(allInteractButtonsList.Count == 1)
            Input.Instance.OnInteractAction += GameInput_OnInteractAction;
    }
    
    public void RemoveButtonInteractToScreen(AddInteractButtonUI interactButtonUI)
    {
        for(int i = 0; i < allInteractableItemButtonsList.Count; i++)
        {
            if (allInteractableItemButtonsList[i] == interactButtonUI)
            {
                if(activeButton == allInteractButtonsList[i])
                {
                    if (allInteractButtonsList.Count != 1)
                        if (i != 0)
                            activeButton = allInteractButtonsList[i - 1];
                        else
                            activeButton = allInteractButtonsList[i + 1];
                }

                Destroy(allInteractButtonsList[i].gameObject);
                allInteractButtonsList.RemoveAt(i);

                allInteractableItemButtonsList.RemoveAt(i);

                if (allInteractButtonsList.Count == 0)
                {
                    activeButton = null;
                    Input.Instance.OnInteractAction -= GameInput_OnInteractAction;
                    Hide();
                }

                break;
            }
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        for(int i = 0; i < allInteractButtonsList.Count; i++)
        {
            if (allInteractButtonsList[i].gameObject == EventSystemClass.Instance.EventSystem.currentSelectedGameObject)
            {
                allInteractableItemButtonsList[i].OnInteract();
            }
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);

        Input.Instance.OnInteractAction -= GameInput_OnInteractAction;
    }

    public bool IsShow()
    {
        return gameObject.activeSelf;
    }
}
