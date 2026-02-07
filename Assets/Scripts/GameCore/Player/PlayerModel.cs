using UnityEngine;
using Mirror;

namespace GameCore.Player
{
    public class PlayerModel : MonoBehaviour
    {
        public GameObject currentModel;

        public void SetModel(GameObject model)
        {
            currentModel = model;
        }
    }
}
