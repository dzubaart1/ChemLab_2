using UnityEngine;

namespace Core
{
    public class PlayerSpawner : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private GameManager _gameManager;
        
        [Header("Configs")]
        [SerializeField] private Player _playerPrefab;

        private void OnEnable()
        {
            _gameManager.LoadSceneCompleteEvent += OnLoadGameComplete;
        }

        private void OnDisable()
        {
            _gameManager.LoadSceneCompleteEvent -= OnLoadGameComplete;
        }

        private void OnLoadGameComplete(string scene)
        {
            Player player = FindObjectOfType<Player>();

            if (player == null)
            {
                player = Instantiate(_playerPrefab);
            }

            PlayerSpawnPoint playerSpawnPoint = FindObjectOfType<PlayerSpawnPoint>();

            if (playerSpawnPoint == null)
            {
                player.transform.position = Vector3.zero;
                player.transform.rotation = Quaternion.identity;
            }
            else
            {
                player.transform.position = playerSpawnPoint.Position;
                player.transform.rotation = playerSpawnPoint.Rotation;
            }
            
        }
    }
}