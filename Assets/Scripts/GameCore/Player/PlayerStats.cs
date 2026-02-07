using Mirror;
using GameCore.UI;
using UnityEngine;

namespace GameCore.Player
{
    public class PlayerStats : NetworkBehaviour
    {
        [SyncVar]
        public float currentHeath;
        public float currentMaxHeath;

        private PlayerHeathSlider heathSlider;

        private void Start()
        {
            heathSlider = UserMainInterface.singlenton.heathSlider;
        }

        private void OnHeathChange(float oldValue, float newValue)
        {
            if (!isLocalPlayer) return;
            heathSlider.ChangeHp(newValue);
        }

        [TargetRpc]
        private void TargetApply(float maxValue, Color color)
        {
            heathSlider.SetupHeath(maxValue, 0, color);
        }

        [Server]
        public void ApplyNewHeath(float maxValue, Color color)
        {
            TargetApply(maxValue, color);
            currentMaxHeath = maxValue;
            currentHeath = maxValue;
        }
    }
}
