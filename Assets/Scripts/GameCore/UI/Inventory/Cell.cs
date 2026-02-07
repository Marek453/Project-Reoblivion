using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameCore.UI.Inventory
{
    public class Cell : MonoBehaviour
    {
        public bool isSelected = false;
        public Image image;
        public Image itemCell;
        public Button button;

        private void Start()
        {
            image.alphaHitTestMinimumThreshold = 0.1f;
        }

        public void Setup(Color color)
        {
            isSelected = false;
            ColorBlock cb = button.colors;
            image.color = color;
            color.a = 0.30f;
            cb.highlightedColor = color;
            color.r = 0.7f;
            cb.pressedColor = color;
            button.colors = cb;
        }

    }
}
