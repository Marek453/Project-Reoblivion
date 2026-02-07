using GameCore.Audio.Music;
using GameCore.Console;
using GameCore.Cutscene;
using GameCore.UI;
using Mirror;
using NetCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

using Random = UnityEngine.Random;

namespace GameCore.Player.Class
{
    public class CharacterClassManager : NetworkBehaviour
    {
        [Header("Setup")]
        public List<ClassData> availableClasses;

        [Header("References")]
        public GameObject[] characterModels;
        public PlayerManager playerManager;

        [SyncVar(hook = nameof(OnRoleChanged))]
        public RoleType curRoleTypeId = RoleType.Spectator;

        public override void OnStartLocalPlayer()
        {
            StartCoroutine(waitForGeneration());
            if (isLocalPlayer)
            {
                DeveloperConsole.singleton.RegisterCommand("set_class", SetClass);
                DeveloperConsole.singleton.RegisterCommand("add_bot", RequestAddBot);
            }
        }

        private IEnumerator waitForGeneration()
        {
            yield return new WaitUntil(() => FindObjectOfType<NetworkMap>().isGenerated);
            yield return new WaitForSeconds(2);
            UserMainInterface.singlenton.roundManagerUI.CheckRound(UserMainInterface.singlenton.roundManagerUI.RoundCanStart);
        }

        private void SetClass(string[] args)
        {
            if (args.Length > 0 && Enum.TryParse<RoleType>(args[0], true, out RoleType amount))
            {
                if (isLocalPlayer)
                {
                    CmdRequestSpawn(amount);
                }
            }
        }
        public void RequestAddBot(string[] args)
        {
            if (args.Length > 0 && Enum.TryParse<RoleType>(args[0], true, out RoleType amount))
            {
                if (isLocalPlayer)
                {
                    CmdRequestAddBot(amount);
                }
            } 
        }

        [Command]
        private void CmdRequestAddBot(RoleType role)
        {
            AddBot(role);
        }
        
        [Server]
        private void AddBot(RoleType role)
        {
            NetworkConnectionToClient client = new NetworkConnectionToClient(-1);
            GameObject bot = Instantiate(NetworkManager.singleton.playerPrefab);
            NetworkServer.AddPlayerForConnection(client,bot);
            bot.GetComponent<CharacterClassManager>().SetRole(role);
            Debug.Log($"Bot {client.connectionId} :: {role} has connected");
        }


        [Command]
        void CmdRequestSpawn(RoleType role)
        {
            SetRole(role);
        }

        [Server]
        public void SetRole(RoleType role)
        {
            ClassData data = GetClassData(role);
            if (data == null && role != RoleType.Spectator)
            {
                return;
            }
            curRoleTypeId = role;
            if (role != RoleType.Spectator)
            {
                ApplyClassStats(data);
                TeleportToSpawn(data.spawnPointTag);
            }
        }

        [ClientRpc]
        void TeleportToSpawn(string tag)
        {
            GameObject[] spawns = GameObject.FindGameObjectsWithTag(tag);
            if (spawns.Length > 0)
            {
                Transform spawnPoint = spawns[Random.Range(0, spawns.Length)].transform;
                CharacterController cc = GetComponent<CharacterController>();
                if (cc != null) cc.enabled = false;

                transform.position = spawnPoint.position;
                transform.rotation = spawnPoint.rotation;

                if (cc != null) cc.enabled = true;
            }
        }
       
        void OnRoleChanged(RoleType oldRole, RoleType newRole)
        {
            ClassData data = GetClassData(newRole);

            UpdateVisuals(data);

            if (isLocalPlayer)
            {    
                playerManager.playerController.SetPlayerSettings(data.playerSetings);
                if (data.modelIndex >= 0 && data.modelIndex < characterModels.Length)
                { 
                    playerManager.playerController.playerModel  = characterModels[data.modelIndex].transform;
                    playerManager.playerController.playerAnimator = characterModels[data.modelIndex].GetComponent<UnityEngine.Animator>();
                    playerManager.playerAnimator.animator = characterModels[data.modelIndex].GetComponent<UnityEngine.Animator>();
                }
                foreach (var item in playerManager.playerScriptBases)
                {
                    item.Init(newRole);
                }
                //UserMainInterface.singlenton.forceclassUI.Forceclass(data.className, data.classColor);
                playerManager.cutsceneManager.PlayCatscene(data.teamRole,playerManager);
                //playerManager.playerController.enabled = true;
                //playerManager.cursorManager.enabled = true;
                Debug.Log($"{newRole}");
            }
        }

        void UpdateVisuals(ClassData data)
        {
                foreach (var model in characterModels)
                {
                    model.SetActive(false);
                }

                if (data == null) return;

                if (data.modelIndex >= 0 && data.modelIndex < characterModels.Length)
                {
                characterModels[data.modelIndex].SetActive(true);
                if (!isLocalPlayer)
                {
                    foreach (Renderer renderer in GetRenderers(characterModels[data.modelIndex]))
                    {
                        renderer.enabled = true;
                    }
                    playerManager.playerAnimator.animator = characterModels[data.modelIndex].GetComponent<UnityEngine.Animator>();
                }
            }
        }

        private Renderer[] GetRenderers(GameObject model)
        {
            return model.GetComponentsInChildren<Renderer>();
        }
        void ApplyClassStats(ClassData data)
        { 
            playerManager.playerStats.ApplyNewHeath(data.playerSetings.maxHealth, data.classColor);
            UserMainInterface.singlenton.inventory.SetupInventoryUI(data.classColor);
        }

        private void OnDestroy()
        {
            if (isLocalPlayer)
            {
                ES3.Save("LastRound", System.DateTime.Now);
            }
        }

        public ClassData GetClassData(RoleType role)
        {
            return availableClasses.FirstOrDefault(c => c.roleType == role);
        }

    }
}
