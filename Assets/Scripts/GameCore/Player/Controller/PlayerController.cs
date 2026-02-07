using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using PrimeTween;
using GameCore.Player.Class.Classes.Human;

namespace GameCore.Player.Controller
{
    public class PlayerController : MonoBehaviour
    {

        [Header("Player Components")]
        public PlayerSetings playerSetings;
        [SerializeField]
        private CharacterController characterController;
        [SerializeField]
        private CinemachineCamera cinemachineCamera;
        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private PlayerManager playerManager;

        public Transform playerModel;
        public UnityEngine.Animator playerAnimator;
        public AudioSource idle, running;
        private CinemachineBasicMultiChannelPerlin noise;
        private CinemachineInputAxisController axisController;
        private float currentSpeed;
        private Vector2 move;
        private Vector3 velocity;
        private bool isGrounded;
        private bool wasGrounded;
        private CharacterState currentState;
        private float stepTimer;
        private bool isLocked;
        private float hozirontal;
        private float vertical;
        private bool isSprint;
        private HumanPlayerScript humanPlayerScript;

        public bool GetLock()
        {
            return isLocked;
        }

        public void SetLock(bool value)
        {
            isLocked = value;
            for (int i = 0; i < axisController.Controllers.Count; i++)
            {
                axisController.Controllers[i].Enabled = !isLocked;
            }
        }

        public void SetPlayerSettings(PlayerSetings _playerSetings)
        {
            playerSetings = Instantiate(_playerSetings);
            currentSpeed = playerSetings.walkSpeed;
        }

        private void Start()
        {
            currentSpeed = playerSetings.walkSpeed;
            humanPlayerScript = playerManager.GetPlayerScript<HumanPlayerScript>();
            if (cinemachineCamera != null)
            {
                axisController = cinemachineCamera.GetComponentInChildren<CinemachineInputAxisController>();
                noise = cinemachineCamera.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();
            }

            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            SetCharacterState(CharacterState.Idle);
        }

        private void Update()
        {
            wasGrounded = isGrounded;
            isGrounded = characterController.isGrounded;

            // Detect landing
            if (!wasGrounded && isGrounded)
            {
                PlayLandingShake();
                PlaySound(playerSetings.landClip);
            }

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            Vector3 moveDirection = (GetForward() * move.y + GetRight() * move.x).normalized;
            if (isLocked)
            {
                moveDirection = Vector3.zero;
            }
            characterController.Move(moveDirection * currentSpeed * Time.deltaTime);

            velocity.y += playerSetings.gravity * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
            currentSpeed = (!isSprint? playerSetings.walkSpeed : playerSetings.runSpeed);
            if (playerAnimator != null)
            {
                playerAnimator.SetFloat("vert", Mathf.Lerp(playerAnimator.GetFloat("vert"), vertical, 0.6f));
                playerAnimator.SetFloat("horiz", Mathf.Lerp(playerAnimator.GetFloat("horiz"), hozirontal, 0.6f));
            }
            UpdateCharacterState(moveDirection);
            HandleFootsteps();
            LookAnim();
        }

        private void HandleFootsteps()
        {
            if (!isGrounded || currentState == CharacterState.Idle || currentState == CharacterState.Jumping)
            {
                stepTimer = 0f;
                return;
            }


            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                if (currentState == CharacterState.Walking)
                {
                    PlayRandomStep(playerSetings.walkStepClips);
                    stepTimer = playerSetings.walkStepInterval;             
                }
                else if (currentState == CharacterState.Running)
                {
                    PlayRandomStep(playerSetings.runStepClips);
                    stepTimer = playerSetings.runStepInterval;
                }
            }
        }

        private void PlayRandomStep(AudioClip[] clips)
        {
            if (clips == null || clips.Length == 0) return;
            int index = Random.Range(0, clips.Length);
            PlaySound(clips[index]);
        }

        private void PlaySound(AudioClip clip)
        {
            if (clip == null || audioSource == null) return;
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(clip);
        }

        private void UpdateCharacterState(Vector3 moveDirection)
        {
            if (!isGrounded)
            {
                SetCharacterState(CharacterState.Jumping);
                return;
            }

            if (moveDirection.magnitude < 0.1f)
            {
                SetCharacterState(CharacterState.Idle);
            }
            else if (Mathf.Approximately(currentSpeed, playerSetings.runSpeed))
            {
                SetCharacterState(CharacterState.Running);
            }
            else
            {
                SetCharacterState(CharacterState.Walking);
            }
        }

        private void SetCharacterState(CharacterState newState)
        {
            if (newState == currentState)
                return;

            currentState = newState;

            if (noise == null) return;

            float targetAmp = playerSetings.idleAmplitude;
            float targetFreq = playerSetings.idleFrequency;

            switch (currentState)
            {
                case CharacterState.Idle:
                    targetAmp = playerSetings.idleAmplitude;
                    targetFreq = playerSetings.idleFrequency;
                    Tween.Custom(idle.volume, 1, 2, v => idle.volume = v);
                    Tween.Custom(running.volume, 0, 2, v => running.volume = v);
                    humanPlayerScript.SetNoise(0);
                    break;
                case CharacterState.Walking:
                    targetAmp = playerSetings.walkAmplitude;
                    targetFreq = playerSetings.walkFrequency;
                    Tween.Custom(idle.volume, 1, 2, v => idle.volume = v);
                    Tween.Custom(running.volume, 0, 2, v => running.volume = v);
                    humanPlayerScript.SetNoise(20);
                    break;
                case CharacterState.Running:
                    targetAmp = playerSetings.runAmplitude;
                    targetFreq = playerSetings.runFrequency;
                    Tween.Custom(idle.volume, 0, 2, v => idle.volume = v);
                    Tween.Custom(running.volume, 1, 2, v => running.volume = v);
                    humanPlayerScript.SetNoise(40);
                    break;
                case CharacterState.Jumping:
                    targetAmp = playerSetings.runAmplitude * 0.5f;
                    targetFreq = playerSetings.runFrequency * 0.8f;
                    Tween.Custom(idle.volume, 0, 2, v => idle.volume = v);
                    Tween.Custom(running.volume, 1, 2, v => running.volume = v);
                    humanPlayerScript.SetNoise(30);
                    break;
            }

            Tween.StopAll(noise);
            Tween.Custom(noise.AmplitudeGain, targetAmp, playerSetings.transitionDuration, v => noise.AmplitudeGain = v);
            Tween.Custom(noise.FrequencyGain, targetFreq, playerSetings.transitionDuration, v => noise.FrequencyGain = v);
        }

        public void OnMove(InputValue inputValue)
        {
            move = inputValue.Get<Vector2>();
            Vector2 vector = Vector2.zero;
            if (currentState == CharacterState.Running)
            {
                vector = (inputValue.Get<Vector2>() * 2);
            }
            else
            {
                vector = (inputValue.Get<Vector2>());
            }
            hozirontal = vector.x;
            vertical = vector.y;

        }

        public void LookAnim()
        {
            if (playerModel == null) return;
            Vector3 camForward = cinemachineCamera.transform.forward;
            camForward.y = 0;
            camForward.Normalize();

            Vector3 charForward = playerModel.forward;
            charForward.y = 0;
            charForward.Normalize();

            float angle = Vector3.SignedAngle(charForward, camForward, Vector3.up);

            float horizValue = angle / -10.0f;

            horizValue = Mathf.Clamp(horizValue, -2f, 2f);
            playerAnimator.SetFloat("rotate", horizValue);
            float speed = -horizValue;
            playerAnimator.SetFloat("Speed", (horizValue < 0 ? speed : horizValue));
        }

        public void OnSprint(InputValue inputValue)
        {
            if (isLocked) return;
            if (inputValue.Get<float>() > 0.5f)
            {
                isSprint = true;
            }
            else
            {
               isSprint = false;
            }
        }

        public void OnJump(InputValue inputValue)
        {
            if (isLocked) return;
            if (inputValue.isPressed && isGrounded)
            {
                if (playerAnimator != null) playerAnimator.SetTrigger("Jump");

                velocity.y = Mathf.Sqrt(playerSetings.jumpSpeed * -2f * playerSetings.gravity);
                PlayJumpShake();
                PlaySound(playerSetings.jumpClip);
            }
        }

        private void PlayJumpShake()
        {
            if (noise == null) return;

            float originalAmp = noise.AmplitudeGain;

            Tween.Custom(originalAmp, playerSetings.jumpShakeAmplitude, 0.05f, v => noise.AmplitudeGain = v)
                .Chain(Tween.Custom(playerSetings.jumpShakeAmplitude, originalAmp, playerSetings.jumpShakeDuration, v => noise.AmplitudeGain = v));
        }

        private void PlayLandingShake()
        {
            if (noise == null) return;

            float originalAmp = noise.AmplitudeGain;

            Tween.Custom(originalAmp, playerSetings.landShakeAmplitude, 0.05f, v => noise.AmplitudeGain = v)
                .Chain(Tween.Custom(playerSetings.landShakeAmplitude, originalAmp, playerSetings.landShakeDuration, v => noise.AmplitudeGain = v));
        }

        private Vector3 GetForward()
        {
            Vector3 forward = cinemachineCamera.transform.forward;
            forward.y = 0;
            return forward.normalized;
        }

        private Vector3 GetRight()
        {
            Vector3 right = cinemachineCamera.transform.right;
            right.y = 0;
            return right.normalized;
        }
    }
}
