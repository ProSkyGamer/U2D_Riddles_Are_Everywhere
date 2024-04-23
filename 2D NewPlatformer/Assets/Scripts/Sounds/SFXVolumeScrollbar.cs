using UnityEngine;
using UnityEngine.UI;

public class SFXVolumeScrollbar : MonoBehaviour
{
    private Slider sfxVolumeSlider;

    private bool isFirstUpdate = true;

    private void Awake()
    {
        sfxVolumeSlider = GetComponent<Slider>();
        sfxVolumeSlider.onValueChanged.AddListener(scrollbarValue =>
        {
            if (scrollbarValue % 0.01 != 0)
                sfxVolumeSlider.value -= (float)(scrollbarValue % 0.01);

            SFXVolumeSceneController.Instance.ChangeVolume(sfxVolumeSlider.value);
        });
    }

    private void Update()
    {
        if (isFirstUpdate)
        {
            isFirstUpdate = false;
            sfxVolumeSlider.value = SFXVolumeSceneController.Instance.GetVolumeFromPlayerPrefs();
        }
    }
}
