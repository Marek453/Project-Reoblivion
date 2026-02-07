using UnityEngine;
using Mirror;
using GameCore.Player.Class;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

namespace GameCore.Round
{
    public class RoundManager : NetworkBehaviour
    {
        [Header("Settings")]
        public int minPlayersToStart = 2;

        private List<CharacterClassManager> _players = new List<CharacterClassManager>();
        public Button startButton;
        private CharacterClassManager characterClassManager;

        public List<Team> TeamLimit;

        [Server]
        public void StartRound()
        {
            _players = FindObjectsOfType<CharacterClassManager>().ToList();
             characterClassManager = _players.Find(pl => pl.isServer);


            for (int i = 0; i < _players.Count; i++)
            {
                RoleType num = FindRandomIdUsingDefinedTeam(TeamLimit[i]);
                _players[i].SetRole(num);
            }

            Debug.Log("Round has Started");
        }

        private RoleType FindRandomIdUsingDefinedTeam(Team team)
        {
            List<RoleType> list = new List<RoleType>();
            for (int i = 0; i < characterClassManager.availableClasses.Count; i++)
            {
                if (characterClassManager.availableClasses[i].teamRole == team)
                {
                    list.Add(characterClassManager.availableClasses[i].roleType);
                }
            }
            int index = Random.Range(0, list.Count);
            return list[index];
        }
    }
}