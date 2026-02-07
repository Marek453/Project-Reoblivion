using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Facility.Map
{
    [System.Serializable]
    public class RoomData
    {
        public RoomType roomType;
        public GameObject prefab;
       // public List<Direction> doorDirections; // Где у комнаты двери
        public int weight = 1; // Вероятность появления
        public int rotationAngle = 0; // Для вращения комнаты (0, 90, 180, 270)
    }
}