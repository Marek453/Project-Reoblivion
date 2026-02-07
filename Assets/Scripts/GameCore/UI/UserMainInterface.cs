using GameCore.Player;
using GameCore.UI.Inventory;
using UnityEngine;

namespace GameCore.UI
{
    public class UserMainInterface : MonoBehaviour
    {
        [Header("UI")]
        public ForceclassUI forceclassUI;
        public RoundManagerUI roundManagerUI;

        [Header("PlayerUI")]
        public Canvas PlayerUI;
        public PlayerHeathSlider heathSlider;
        public InventoryIU inventory;
        public DobbleSlider dobbleSlider;
        public RpcAbilityInterface rpcAbilityInterface;

        [Header("EscapeMenu")]
        public GameObject escapeMenu;
        public PlayerManager lockalPlayer;

        public static UserMainInterface singlenton;

        private void Awake()
        {
            singlenton = this;
        }

        public void Init(PlayerManager player)
        {
            lockalPlayer = player;
        }

        public void OnEscape()
        {
            if (lockalPlayer == null) return;
            lockalPlayer.cursorManager.isEscapeMenu = !lockalPlayer.cursorManager.isEscapeMenu;
            escapeMenu.SetActive(lockalPlayer.cursorManager.isEscapeMenu);
        }
    }
}
