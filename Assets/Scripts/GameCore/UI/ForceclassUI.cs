using TMPro;
using UnityEngine;

namespace GameCore.UI
{
    public class ForceclassUI : MonoBehaviour
    {
        public Animator forceAnimator;
        public TMP_Text nameClass;


        public void Forceclass(string roleName, Color color)
        {
            nameClass.text = roleName;
            nameClass.color = color;
            forceAnimator.Play("Start");
        }

    }
}
