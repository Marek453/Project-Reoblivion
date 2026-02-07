using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace GameCore.UI
{
    public class LanguageSettings : MonoBehaviour
    {
        public TMP_Text text;
        public Slider sliderLang;
        public void SetLanguage(float value)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[(int)value];
            ES3.Save("Lang", (int)value);
            text.text = LocalizationSettings.AvailableLocales.Locales[(int)value].LocaleName;
        }

        public void SetupLanguage()
        {
            sliderLang.maxValue = LocalizationSettings.AvailableLocales.Locales.Count -1;
            if (ES3.KeyExists("Lang"))
            {
                sliderLang.value = ES3.Load<int>("Lang");
                SetLanguage(sliderLang.value);
            }
            else
            {
                sliderLang.value = LocalizationSettings.AvailableLocales.Locales.FindIndex(lang => lang == LocalizationSettings.SelectedLocale);
            }
        }
    }
}

