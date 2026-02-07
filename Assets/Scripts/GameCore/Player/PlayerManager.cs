using System.Collections.Generic;
using GameCore.Player.Controller;
using GameCore.UI;
using Mirror;
using GameCore.Player.Class.Classes;
using UnityEngine;
using GameCore.Player.Class;
using GameCore.Player.Animator;
using GameCore.Cutscene;
using GameCore.Audio;
using System.Linq;

namespace GameCore.Player
{
    public sealed class PlayerManager : NetworkBehaviour
    {
        public static List<PlayerManager> players { get; set; } = new List<PlayerManager>();
        public PlayerScriptBase[] playerScriptBases;
        public CursorManager cursorManager;
        public PlayerAnimator playerAnimator;
        public PlayerStats playerStats;
        public PlayerModel playerModel;
        public CharacterClassManager classManager;
        public CutsceneManager cutsceneManager;
        public PlayerSelector playerSelector;
        public GameCore.Player.Inventory.Inventory inventory;
        public PlayerController playerController;

        public T GetPlayerScript<T>() where T : PlayerScriptBase
        {
            return playerScriptBases.OfType<T>().FirstOrDefault();
        }

        private void Awake()
        {
            players.Add(this);
        }

        private void Start()
        {
            if (isLocalPlayer)
                FindAnyObjectByType<UserMainInterface>().Init(this);
        }

        private void OnDestroy()
        {
            players.Remove(this);
        }
    }
}
