using UnityEngine;

namespace Core
{
    public class PlayerSpawnPoint : MonoBehaviour
    {
        public Vector3 Position
        {
            get
            {
                return transform.position;
            }
        }

        public Quaternion Rotation
        {
            get
            {
                return transform.rotation;
            }
        }
    }
}