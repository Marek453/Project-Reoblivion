using UnityEngine;

namespace GameCore.Player.Controller
{
    [CreateAssetMenu(fileName = "PlayerSetings", menuName = "Player/PlayerSetings")]
    public class PlayerSetings : ScriptableObject
    {
        
        [Header("Player Settings")]
        public int maxHealth;
        public int minHealth;
        public float walkSpeed = 5f;
        public float runSpeed = 8f;
        public float jumpSpeed = 5f;
        public float gravity = -9.81f;
        public bool isOnlyWalk = false;
        public bool isOnlyRun = false;

        [Header("Camera Shake Settings")]
        public float walkAmplitude = 0.5f;
        public float walkFrequency = 1.2f;
        public float runAmplitude = 1.2f;
        public float runFrequency = 2.5f;
        public float idleAmplitude = 0f;
        public float idleFrequency = 0f;
        public float transitionDuration = 0.3f;

        [Header("Jump Camera Shake")]
        public float jumpShakeAmplitude = 2f;
        public float jumpShakeDuration = 0.2f;
        public float landShakeAmplitude = 2.5f;
        public float landShakeDuration = 0.25f;

        [Header("Footstep Sounds")]
        public AudioClip[] walkStepClips;
        public AudioClip[] runStepClips;
        public AudioClip jumpClip;
        public AudioClip landClip;
        public float walkStepInterval = 0.5f;
        public float runStepInterval = 0.35f;
    }
}