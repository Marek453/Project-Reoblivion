using UnityEngine;
using UnityEngine.InputSystem;
using GameCore.Interfaces;
using GameCore.Player;

namespace GameCore.Player
{
    public class PlayerInteract : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float interactionDistance = 3f;
        [SerializeField] private LayerMask interactionLayerMask;

        [Header("References")]
        [SerializeField] private Camera playerCamera;
        [SerializeField] private PlayerManager playerManager;

        void Start()
        {
            playerCamera = Camera.main;
        }
        public void OnInteract(InputValue inputValue)
        {
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward,
                out RaycastHit hit, interactionDistance, interactionLayerMask))
            {
                ProcessInteraction(hit);
            }
        }

        private void ProcessInteraction(RaycastHit hit)
        {
            IInteractable interact = (IInteractable)hit.collider.GetComponentInParent(typeof(IInteractable));
            if (interact != null)
            {
                interact.Interact(playerManager);
            }
        }
    }
}