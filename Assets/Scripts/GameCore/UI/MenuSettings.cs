using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace GameCore.UI
{
    public class MenuSettings : MonoBehaviour
    {
        public List<AudioSliderUI> AudioSliderUI;
        public VideoSettings videoSettings;
        public LanguageSettings languageSettings;

        private void Start()
        {
            SetupVolume();
            languageSettings.SetupLanguage();
        }

        private void SetupVolume()
        {
            videoSettings.SetupVideo();
            foreach (AudioSliderUI audioSlider in AudioSliderUI)
            {
                audioSlider.SetupVolume();
            }
        }
    }
}
