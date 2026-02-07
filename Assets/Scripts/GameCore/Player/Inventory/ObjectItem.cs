using UnityEngine;
using GameCore.Interfaces;
using Mirror;

namespace GameCore.Player.Inventory
{
    public class ObjectItem : NetworkBehaviour, IInteractable
    {
        public int id;
        public float duraction;

        public void Interact(PlayerManager player)
        {
            player.inventory.AddItem(id, duraction);
            CmdDestroy();
        }

        [Command(requiresAuthority =false)]
        private void CmdDestroy()
        {
            NetworkServer.Destroy(base.gameObject);
        }
    }
}
