using GameCore.Player;
using System.Collections.Generic;
using GameCore.Round;
using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace GameCore.UI
{
    public class RoundManagerUI : NetworkBehaviour
    {
        public TMP_Text Timer, PlayersCounter;
        [SyncVar(hook = "SyncTime")]
        public float CurTime = 100;
        [SyncVar(hook = "SyncStart")]
        public bool RoundCanStart;
        public GameObject Panel;
        public Image TimerSlider;
        public GameObject button;
        public RoundManager roundManager;
        bool isTrue;
        float d;
        // Start is called before the first frame update
        public void StartRoundImmedately()
        {
            isTrue = false;
            SyncStart(false, true);
        }

        void Update()
        {

            if (isTrue)
            {
                if (PlayerManager.players.Count > 1)
                {
                    d -= 2 * Time.deltaTime;

                    CurTime = d;
                    Timer.text = "Round Start in: <color=yellow>" + CurTime.ToString("0") + "<size=30>s";
                    PlayersCounter.text = PlayerManager.players.Count.ToString();
                    TimerSlider.fillAmount = CurTime/100;
                }
                else
                {
                    Timer.text = "Round Start is <color=red>PAUSED</color>";
                    PlayersCounter.text = PlayerManager.players.Count.ToString();
                }
            }

        }
        public void SyncTime(float old, float New)
        {
            CurTime = New;
        }

        public void SyncStart(bool old, bool New)
        {
            RoundCanStart = New;
            Panel.SetActive(old);
            roundManager.StartRound();
        }
        private IEnumerator Start()
        {
            yield return new WaitUntil(() => CurTime <= 0);
            StartRoundImmedately();
        }
        public void CheckRound(bool RoundIsStarted)
        {
            d = CurTime;
            TimerSlider.fillAmount = CurTime;
            for (int i = 0; i < PlayerManager.players.Count; i++)
            {
                if (PlayerManager.players[i].isServer && PlayerManager.players[i].isLocalPlayer)
                {
                    button.SetActive(true);
                }
            }
            if (RoundIsStarted) return;
            isTrue = true;
            Panel.SetActive(true);
        }
    }
}
