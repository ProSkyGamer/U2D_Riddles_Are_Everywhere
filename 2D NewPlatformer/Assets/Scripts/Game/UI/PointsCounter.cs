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
        PointsCollectedController.Instance.OnPointsCollectedChange += PointsCollectedController_OnPointsCollectedChange; ;
    }

    private void PointsCollectedController_OnPointsCollectedChange(object sender, PointsCollectedController.OnPointsCollectedChangeEventArgs e)
    {
        pointsValueText.text = e.currentPoints.ToString() + "/" + e.needePoints.ToString();
    }
}
