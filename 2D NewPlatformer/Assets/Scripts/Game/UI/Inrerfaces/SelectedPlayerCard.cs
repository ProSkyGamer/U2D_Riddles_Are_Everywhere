using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectedPlayerCard : MonoBehaviour
{
    [SerializeField] private Image selectedPlayerImage;
    [SerializeField] private TextMeshProUGUI selectedPlayerDescriptionText;
    private TextTranslationUI selectedPlayerDescriptionTranslationText;
    [SerializeField] private Button chooseSelectedPlayerButton;
    private PlayerSO currentSelectedPlayerCard = null;

    private void Awake()
    {
        chooseSelectedPlayerButton.onClick.AddListener(() =>
        {
            PlayerChangeController.Instance.ChangePlayer(currentSelectedPlayerCard);
        });

        selectedPlayerDescriptionTranslationText = selectedPlayerDescriptionText.gameObject.GetComponent<TextTranslationUI>();
    }

    public void TryChangeSelectedPlayerCard(PlayerSO player)
    {
        if(currentSelectedPlayerCard != player)
        {
            currentSelectedPlayerCard = player;

            selectedPlayerImage.sprite = player.playerSprite;
            selectedPlayerDescriptionText.text = TextTranslationManager.GetTextFromTextTranslationSOByLanguage(
                TextTranslationManager.GetCurrentLanguage(), player.playerDescriptionText);
            selectedPlayerDescriptionTranslationText.ChangeTextTranslationSO(player.playerDescriptionText);

            if(PlayerChangeController.Instance.GetCurrentPlayerSO() != player && (
                PlayerChangeController.Instance.GetAlwaysAvailiblePlayers().Contains(player) ||
                ShopManager.IsCurrentPlayerBought(player)))
            {
                if (PlayerChangeController.Instance.IsCanChangePlayer())
                    chooseSelectedPlayerButton.interactable = true;
                else
                    chooseSelectedPlayerButton.interactable = false;
            }
            else
                chooseSelectedPlayerButton.interactable = false;
        }
    }
}
