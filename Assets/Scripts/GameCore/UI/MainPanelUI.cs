using GameCore.Player;
using GameCore.Player.Class;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameCore.UI
{

    public class MainPanelUI : MonoBehaviour
    {
        public TMP_Text text;
        private PlayerManager localPlayer;
        private CharacterClassManager characterClassManager;
        public string dateLastRound;
        private void Start()
        {
            if(SceneManager.GetActiveScene().name == "MainMenu")
            {
                dateLastRound = (ES3.KeyExists("LastRound") ? ES3.Load("LastRound") : "---------").ToString();
              //  text.text = "LAST ROUND<br><size=30>" + ;
            }
            else
            {
                localPlayer = PlayerManager.players.Find(pl => pl.isLocalPlayer);
                characterClassManager = localPlayer.GetComponent<CharacterClassManager>();
            }
        }

        private void LateUpdate()
        {
            if (SceneManager.GetActiveScene().name == "Facility")
            {
                text.text = characterClassManager.curRoleTypeId + "<br><size=30>" + System.DateTime.Now;
            }
        }
    }
}
