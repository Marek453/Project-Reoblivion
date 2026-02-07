using UnityEngine;
using System;
using System.Collections.Generic;

namespace GameCore.Player.Class
{
    public class SpawnpointManager : MonoBehaviour
    {
        [Serializable]
        public class SpawnPoint
        {
            public Team team;
            public string spawnPointTag;
            public RoleType role = RoleType.None;
        }

        public List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
        
        public GameObject GetRandomPosition(Team team, RoleType role = RoleType.None)
        {
            if(role != RoleType.None)
            {
                SpawnPoint point = spawnPoints.Find(sp => sp.role == role);
                GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(point.spawnPointTag);
                return gameObjects[UnityEngine.Random.Range(0, gameObjects.Length)];
            }

            SpawnPoint spawnPoint = spawnPoints.Find(sp => sp.team == team);
            GameObject[] points = GameObject.FindGameObjectsWithTag(spawnPoint.spawnPointTag);
            return points[UnityEngine.Random.Range(0, points.Length)];
        }
    }
}
