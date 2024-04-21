using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMusicController : MonoBehaviour
{
    public static SceneMusicController Instance { get; private set; }

    private const string MUSIC_VOLUME_PLAYER_PREFS = "MusicVolumePlayerPrefs";

    [SerializeField] private AudioSource musicSource;

    private void Awake()
    {
        if (Instance != null)
            Destroy(this.gameObject);
        else 
            Instance = this;

        ChangeVolume(GetVolumeFromPlayerPrefs());
    }

    public void ChangeVolume(float volume)
    {
        PlayerPrefs.SetFloat(MUSIC_VOLUME_PLAYER_PREFS, volume);
        musicSource.volume = volume;
    }

    public float GetVolumeFromPlayerPrefs()
    {
        return PlayerPrefs.GetFloat(MUSIC_VOLUME_PLAYER_PREFS, 0.5f);
    }
}
