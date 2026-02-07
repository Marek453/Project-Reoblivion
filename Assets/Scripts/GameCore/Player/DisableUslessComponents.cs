using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace GameCore.Player
{
    public class DisableUslessComponents : NetworkBehaviour
    {
        public List<Behaviour> behaviours;
        private void Start()
        {
            if (!isLocalPlayer)
            {
                if (GetComponent<CharacterController>() != null)
                {
                    DestroyImmediate(GetComponent<CharacterController>());
                }
                foreach (var behaviour in behaviours)
                {
                    DestroyImmediate(behaviour);
                }
            }
        }


    }
}
