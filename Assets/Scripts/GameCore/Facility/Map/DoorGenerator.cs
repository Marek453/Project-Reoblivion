using NetCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameCore.Facility.Map
{
    public class DoorGenerator : MonoBehaviour
    {
        public DoorsPoint[] doorsPoints;
        public NetworkMap networkMap;

        public bool Generate()
        {
            return GeneratorTask_RemoveDoubledDoorPoints();
        }

        private void AssignDoorsToDoorPoints(List<DoorPoint> doorPoints, DoorsPoint doorsPoint)
        {
            for (int i = 0; i < doorsPoint.doors.Length; i++)
            {
                if (i < doorPoints.Count)
                {
                    // Assign door to door point
                    Transform doorPointTransform = doorPoints[i].transform;
                    doorsPoint.doors[i].transform.SetParent(doorPointTransform);
                    doorsPoint.doors[i].transform.position = doorPointTransform.position;
                    doorsPoint.doors[i].transform.rotation = doorPointTransform.rotation;
                    doorsPoint.doors[i].transform.localScale = Vector3.one;
                    doorsPoint.doors[i].SetActive(true);
                    doorsPoint.doors[i].GetComponent<GameCore.Facility.Door.Door>().AdjustButtons();
                }
                else
                {
                    // Deactivate excess doors
                    doorsPoint.doors[i].SetActive(false);
                    Debug.LogWarning($"More doors type {doorsPoint.typeDoorPoint} than door points. Door {i} deactivated.");
                }
            }
        }

        private bool GeneratorTask_RemoveDoubledDoorPoints()
        {
            Random.InitState(networkMap.seed);
            if (this.doorsPoints.Length == 0)
            {
                Debug.LogError("doorsPoints.Length is nullptr");
                return false;
            }
            foreach (var doorsPoint in doorsPoints)
            {
                List<DoorPoint> doorPointObjects = FindObjectsOfType<DoorPoint>().ToList();
                List<DoorPoint> doorPoints = doorPointObjects.FindAll(dp => dp.typeDoorPoint == doorsPoint.typeDoorPoint);

                if (doorPoints.Count == 0)
                {
                    return false;
                }
                List<DoorPoint> uniqueDoorPoints = RemoveDuplicateDoorPoints(doorPoints.ToArray());
                AssignDoorsToDoorPoints(uniqueDoorPoints, doorsPoint);
            }

            return true;
        }

        private List<DoorPoint> RemoveDuplicateDoorPoints(DoorPoint[] doorPointObjects)
        {
            List<DoorPoint> uniqueDoorPoints = new List<DoorPoint>();
            float minDistanceThreshold = 2f;

            foreach (DoorPoint currentDoorPoint in doorPointObjects)
            {
                bool isDuplicate = false;

                foreach (DoorPoint uniqueDoorPoint in uniqueDoorPoints)
                {
                    if (Vector3.Distance(currentDoorPoint.transform.position,
                                        uniqueDoorPoint.transform.position) < minDistanceThreshold)
                    {
                        isDuplicate = true;
                        DestroyImmediate(currentDoorPoint);
                        break;
                    }
                }

                if (!isDuplicate)
                {
                    uniqueDoorPoints.Add(currentDoorPoint);
                }
            }

            return uniqueDoorPoints;
        }
    }
}
