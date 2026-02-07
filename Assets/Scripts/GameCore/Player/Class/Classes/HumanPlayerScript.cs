using Mirror;
using UnityEngine;

namespace GameCore.Player.Class.Classes.Human
{
    public class HumanPlayerScript : PlayerScriptBase
    {
        [SyncVar]
        public float currentNoiseDb = 0;

        public void SetNoise(float value)
        {
            if(!isLocalPlayer) return;

            CmdSetNoise(value);
        }
        [Command]
        private void CmdSetNoise(float value)
        {
            currentNoiseDb = value;
        }
    }
}