using UnityEngine;
using GameCore.Player;

namespace GameCore.UI
{
    public class CursorManager : MonoBehaviour
    {
        public bool isEscapeMenu, isConsole, isUseMachine, isInventory, isSeach, isNotStarted, isMap, isAdminPanel;
        private PlayerManager playerManager;
        void Start()
        {
            playerManager = GetComponent<PlayerManager>();
        }
        void Update()
        {
            Cursor.lockState = (isEscapeMenu || isConsole || isMap || isUseMachine || isInventory || isAdminPanel || isNotStarted ? CursorLockMode.None : CursorLockMode.Locked);
            if (playerManager.playerController != null)
            {
                playerManager.playerController.SetLock(isEscapeMenu || isConsole || isUseMachine || isInventory || isAdminPanel || isNotStarted);
            }
        }
    }
}
