using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PrimeTween;

namespace GameCore.UI
{
    public class PlayerHeathSlider : MonoBehaviour
    {
        public TMP_Text text;
        public Slider slider;

        public Image backround, front;

        public void SetupHeath(float maxHeath, float minHeath, Color colorSlider)
        {
            slider.maxValue = maxHeath;
            slider.minValue = minHeath;
            slider.value = maxHeath;
            backround.color = colorSlider;
            text.color = colorSlider;
            colorSlider.a = 0.35f;
            front.color = colorSlider;
        }

        public void ChangeHp(float heath)
        {
            Tween.Custom(slider.value, heath,1, v => slider.value = v);
        }

        public void OnSliderValue(float value)
        {
            text.text = value.ToString("0 HP");
        }
    }
}