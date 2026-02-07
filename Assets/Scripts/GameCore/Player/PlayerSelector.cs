using GameCore.Entity;
using UnityEngine;

namespace GameCore.Player
{
    public class PlayerSelector : MonoBehaviour
    {
        public ObjectLockAtCamera indecator;
       public void Select()
        {
            indecator.gameObject.SetActive(true);
        }
        public void Diselect()
        {
            indecator.gameObject.SetActive(false);
        }
    }
}
