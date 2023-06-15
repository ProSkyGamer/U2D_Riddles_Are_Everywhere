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
    private List<InteractableItem> allInteractableItemButtonsList = new List<InteractableItem>();
    [SerializeField] private Transform interactButtonPrefab;
    [SerializeField] private Transform buttonsLayoutGroup;

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

    public void AddButtonInteractToScreen(InteractableItem interactableItem, string buttonText)
    {
        if (allInteractButtonsList.Count == 0)
            Show();

        Transform interactableItemButtonTransform = Instantiate(interactButtonPrefab, buttonsLayoutGroup);
        Button interactableItemButton = interactableItemButtonTransform.GetComponent<Button>();
        interactableItemButtonTransform.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;

        interactableItemButton.onClick.AddListener(() =>
        {
            interactableItem.OnInteract();

            RemoveButtonInteractToScreen(interactableItem);
        });

        interactableItemButton.Select();

        allInteractButtonsList.Add(interactableItemButton);
        allInteractableItemButtonsList.Add(interactableItem);

        if(allInteractButtonsList.Count == 1)
            Input.Instance.OnInteractAction += GameInput_OnInteractAction;
    }
    
    public void RemoveButtonInteractToScreen(InteractableItem interactableItem)
    {
        for(int i = 0; i < allInteractableItemButtonsList.Count; i++)
        {
            if (allInteractableItemButtonsList[i] == interactableItem)
            {
                Destroy(allInteractButtonsList[i].gameObject);
                allInteractButtonsList.RemoveAt(i);

                allInteractableItemButtonsList.RemoveAt(i);

                if (allInteractButtonsList.Count == 0)
                {
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
                RemoveButtonInteractToScreen(allInteractableItemButtonsList[i]);
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