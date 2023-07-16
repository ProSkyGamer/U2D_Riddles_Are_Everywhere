using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectedPlayerCard : MonoBehaviour
{
    [SerializeField] private Image selectedPlayerImage;
    [SerializeField] private TextMeshProUGUI selectedPlayerDescriptionText;
    [SerializeField] private Button chooseSelectedPlayerButton;
    private PlayerSO currentSelectedPlayerCard = null;

    private void Awake()
    {
        chooseSelectedPlayerButton.onClick.AddListener(() =>
        {
            PlayerChangeController.Instance.ChangePlayer(currentSelectedPlayerCard);
        });
    }

    public void TryChangeSelectedPlayerCard(PlayerSO player)
    {
        if(currentSelectedPlayerCard != player)
        {
            currentSelectedPlayerCard = player;

            selectedPlayerImage.sprite = player.playerSprite;
            selectedPlayerDescriptionText.text = player.playerDescriptionText;

            //Блокировка кнопки выбора персонажа (если не куплен/уже выбран)
            if(PlayerChangeController.Instance.GetCurrentPlayerSO() != player)
            {
                if (PlayerChangeController.Instance.IsChangeRecharged())
                    chooseSelectedPlayerButton.interactable = true;
                else
                {
                    chooseSelectedPlayerButton.interactable = false;
                    PlayerChangeController.Instance.OnPLayerRechargeDone += PlayerChangeController_OnPLayerRechargeDone;
                }
            }
            else
            {
                chooseSelectedPlayerButton.interactable = false;
            }
        }
    }

    private void PlayerChangeController_OnPLayerRechargeDone(object sender, System.EventArgs e)
    {
        PlayerChangeController playerChangeController = (PlayerChangeController)sender;

        chooseSelectedPlayerButton.interactable = true;

        playerChangeController.OnPLayerRechargeDone -= PlayerChangeController_OnPLayerRechargeDone;
    }
}
