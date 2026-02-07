using GameCore.Player.Controller;
using System;
using UnityEngine;

namespace GameCore.Player.Class
{
    [CreateAssetMenu(fileName = "NewClass", menuName = "Player/Class Data")]
    public class ClassData : ScriptableObject
    {
        public string className;        
        public RoleType roleType;
        public Team teamRole;
        public Color classColor;
        public PlayerSetings playerSetings;
        public string spawnPointTag; 
        public string nameCutScene;   

        [Header("Visuals")]
        public int modelIndex;
    }
}
