using UnityEngine;
using Mirror;

namespace GameCore.Player.Class.Classes
{
    public abstract class PlayerScriptBase : NetworkBehaviour
    {
        public RoleType currentRole;
        public bool isInit;
        public virtual void Init(RoleType role)
        {
            if(currentRole == role)
            {
                isInit = true;
            }
            else
            {
                isInit= false;
            }
        }
    }
}
