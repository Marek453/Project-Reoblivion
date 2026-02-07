using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace GameCore.UI
{
    public class AudioSliderUI : MonoBehaviour
    {
        [Header("Audio Settings")]
        public AudioMixer main;
        public string nameSlider;
        public Slider volumeSlider;
        public TMP_Text volumeText;

        public void VolumeText(float volume)
        {
            volumeText.text = volume.ToString("0 db");
        }

        public void SetupVolume()
        {
            if (ES3.KeyExists(nameSlider + "Volume"))
            {
                float volume = ES3.Load<float>(nameSlider + "Volume");
                main.SetFloat(nameSlider, volume);
                volumeSlider.value = volume;
            }
        }


        public void SetVolume(float volume)
        {
            main.SetFloat(nameSlider, volume);
            ES3.Save(nameSlider + "Volume", volume);
        }
    }
}
