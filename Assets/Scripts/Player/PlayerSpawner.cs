using UnityEngine;

namespace Core
{
    public class PlayerSpawner : MonoBehaviour
    {
        [Header("Configs")]
        [SerializeField] private Player _playerPrefab;
        
        public Player Player { get; private set; }

        public void InitPlayer()
        {
            if (Player == null)
            {
                Player = Instantiate(_playerPrefab);
            }

            PlayerSpawnPoint playerSpawnPoint = FindObjectOfType<PlayerSpawnPoint>();

            if (playerSpawnPoint == null)
            {
                Player.transform.position = Vector3.zero;
                Player.transform.rotation = Quaternion.identity;
            }
            else
            {
                Player.transform.position = playerSpawnPoint.Position;
                Player.transform.rotation = playerSpawnPoint.Rotation;
            }
            
            Player.Init();
        }
    }
}