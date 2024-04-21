using System;
using UnityEngine;
using UnityEngine.UI;

public class TransitionsInterface : MonoBehaviour
{
    public static TransitionsInterface Instance { get; private set; }

    public event EventHandler OnTransitionFinished;

    [SerializeField] private Image defaultTransitionImage;

    private Action ActionToMake;
    private readonly float transitonTime = 1f;
    private float transtionTimer;
    private bool isShowing;
    private TransitionsTypes type;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        Hide();
    }

    public enum TransitionsTypes
    {
        Default
    }

    public void MakeTransition(TransitionsTypes type, Action actionToMake)
    {
        Show();
        ActionToMake = actionToMake;
        transtionTimer = transitonTime;
        this.type = type;
        isShowing = true;
    }

    private void Update()
    {
        if (transtionTimer > 0f)
        {
            transtionTimer -= Time.deltaTime;
            float imageA;
            if (isShowing)
                imageA = (transitonTime - transtionTimer) / transitonTime;
            else
                imageA = transtionTimer / transitonTime;

            switch (type)
            {
                case TransitionsTypes.Default:
                    defaultTransitionImage.color = new Color(defaultTransitionImage.color.r,
                        defaultTransitionImage.color.g, defaultTransitionImage.color.b, imageA);
                    break;
            }
        }
        else
        {
            if (ActionToMake != null)
            {
                ActionToMake();
                transtionTimer = transitonTime;
                isShowing = false;
                ActionToMake = null;
            }
            else
            {
                if (IsShown())
                {
                    Hide();
                    OnTransitionFinished?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }

    private bool IsShown()
    {
        return gameObject.activeSelf;
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
