using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Transform heartsGrid;
    [SerializeField] private Transform fullHealthPrefab;
    [SerializeField] private Transform emptyHealthPrefab;
    private List<Transform> healthList = new List<Transform>();

    private void Start()
    {
        PlayerController.OnPlayerHealthChange += PlayerController_OnPlayerHealthChange;
        PlayerChangeController.Instance.OnPlayerChange += PlayerChangeController_OnPlayerChange;

        fullHealthPrefab.gameObject.SetActive(false);
        emptyHealthPrefab.gameObject.SetActive(false);

        ChangeHealth(PlayerChangeController.Instance.GetCurrentPlayerController().GetCurrentHearts(),
            PlayerChangeController.Instance.GetCurrentPlayerController().GetMaxHearts() -
            PlayerChangeController.Instance.GetCurrentPlayerController().GetCurrentHearts());
    }

    private void PlayerChangeController_OnPlayerChange(object sender, System.EventArgs e)
    {
        ChangeHealth(PlayerChangeController.Instance.GetCurrentPlayerController().GetCurrentHearts(),
            PlayerChangeController.Instance.GetCurrentPlayerController().GetMaxHearts() -
            PlayerChangeController.Instance.GetCurrentPlayerController().GetCurrentHearts());
    }

    private void PlayerController_OnPlayerHealthChange(object sender, PlayerController.OnPlayerHealthChangeEventArgs e)
    {
        ChangeHealth(e.currentHealth, e.maxHealth - e.currentHealth);
    }

    public void ChangeHealth(int fullHearts, int emptyHearts)
    {
        foreach(Transform heart in healthList)
        {
            Destroy(heart.gameObject);
        }
        healthList.Clear();

        for(int i = 0; i < fullHearts; i++)
        {
            Transform fullHeart = Instantiate(fullHealthPrefab, heartsGrid);
            fullHeart.gameObject.SetActive(true);
            healthList.Add(fullHeart);
        }

        for (int i = 0; i < emptyHearts; i++)
        {
            Transform emptyHeart = Instantiate(emptyHealthPrefab, heartsGrid);
            emptyHeart.gameObject.SetActive(true);
            healthList.Add(emptyHeart);
        }
    }
}
