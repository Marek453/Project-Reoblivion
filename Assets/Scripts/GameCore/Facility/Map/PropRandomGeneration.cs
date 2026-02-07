using NetCore;
using UnityEngine;

namespace GameCore.Facility.Map
{
    public class PropRandomGeneration : MonoBehaviour
    {
        public GameObject[] gameObjects;

        public void Start()
        {
            if (FindObjectOfType<NetworkMap>() == null) return;
            // 1. Получаем глобальный сид
            int globalSeed = FindObjectOfType<NetworkMap>().seed;

            // 2. Создаем уникальный сид для ЭТОГО объекта.
            // Мы складываем глобальный сид с хешем позиции или координатами.
            // Поскольку позиция у каждой комнаты разная, сид тоже будет разным.
            // Но если перезапустить игру с тем же globalSeed, результат будет тем же.
            int localSeed = globalSeed + GetComponentInParent<ReleaseAddresableOnDestroy>().transform.parent.position.GetHashCode();

            // 3. Используем System.Random, чтобы не сбрасывать глобальный Random Unity
            System.Random prng = new System.Random(localSeed);

            foreach (var obj in gameObjects)
            {
                obj.SetActive(false);
            }

            // 4. Используем наш локальный генератор (Next заменяет Range)
            int randomIndex = prng.Next(0, gameObjects.Length);
            gameObjects[randomIndex].SetActive(true);

        }
    }

}
