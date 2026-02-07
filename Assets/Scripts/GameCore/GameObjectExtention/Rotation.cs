using UnityEngine;


namespace GameCore.GameObjectExtention
{
    public class Rotation : MonoBehaviour
    {
        public Vector3 speed;

        private void Update()
        {
            base.transform.Rotate(speed * Time.deltaTime);
        }
    }
}
