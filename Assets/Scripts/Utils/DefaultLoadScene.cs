using Core;
using UnityEngine;

namespace Utils
{
    public class DefaultLoadScene : MonoBehaviour
    {
        [Header("Configs")]
        [SerializeField] private string _sceneName;
        
        [Space]
        [Header("Refs")]
        [SerializeField] private GameManager _gameManager;
        
        private void Start()
        {
            _gameManager.LoadScene(_sceneName);
        }
    }
}