using System.Collections.Generic;
using GameCore.Player;
using GameCore.Player.Class;
using UnityEngine;

namespace GameCore.Cutscene
{
    public class CutsceneManager : MonoBehaviour
    {
        public List<CutsceneData> cutsceneDatas;

        public void AddCutscene(CutsceneData cutsceneData)
        {
            if (cutsceneDatas.Find(cutsene => cutsene.cutsceneTeam == cutsceneData.cutsceneTeam) == null)
            {
                cutsceneDatas.Add(cutsceneData);
            }
        }

        public void PlayCatscene(Team team, PlayerManager player)
        {
            CutsceneData cutscene = cutsceneDatas.Find(ct => ct.cutsceneTeam == team);
            if(cutscene == null)
            {
                cutsceneDatas[0].cutscene.End();
                Debug.LogError($"Cutscene for {team} not found. {nameof(CutsceneManager)}::{nameof(PlayCatscene)}(Team team, PlayerManager player);" );
                return;
            }
            cutscene.cutscene.Play(player);
        }
    }
}