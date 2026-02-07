using UnityEngine;
using UnityEngine.UI;

namespace GameCore.UI
{
    public class DobbleSlider : MonoBehaviour
    {
        public Image slider1, slider2;

        public float value;

        private void Update()
        {
            float v1 = slider1.fillAmount;
            float v2 = slider2.fillAmount;

            v1 = value;
            v2 = value;
            slider2.fillAmount = v2;
            slider1.fillAmount = v1;

        }
    }
}
