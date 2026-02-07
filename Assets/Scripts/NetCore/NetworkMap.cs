using UnityEngine;
using System.Collections;
using Mirror;
using ALOB.Map;
using GameCore.Facility.Map;

namespace NetCore
{
    public class NetworkMap : NetworkBehaviour
    {
        public NewGen mapGenerator;

        [SyncVar(hook = nameof(SetSeed))]
        public int seed;
        public bool isGenerated;

        public void SetSeed(int oldInt, int newInt)
        {
            seed = newInt;
        }
        [Server]
        private void ServerStart()
        {
            seed = Random.Range(-9999999, 9999999);
        }

        private IEnumerator TryAgain()
        {
            DoorGenerator[] doorGenerators = FindObjectsOfType<DoorGenerator>();
            foreach (var item in doorGenerators)
            {
                bool generate = item.Generate();
                if(!generate)
                {
                    yield return new WaitForSeconds(1);
                   StartCoroutine(TryAgain());
                    yield break;
                }
            }
            isGenerated = true;
        }
        private IEnumerator Start()
        {
            if (isServer) ServerStart();
            yield return new WaitForSeconds(1);
            mapGenerator.generateMap(seed);
            StartCoroutine(TryAgain());
        }
    }
}
