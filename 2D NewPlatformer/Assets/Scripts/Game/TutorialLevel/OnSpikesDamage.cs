using System;
using UnityEngine;
using UnityEngine.UI;

public class OnSpikesDamage : MonoBehaviour
{
    public event EventHandler OnButtonPressed;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        PlayerChangeController.Instance.GetCurrentPlayerController().TeleportToCurrentCheckpoint();
        TransitionsInterface.Instance.OnTransitionFinished += TransitionsInterface_OnTransitionFinished;
        PlayerChangeController.Instance.GetCurrentPlayerController().RegenerateHearts(2);
        PlayerChangeController.Instance.GetCurrentPlayerController().SetImmuneHits(1);

        GuideInterface.Instance.Hide();

        OnButtonPressed?.Invoke(this, EventArgs.Empty);
    }

    private void TransitionsInterface_OnTransitionFinished(object sender, EventArgs e)
    {
        TransitionsInterface.Instance.OnTransitionFinished -= TransitionsInterface_OnTransitionFinished;
        PlayerChangeController.Instance.GetCurrentPlayerController().SetImmuneHits(0);
    }
}
