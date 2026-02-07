using System;
using System.Collections;
using System.Collections.Generic;
using GameCore.Audio.Music;
using NetCore;
using TMPro;
using UnityEngine;

namespace GameCore.UI
{
    public class MenuUI : MonoBehaviour
    {
        public List<MenuElement> menuElements;
        public TMP_Text mainName;
        public string currentElement;
        public Animator creditsAnimator;
        public Animator connectAnimator;

        public void SetElement(string nameElement)
        {
            MenuElement menuElement = menuElements.Find(element => element.nameElement == nameElement);
            for (int i = 0; i < menuElements.Count; i++)
            {
                menuElements[i].element.SetActive(false);
            }
            if (nameElement == String.Empty)
            {
                currentElement = String.Empty;
                return;
            }
            menuElement.element.SetActive(true);
            currentElement = menuElement.nameElement;
        }

        public void DisableAnim()
        {
            connectAnimator.enabled = false;
        }

        public void Connect()
        {
            connectAnimator.enabled = true;
            connectAnimator.Play("Connect");
        }

        public void Disconnect()
        {
            connectAnimator.SetTrigger("Diss");
            connectAnimator.Play("Disconnect");
        }

        public void Credits()
        {
            MusicManager.Instance.PlayMusic(MusicEventType.Credits, false, false, 0, 1, true);
            creditsAnimator.Play("Start");
        }

        public void SetElement(int id)
        {
            MenuElement menuElement = menuElements[id];
            for (int i = 0; i < menuElements.Count; i++)
            {
                menuElements[i].element.SetActive(false);
            }
            menuElement.element.SetActive(true);
            currentElement = menuElement.nameElement;
        }
    }
}
