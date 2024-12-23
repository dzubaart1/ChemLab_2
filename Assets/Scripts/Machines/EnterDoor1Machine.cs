using System.Collections;
using BioEngineerLab.Activities;
using Core;
using JetBrains.Annotations;
using Mechanics;
using UI.Components;
using UnityEngine;

namespace Machines
{
    public class EnterDoor1Machine : MonoBehaviour, ISaveable
    {
        private class SavedData
        {
            public bool IsOpen;
        }
        
        [SerializeField] private Transform _door;
        [SerializeField] private GameObject _handle;
        
        [CanBeNull] private GameManager _gameManager;
        
        private bool _isOpen = false;
        private SavedData _savedData = new SavedData();

        private void Awake()
        {
            _gameManager = GameManager.Instance;
        }

        private void OnEnable()
        {
            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.Game.LoadGameEvent += OnLoadScene;
            _gameManager.Game.SaveGameEvent += OnSaveScene;
        }

        private void OnDisable()
        {
            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.Game.LoadGameEvent -= OnLoadScene;
            _gameManager.Game.SaveGameEvent -= OnSaveScene;
        }

        private void Start()
        {
            OnSaveScene();
        }

        private void Update()
        {
            if (_gameManager == null)
            {
                return;
            }
            
            if (_door.transform.rotation.y > 0.01f && !_isOpen)
            {
                _isOpen = true;
                _gameManager.Game.CompleteTask(new DoorLabActivity(EDoor.EnterDoor1, EDoorActivity.Open));
            }
            
            else if (_door.transform.rotation.y < 0.01f && _isOpen)
            {
                _isOpen = false;
                _handle.SetActive(false);
                _gameManager.Game.CompleteTask(new DoorLabActivity(EDoor.EnterDoor1, EDoorActivity.Closed));
            }
        }
        
        public void OnSaveScene()
        {
            _savedData.IsOpen = _isOpen;
        }

        public void OnLoadScene()
        {
            if (_savedData.IsOpen)
            {
                _isOpen = true;
                _door.transform.rotation = new Quaternion(0, 0.7f, 0, 0.7f);
            }
            else
            {
                _isOpen = false;
                _door.transform.rotation = new Quaternion(0, 0, 0, 1f);
                Rigidbody rb = _door.GetComponent<Rigidbody>();
                rb.velocity = Vector3.zero;
            }
        }
    }
}