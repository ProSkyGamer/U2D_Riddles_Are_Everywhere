using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidesReset : MonoBehaviour
{
    public static GuidesReset Instance { get; private set; }

    [SerializeField] private AllGuidesSO allGuidesSO;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void ResetAllGuides()
    {
        foreach(GuidesSO guide in allGuidesSO.allGuides)
        {
            PlayerPrefs.SetInt(GetGuidePlayerPrefsString(guide.name), 0);
        }
    }

    private string GetGuidePlayerPrefsString(string guideName)
    {
        return guideName + "PlayerPrefs";
    }
}
