using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovableObjectsTimer : MonoBehaviour
{
    [SerializeField] private Image timerImage;

    private float previousValue;
    private float currentValue;

    private void Start()
    {
        MovableHead.OnTimerChange += MovableHead_OnTimerChange;
        BreakableBlock.OnTimerChange += BreakableBlock_OnTimerChange;

        Hide();
    }

    private void BreakableBlock_OnTimerChange(object sender, BreakableBlock.OnTimerChangeEventArgs e)
    {
        ChangeTimer(e.currentTime, e.maxTime);
    }

    private void MovableHead_OnTimerChange(object sender, MovableHead.OnTimerChangeEventArgs e)
    {
        ChangeTimer(e.currentTime, e.maxTime);
    }

    private void ChangeTimer(float currentTime, float maxTime)
    {
        currentValue = currentTime;

        if (!IsShown())
            Show();

        if (currentTime >= maxTime)
            Hide();
        else
        {
            timerImage.fillAmount = currentTime / maxTime;
        }
    }

    private void LateUpdate()
    {
        if (currentValue == previousValue)
            Hide();
        else
            previousValue = currentValue;
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private bool IsShown()
    {
        return gameObject.activeSelf;
    }
}
