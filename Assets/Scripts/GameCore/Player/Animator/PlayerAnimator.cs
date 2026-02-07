using UnityEngine.InputSystem;
using UnityEngine;
using PrimeTween;
using Mirror;

namespace GameCore.Player.Animator
{
    public class PlayerAnimator : NetworkBehaviour
    {
        [Header("Настройки")]
        public PlayerManager playerManager;

        public UnityEngine.Animator animator;


        private UnityEngine.Animator TryGetAnimator()
        {
            if(playerManager.classManager.curRoleTypeId == Class.RoleType.None) return null;

            return playerManager.classManager.characterModels[playerManager.classManager.GetClassData(playerManager.classManager.curRoleTypeId).modelIndex].GetComponent<UnityEngine.Animator>();
        }

        void Update()
        {
            if (animator == null)
            {
                animator = TryGetAnimator();
                return;
            }
        }
    }
}
