using System;
using UnityEngine;

namespace GameCore.Facility.Map
{
    [Serializable]
    public class DoorsPoint
    {
        public TypeDoorPoint typeDoorPoint;
        public GameObject[] doors;
    }
}