using UnityEngine;
using GameCore.Interfaces;
using GameCore.Player;
using TMPro;
using System;
using System.Collections;
using NetCore;
using UnityEngine.Localization.Components;

namespace GameCore.Facility
{
    public class FacilityTouchButton : MonoBehaviour,IInteractable
    {
        public enum StatusType
        {
            None = -1,
            Open,
            Close,
            Warning,
            Locked,
            Dinate,
            ReactorLock,
            Offline,
            OpenKey,
            CloseKey
        }
        [Serializable]
        public class ButtonStatusType
        {
            public string indecatorText;
            public StatusType type;
            public Material baseMaterial;
            public Material indecatorMaterial;
        }
        public ButtonStatusType[] buttonStatusTypes;
        public Renderer indecator;
        public Renderer baseButton;
        public TMP_Text indecatorText;
        public StatusType currentStatus;
        public AudioSource interactSource;
        public AudioSource errorSource;
        public GameObject animator;
        public LocalizeStringEvent localizeStringEvent;
        public float offset = 0.1f;
        public string currentTextIndecator;
        private bool _adjusted;

        public ButtonStatusType GetButtonStatus(StatusType statusType)
        {
            for (int i = 0; i < buttonStatusTypes.Length; i++)
            {
                if (buttonStatusTypes[i].type == statusType)
                {
                    return buttonStatusTypes[i];
                }

            }
            return null;
        }

        public void Adjust()
        {
            if (!_adjusted)
            {
                _adjusted = true;
                base.transform.position += -base.transform.up;
                RaycastHit hitInfo;
                if (Physics.Raycast(new Ray(base.transform.position, base.transform.up), out hitInfo, 2.5f))
                {
                    base.transform.position = hitInfo.point;
                    base.transform.position -= offset * 0.1f * base.transform.up;
                }
            }
        }

        public void Interact(PlayerManager player)
        {
            if(currentStatus == StatusType.Offline) return;
            interactSource.PlayOneShot(interactSource.clip);
            if(currentStatus == StatusType.Locked) Error();
            ObjectInteract(player);
        }

        public void Error()
        {
            errorSource.PlayOneShot(errorSource.clip);
        }

        private void ObjectInteract(PlayerManager player)
        {
            IInteractable interactableObject = (IInteractable)transform.parent.GetComponent(typeof(IInteractable));
            if (interactableObject != null)
            {
                interactableObject.Interact(player);
            }
        }

        public void SetButtonStatus(StatusType statusType)
        {
            if (statusType == currentStatus) return;
            currentStatus = statusType;
            ButtonStatusType buttonStatus = GetButtonStatus(statusType);
            Material[] mats = baseButton.materials;
            if (buttonStatus != null)
            {
                if (statusType == StatusType.Offline)
                {
                    indecator.enabled = false;
                    animator.SetActive(false);
                    mats[3] = buttonStatus.baseMaterial;
                    indecatorText.text = buttonStatus.indecatorText;
                }
                else if (statusType == StatusType.Warning)
                {
                    animator.SetActive(true);
                    indecator.enabled = true;
                    indecator.material = buttonStatus.indecatorMaterial;
                    mats[3] = buttonStatus.baseMaterial;
                    indecatorText.text = buttonStatus.indecatorText;
                }
                else
                {
                    animator.SetActive(false);
                    indecator.enabled = true;
                    indecator.material = buttonStatus.indecatorMaterial;
                    mats[3] = buttonStatus.baseMaterial;
                    indecatorText.text = buttonStatus.indecatorText;
                }
                currentTextIndecator = buttonStatus.indecatorText;
            }
            localizeStringEvent.StringReference.SetReference("ButtonStates",currentTextIndecator);
            baseButton.materials = mats;
        }
    }
}
