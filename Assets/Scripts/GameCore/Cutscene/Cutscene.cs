using System;
using System.Collections;
using System.Linq;
using GameCore.Audio.Music;
using GameCore.Player;
using GameCore.Player.Class;
using GameCore.UI;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace GameCore.Cutscene
{
    public class Cutscene : MonoBehaviour
    {
        public CutsceneData cutsceneData;
        public CinemachineCamera[] cinemachineCameras;
        public PlayerManager Player;
        private void Start()
        {
            Player.cutsceneManager.AddCutscene(cutsceneData);
            TimelineAsset timelineAsset = cutsceneData.playableDirector.playableAsset as TimelineAsset;
            cutsceneData.playableDirector.SetGenericBinding(timelineAsset.outputs.ToList()[0].sourceObject, Camera.main.GetComponent<CinemachineBrain>());
        }


        public void End()
        {
            ClassData classData = Player.classManager.GetClassData(Player.classManager.curRoleTypeId);
            UserMainInterface.singlenton.forceclassUI.gameObject.SetActive(true);
            // if (classData.teamRole != Team.RIP)
            // {
            //     MusicManager.Instance.PlayMusic(MusicEventType.Forceclass, false, true, 0, 1, false);
            // }
            // else
            // {
            //     MusicManager.Instance.PlayMusic(MusicEventType.Death, false, true, 0, 1, false);
            // }
            UserMainInterface.singlenton.forceclassUI.Forceclass(classData.className, classData.classColor);
            UserMainInterface.singlenton.PlayerUI.enabled = true;
            Player.playerController.enabled = true;
            Player.cursorManager.enabled = true;
        }

        public void Play(PlayerManager player)
        {
            Player = player;
            cutsceneData.playableDirector.Play();
        }

    }
}
