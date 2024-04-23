using System;
using System.Collections.Generic;
using UnityEngine;

public class SFXVolume : MonoBehaviour
{
    private readonly List<AudioSource> audioSources = new();

    private void Awake()
    {
        var currentAudioSource = GetComponents<AudioSource>();

        audioSources.AddRange(currentAudioSource);
    }

    private void Start()
    {
        SFXVolumeSceneController.Instance.OnSFXVolumeChanged += SFXSceneVolumeController_OnSFXVolumeChanged;
    }

    private void SFXSceneVolumeController_OnSFXVolumeChanged(object sender, EventArgs e)
    {
        var audioVolume = SFXVolumeSceneController.Instance.GetVolumeFromPlayerPrefs();
        foreach (var audioSource in audioSources) audioSource.volume = audioVolume;
    }

    private void OnDestroy()
    {
        SFXVolumeSceneController.Instance.OnSFXVolumeChanged -= SFXSceneVolumeController_OnSFXVolumeChanged;
    }
}
