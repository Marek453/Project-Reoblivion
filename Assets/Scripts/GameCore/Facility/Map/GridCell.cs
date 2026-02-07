using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Facility.Map
{
    public class GridCell
    {
        public Vector2Int position;
        public RoomData roomData;
        public GameObject instance;
        public bool isOccupied;
       //  public HashSet<Direction> connectedDirections = new HashSet<Direction>();
    }
}