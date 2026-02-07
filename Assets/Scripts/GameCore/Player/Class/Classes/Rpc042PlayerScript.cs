using GameCore.Audio;
using GameCore.UI;
using Mirror;
using NetCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCore.Facility;
using GameCore.Player.Class.Classes.Human;
using System.Linq;
using GameCore.Audio.Music;
using GameCore.Console;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace GameCore.Player.Class.Classes.RPC042
{
    public class Rpc042PlayerScript : PlayerScriptBase
    {
        public Rpc042Stages currentStage = Rpc042Stages.Idle;
        public float maxAgroTime = 10f;
        public float attackDistance = 3f;
        public float attackCooldown = 1.2f;
        public float minDist;
        public int runSpeed = 10;
        public int walkSpeed = 5;
        public float timeReload;
        public LayerMask attackMask;

        public AudioSource audioSource;
        public AudioClip soundStart;
        public AudioClip soundLoop;
        public AudioClip soundEnd;
        public PlayerManager playerManager;

        private FacilityLight[] facilityLights;

        [SyncVar(hook = nameof(SetProgressState))]
        public bool isAbilityActive;
        public List<PlayerManager> targets = new List<PlayerManager>();

        private float currentAgroTime;
        private float currentAttackCooldown;
        public float currentTimeReload;
        private bool isAbilityInProgress;
        private Camera playerCamera;
        private Coroutine coroutine;
        private Coroutine updateTarget;
        private Coroutine lampDist;
        private Coroutine uiCorotuneSlider;
        private Slider uiSlider;
        private bool targetEquraed;
        private bool isKeyPressd;
        [SyncVar]
        private RpcType currentRpcType = RpcType.None;

        [Command]
        private void CmdSetProgress(bool value) => SetProgressState(isAbilityActive, value);

        private void SetProgressState(bool oldValue, bool newValue)
        {
            isAbilityActive = newValue;
        }

        public override void Init(RoleType role)
        {
            base.Init(role);
            if (!isInit)
            {
                ClearAll();
                UserMainInterface.singlenton.dobbleSlider.value = 0;
                return;
            }
            facilityLights = FindObjectsOfType<FacilityLight>();
            updateTarget = StartCoroutine(UpdateTargets());
        }

        private void Start()
        {
            if (isLocalPlayer)
            {
                DeveloperConsole.singleton.RegisterCommand("set_range", Set042Range);
            }
        }

        private void Set042Range(string[] args)
        {
            currentStage = Rpc042Stages.Raging;
        }

        private void ClearAll()
        {
            if (isLocalPlayer)
            {
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                    coroutine = null;
                }
                if (updateTarget != null)
                {
                    StopCoroutine(updateTarget);
                    updateTarget = null;
                }
                if (lampDist != null)
                {
                    StopCoroutine(lampDist);
                    lampDist = null;
                }
                if (targets.Count > 0)
                {
                    CleanUpTargets();
                }
                CmdStopSource();
            }
        }

        private IEnumerator UpdateTargets()
        {
            foreach (var player in PlayerManager.players)
            {
                if (player.classManager.GetClassData(player.classManager.curRoleTypeId).teamRole == Team.RPC) continue;

                float dist = Vector3.Distance(player.transform.position, transform.position);
                if (dist < minDist)
                {
                    if (isLocalPlayer)
                    {
                        switch (currentStage)
                        {
                            case Rpc042Stages.Raging:
                                if (targets.Contains(player))
                                {
                                    CmdSetMusic(RpcType.Target, player);
                                }
                                else
                                {
                                    CmdSetMusic(RpcType.NonTarget, player);
                                }
                                break;
                            case Rpc042Stages.Idle:
                                if (targets.Contains(player))
                                {
                                    CmdSetMusic(RpcType.IdleTarget, player);
                                }
                                else
                                {
                                    CmdSetMusic(RpcType.Idle, player);
                                }

                                break;
                            case Rpc042Stages.Ending:
                                if (targets.Contains(player))
                                {
                                    CmdSetMusic(RpcType.RangeEndTarget, player);
                                }
                                else
                                {
                                    CmdSetMusic(RpcType.RangeEnd, player);
                                }
                                break;
                        }
                    }
                    if (player.GetPlayerScript<HumanPlayerScript>().currentNoiseDb > 0)
                    {
                        if (targets.Contains(player)) continue;
                        targets.Add(player);
                        player.playerSelector.Select();

                    }
                }
                else if (currentStage == Rpc042Stages.Idle || (currentStage == Rpc042Stages.Raging && !targets.Contains(player)))
                {
                    if (isLocalPlayer)
                    {
                        CmdSetMusic(RpcType.None, player);
                    }
                }
            }
            if (targets.Count > 0 && currentStage == Rpc042Stages.Idle && !targetEquraed)
            {
                targetEquraed = true;
                uiSlider = UserMainInterface.singlenton.rpcAbilityInterface.SetEnableAbility(out uiCorotuneSlider, RoleType.RPC042, "Range", 20, timeReload);
            }
            yield return new WaitForSeconds(1);
            updateTarget = StartCoroutine(UpdateTargets());
        }
        [Command]
        private void CmdSetMusic(RpcType rpcType, PlayerManager target)
        {
            if (currentRpcType == rpcType) return;
            currentRpcType = rpcType;
            TargetSetMusic(target.connectionToClient, rpcType);
        }

        [TargetRpc]
        private void TargetSetMusic(NetworkConnectionToClient target, RpcType rpcType)
        {
            MusicManager.Instance.PlayRpcMusic(RoleType.RPC042, rpcType, true, false);
        }

        public void OnAttack()
        {
            if (!isInit || currentStage != Rpc042Stages.Active || !isAbilityActive) return;
            if (currentAttackCooldown > 0) return;
            currentAttackCooldown = 1;
            playerManager.playerAnimator.animator.Play("Attack");
        }

        public void OnReload(InputValue inputValue)
        {
            if (!targetEquraed) return;
            if (inputValue.Get<float>() > 0.5f)
            {
                isKeyPressd = true;
            }
            else
            {
                isKeyPressd = false;
            }
        }

        private void Update()
        {
            if (!isLocalPlayer) return;

            if (isInit)
            {
                if (currentAttackCooldown > 0)
                    currentAttackCooldown -= Time.deltaTime;
                UserMainInterface.singlenton.dobbleSlider.value = currentAttackCooldown;

                if (targetEquraed)
                {
                    if (uiSlider == null)
                    {
                        targetEquraed = false;
                        currentTimeReload = 0;
                        CleanUpTargets();
                    }
                    if (isKeyPressd)
                    {
                        currentTimeReload += 10 * Time.deltaTime;
                        uiSlider.value = currentTimeReload;
                        if (currentTimeReload >= timeReload)
                        {
                            if (uiSlider == null)
                                StopCoroutine(uiCorotuneSlider);
                            Destroy(uiSlider.gameObject);
                            currentTimeReload = 0;
                            currentStage = Rpc042Stages.Raging;
                        }
                    }
                    else
                    {
                        currentTimeReload = 0;
                        uiSlider.value = 0;
                    }
                }
                switch (currentStage)
                {
                    case Rpc042Stages.Raging when !isAbilityInProgress:
                        StartAbility();
                        break;

                    case Rpc042Stages.Active:
                        if (currentAgroTime > 0)
                        {
                            currentAgroTime -= Time.deltaTime;
                        }
                        break;
                }
            }
        }

        private void StartAbility()
        {
            CmdSetProgress(true);
            isAbilityInProgress = true;
            CmdPlayOneShot(Rpc042Stages.Raging);
            lampDist = StartCoroutine(SetLampDestarch());
            playerManager.playerAnimator.animator.SetBool("isAgr", true);
            coroutine = StartCoroutine(AbilityProcess());
        }



        private IEnumerator AbilityProcess()
        {
            yield return new WaitForSeconds(8);

            currentStage = Rpc042Stages.Active;
            playerManager.playerController.playerSetings.walkSpeed = runSpeed;
            playerManager.playerController.playerSetings.runSpeed = runSpeed;
            CmdPlayOneShot(Rpc042Stages.Active);
            currentAgroTime = maxAgroTime;

            yield return new WaitWhile(() => currentAgroTime > 0 && currentStage == Rpc042Stages.Active);
            playerManager.playerAnimator.animator.SetBool("isAgr", false);
            currentStage = Rpc042Stages.Ending;
            CleanUpTargets();
            playerManager.playerController.playerSetings.walkSpeed = walkSpeed;
            playerManager.playerController.playerSetings.runSpeed = walkSpeed;
            CmdPlayOneShot(Rpc042Stages.Ending);
            StopCoroutine(lampDist);
            lampDist = null;
            yield return new WaitWhile(() => audioSource.isPlaying);
            isAbilityInProgress = false;
            currentStage = Rpc042Stages.Idle;
            CmdSetProgress(false);
        }

        public IEnumerator SetLampDestarch()
        {
            if (isLocalPlayer)
            {
                CmdFlickering();
            }
            yield return new WaitForSeconds(10);
            lampDist = StartCoroutine(SetLampDestarch());
        }

        [Command]
        private void CmdFlickering()
        {
            RpcFlickering();
        }

        [ClientRpc]
        private void RpcFlickering()
        {
            if (facilityLights == null)
            {
                facilityLights = FindObjectsOfType<FacilityLight>();
            }
            foreach (var lamp in facilityLights)
            {
                float dist = Vector3.Distance(lamp.transform.position, transform.position);
                if (dist < minDist)
                {
                    lamp.EnableFlickering(10);
                }
            }
        }

        private void CleanUpTargets()
        {
            foreach (var target in targets)
            {
                target.playerSelector.Diselect();
            }
            targets.Clear();
        }

        [Command]
        private void CmdStopSource()
        {
            RpcStopSource();
        }

        [ClientRpc]
        private void RpcStopSource()
        {
            audioSource.Stop();
        }

        [Command]
        private void CmdPlayOneShot(Rpc042Stages stage)
        {
            RpcPlayOneShot(stage);
        }
        [ClientRpc]
        private void RpcPlayOneShot(Rpc042Stages stage)
        {
            switch (stage)
            {
                case Rpc042Stages.Raging:
                    audioSource.clip = soundStart;
                    audioSource.Play();
                    break;
                case Rpc042Stages.Active:
                    audioSource.loop = true;
                    audioSource.clip = soundLoop;
                    audioSource.Play();
                    break;
                case Rpc042Stages.Ending:
                    audioSource.Stop();
                    audioSource.loop = false;
                    audioSource.clip = null;
                    audioSource.PlayOneShot(soundEnd);
                    break;
            }
        }
    }
}
