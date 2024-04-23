using System;
using UnityEngine;

public class SFXVolumeSceneController : MonoBehaviour
{
    public event EventHandler OnSFXVolumeChanged;

    public static SFXVolumeSceneController Instance { get; private set; }

    private const string SFX_VOLUME_PLAYER_PREFS = "SFXVolumePlayerPrefs";

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        ChangeVolume(GetVolumeFromPlayerPrefs());
    }

    public void ChangeVolume(float volume)
    {
        PlayerPrefs.SetFloat(SFX_VOLUME_PLAYER_PREFS, volume);

        OnSFXVolumeChanged?.Invoke(this, EventArgs.Empty);
    }

    public float GetVolumeFromPlayerPrefs()
    {
        return PlayerPrefs.GetFloat(SFX_VOLUME_PLAYER_PREFS, 0.5f);
    }
}
