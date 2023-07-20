using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMainMenuInterface : MonoBehaviour
{
    public static SettingsMainMenuInterface Instance { get; private set; }

    [SerializeField] private Button returnToMainMenuButton;
    [SerializeField] private Transform pressToRebindKeyInterface;

    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button jumpButton;
    [SerializeField] private Button sprintButton;
    [SerializeField] private Button returnToCheckpointButton;
    [SerializeField] private Button changePlayerButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button pauseButton;

    [SerializeField] private TextMeshProUGUI moveLeftButtonBindingText;
    [SerializeField] private TextMeshProUGUI moveRightButtonBindingText;
    [SerializeField] private TextMeshProUGUI jumpButtonBindingText;
    [SerializeField] private TextMeshProUGUI sprintButtonBindingText;
    [SerializeField] private TextMeshProUGUI returnToCheckpointButtonBindingText;
    [SerializeField] private TextMeshProUGUI changePlayerButtonBindingText;
    [SerializeField] private TextMeshProUGUI interactButtonBindingText;
    [SerializeField] private TextMeshProUGUI pauseButtonBindingText;

    private bool isFirstUpdate = true;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        returnToMainMenuButton.onClick.AddListener(() =>
        {
            MainMenuInterface.Instance.Show();
            Hide();
        });

        moveLeftButton.onClick.AddListener(() => { RebingBinding(Input.Binding.MoveLeft); });
        moveRightButton.onClick.AddListener(() => { RebingBinding(Input.Binding.MoveRight); });
        jumpButton.onClick.AddListener(() => { RebingBinding(Input.Binding.Jump); });
        sprintButton.onClick.AddListener(() => { RebingBinding(Input.Binding.Sprint); });
        returnToCheckpointButton.onClick.AddListener(() => { RebingBinding(Input.Binding.ReturnToCheckpoint); });
        changePlayerButton.onClick.AddListener(() => { RebingBinding(Input.Binding.ChangePlayer); });
        interactButton.onClick.AddListener(() => { RebingBinding(Input.Binding.Interact); });
        pauseButton.onClick.AddListener(() => { RebingBinding(Input.Binding.Pause); });

        HidePressToRebingKey();
    }

    private void Start()
    {
        UpdateVisual();
    }

    private void Update()
    {
        if (isFirstUpdate)
        {
            isFirstUpdate = false;
            HidePressToRebingKey();
            Hide();
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public bool IsShown()
    {
        return gameObject.activeSelf;
    }

    private void RebingBinding(Input.Binding binding)
    {
        ShowPressToRebingKey();

        Input.Instance.RebingBinding(binding, () =>
        {
            HidePressToRebingKey();
            UpdateVisual();
        });
    }

    private void UpdateVisual()
    {
        moveLeftButtonBindingText.text = Input.Instance.GetBindingText(Input.Binding.MoveLeft);
        moveRightButtonBindingText.text = Input.Instance.GetBindingText(Input.Binding.MoveRight);
        jumpButtonBindingText.text = Input.Instance.GetBindingText(Input.Binding.Jump);
        sprintButtonBindingText.text = Input.Instance.GetBindingText(Input.Binding.Sprint);
        returnToCheckpointButtonBindingText.text = Input.Instance.GetBindingText(Input.Binding.ReturnToCheckpoint);
        changePlayerButtonBindingText.text = Input.Instance.GetBindingText(Input.Binding.ChangePlayer);
        interactButtonBindingText.text = Input.Instance.GetBindingText(Input.Binding.Interact);
        pauseButtonBindingText.text = Input.Instance.GetBindingText(Input.Binding.Pause);
    }

    private void ShowPressToRebingKey()
    {
        pressToRebindKeyInterface.gameObject.SetActive(true);
    }

    private void HidePressToRebingKey()
    {
        pressToRebindKeyInterface.gameObject.SetActive(false);
    }
}
