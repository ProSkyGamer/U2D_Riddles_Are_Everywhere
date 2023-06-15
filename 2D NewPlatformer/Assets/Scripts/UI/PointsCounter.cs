using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointsCounter : MonoBehaviour
{
    private TextMeshProUGUI pointsValueText;

    private void Awake()
    {
        pointsValueText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        PlayerController.OnPointsCollectedChange += PlayerController_OnPointsCollectedChange;
    }

    private void PlayerController_OnPointsCollectedChange(object sender, PlayerController.OnPointsCollectedChangeEventArgs e)
    {
        pointsValueText.text = e.currentPoints.ToString();
    }
}
