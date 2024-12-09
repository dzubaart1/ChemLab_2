using UnityEngine;

namespace Core
{
    public class Player : MonoBehaviour
    {
        [SerializeField] public Camera Camera;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}