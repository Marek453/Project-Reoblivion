using UnityEngine;
using GameCore.Player;
using GameCore.Interfaces;
using Mirror;

namespace GameCore.Facility.Door
{
    public class Door : NetworkBehaviour,IInteractable
    {
        public AccessType accessType;
        public FacilityTouchButton[] facilityTouchButtons;
        public FacilityTouchButton.StatusType currentStatus;
        public Animator animator;
        public AudioClip open, close;
        public AudioSource source;
        [SyncVar(hook = nameof(OnOpenChange))]
        public bool isOpen;
        public bool isLocked;
        public bool isReactorLocked;
        public float processingTime;
        public float currentTime;
        public bool isProcess;

        public void OnOpenChange(bool oldValue,bool newValue)
        {
            isOpen = newValue;
            animator.SetBool("isOpen", newValue);
            if(newValue)
            {
                source.PlayOneShot(open);
            }
            else
            {
                source.PlayOneShot(close);
            }
        }

        public void AdjustButtons()
        {
            foreach (var button in facilityTouchButtons)
            {
                button.Adjust();
            }
        }

        public void Interact(PlayerManager player)
        {
            if(player.isLocalPlayer)
            {
                if(isLocked || isReactorLocked)
                {
                    for (int i = 0; i < facilityTouchButtons.Length; i++)
                    {
                        facilityTouchButtons[i].Error();
                    }
                    return;
                }
                CmdInteract();
            }
        }

        private void SetButtonStatus(FacilityTouchButton.StatusType status)
        {
            if(currentStatus == status) return;
            for (int i = 0; i < facilityTouchButtons.Length; i++)
            {
                facilityTouchButtons[i].SetButtonStatus(status);
            }
            currentStatus = status;
        }

        public void Update()
        {
            if (currentTime > 0)
            {
                currentTime -= 1 * Time.deltaTime;
            }
            else
            {
                isProcess = false;
            }
            if(isReactorLocked)
            {
                SetButtonStatus(FacilityTouchButton.StatusType.ReactorLock);
                return;
            }
            if(isLocked)
            {
                SetButtonStatus(FacilityTouchButton.StatusType.Locked);
                return;
            }
            
            if(isProcess)
            {
                SetButtonStatus(FacilityTouchButton.StatusType.Warning);
            }
            else
            {
                FacilityTouchButton.StatusType statusType = FacilityTouchButton.StatusType.None;
                bool isOpenLocked = isOpen && accessType != AccessType.None;
                if (isOpenLocked)
                {
                    statusType = FacilityTouchButton.StatusType.OpenKey;
                }
                else if(!isOpenLocked && accessType != AccessType.None)
                {
                    statusType = FacilityTouchButton.StatusType.CloseKey;
                }
                else if(isOpen)
                {
                    statusType = FacilityTouchButton.StatusType.Open;
                }
                else
                {
                    statusType = FacilityTouchButton.StatusType.Close;
                }    
                SetButtonStatus(statusType);
            }
        }

        [Command(requiresAuthority = false)]
        private void CmdInteract()
        {
            if (currentTime > 0) return;
            isOpen = !isOpen;
            RpcInteract();
        }

        [ClientRpc]
        private void RpcInteract()
        {
            currentTime = processingTime;
            isProcess = true;
        }
    }
}
