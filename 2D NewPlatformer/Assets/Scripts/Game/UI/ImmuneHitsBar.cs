using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ImmuneHitsBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI immnuneHitsText;

    private void Start()
    {
        PlayerController.OnPlayerImmuneHit += PlayerController_OnPlayerImmuneHit;

        PlayerChangeController.Instance.OnPlayerChange += PlayerChangeController_OnPlayerChange;

        ChangeImmuneHits(PlayerChangeController.Instance.GetCurrentPlayerController().GetImmuneHits());
    }

    private void PlayerChangeController_OnPlayerChange(object sender, System.EventArgs e)
    {
        ChangeImmuneHits(PlayerChangeController.Instance.GetCurrentPlayerController().GetImmuneHits());
    }

    private void PlayerController_OnPlayerImmuneHit(object sender, PlayerController.OnPlayerImmnuneHitEventArgs e)
    {
        ChangeImmuneHits(e.currentImmuneHits);
    }

    public void ChangeImmuneHits(int immuneHits)
    {
        if (immuneHits > 0)
        {
            Show();
            immnuneHitsText.text = immuneHits.ToString();
        }
        else
            Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
