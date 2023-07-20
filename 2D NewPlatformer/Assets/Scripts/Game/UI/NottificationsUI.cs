using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class NottificationsUI : MonoBehaviour
{
    public static NottificationsUI Instance { get; private set; }

    [SerializeField] private Transform nottificationPrefab;
    [SerializeField] private Transform nottificationsGrid;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        nottificationPrefab.gameObject.SetActive(false);
    }

    public void AddNotification(string notiffication)
    {
        var nottificationTransfrom = Instantiate(nottificationPrefab, nottificationsGrid);
        nottificationTransfrom.gameObject.SetActive(true);
        nottificationTransfrom.GetComponent<TextMeshProUGUI>().text = notiffication;
    }
}
