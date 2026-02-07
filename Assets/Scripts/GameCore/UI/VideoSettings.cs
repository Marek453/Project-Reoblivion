using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore.UI
{

    public class VideoSettings : MonoBehaviour
    {
        [Header("Resolution")]
        public TMP_Text text;
        public TMP_Text textDisplayMode;
        public TMP_Text textVsync;
        public Slider slider;
        public Slider sliderDisplayMode;
        public Slider sliderVsync;

        public int currentResolution;
        public FullScreenMode currentDisplayMode;
        public int currentVsync;

        private string[] typeVsync = { "On", "Off" };

        public void SetResolution(float value)
        {
            text.text = Screen.resolutions[(int)value].ToString().Split("@")[0];
            currentResolution = (int)value;
        }

        public void SetVsync(float value)
        {
            textVsync.text = typeVsync[(int)value];
            currentVsync = (int)value;
        }
        private void UpdateAllSliders()
        {
            slider.onValueChanged.Invoke(slider.value);
            sliderVsync.onValueChanged.Invoke(sliderVsync.value);
            sliderDisplayMode.onValueChanged.Invoke(sliderDisplayMode.value);
        }

        public void SetupVideo()
        {
            if (ES3.KeyExists("Resolution"))
            {
                slider.maxValue = Screen.resolutions.Length - 1;
                slider.value = (float)ES3.Load<int>("Resolution");
                sliderVsync.value = (float)ES3.Load<int>("vsync");
                sliderDisplayMode.value = ES3.Load<int>("fullscreen"); 
                currentDisplayMode = (FullScreenMode)ES3.Load<int>("fullscreen");
                currentResolution = ES3.Load<int>("Resolution");
                QualitySettings.vSyncCount = currentVsync;
            }
            else
            {
                float res = Screen.resolutions.ToList().FindIndex(rs => rs.width == Screen.currentResolution.width && rs.height == Screen.currentResolution.height);
                slider.maxValue = Screen.resolutions.Length - 1;
                slider.value = res;
                sliderVsync.value = QualitySettings.vSyncCount;
                sliderDisplayMode.value = (float)Screen.fullScreenMode;
                SetFullscreen((float)Screen.fullScreenMode);
                SetResolution(res);
            }
            UpdateAllSliders();
        }

        public void SetFullscreen(float value)
        {
            currentDisplayMode = (FullScreenMode)value;
            textDisplayMode.text = currentDisplayMode.ToString();
        }

        public void Apply()
        {
            Resolution resolution = Screen.resolutions[currentResolution];
            Screen.SetResolution(resolution.width, resolution.height, currentDisplayMode);
            QualitySettings.vSyncCount = currentVsync;
            ES3.Save("Resolution", currentResolution);
            ES3.Save("fullscreen", (int)currentDisplayMode);
            ES3.Save("vsync", (int)currentVsync);
        }
    }
}