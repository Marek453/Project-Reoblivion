using UnityEngine;

namespace GameCore.Entity
{
    public class ObjectLockAtCamera : MonoBehaviour
    {
        private void Update()
        {
            transform.LookAt(Camera.main.transform);
        }
    }
}
