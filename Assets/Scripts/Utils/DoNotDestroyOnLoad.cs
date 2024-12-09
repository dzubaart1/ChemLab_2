using UnityEngine;

namespace Utils
{
    public class DoNotDestroyOnLoad : MonoBehaviour
    {
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}