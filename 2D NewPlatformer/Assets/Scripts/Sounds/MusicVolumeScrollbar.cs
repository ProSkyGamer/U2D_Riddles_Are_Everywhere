using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeScrollbar : MonoBehaviour
{
    private Slider musicVolumeSlider;

    private void Awake()
    {
        musicVolumeSlider = GetComponent<Slider>();
        musicVolumeSlider.onValueChanged.AddListener((float scrollbarValue) =>
        {
            if(scrollbarValue % 0.01 != 0)
                musicVolumeSlider.value -= (float)(scrollbarValue % 0.01);

            SceneMusicController.Instance.ChangeVolume(musicVolumeSlider.value);
        });
    }

    private void Start()
    {
        musicVolumeSlider.value = SceneMusicController.Instance.GetVolumeFromPlayerPrefs();
    }
}
