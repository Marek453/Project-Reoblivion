using System;
using GameCore.Player.Class;
using UnityEngine;
using UnityEngine.Playables;

namespace GameCore.Cutscene
{
    [Serializable]
    public class CutsceneData
    {
        public Team cutsceneTeam;
        public PlayableDirector playableDirector;
        public Cutscene cutscene;
    }
}

