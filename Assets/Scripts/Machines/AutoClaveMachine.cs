using BioEngineerLab.Activities;
using Core;
using JetBrains.Annotations;
using Mechanics;
using UI.Components;
using UnityEngine;

namespace Machines
{
    public class AutoClaveMachine : MonoBehaviour, ISaveable
    {
        private class SavedData
        {
            public bool IsOn;
            public bool IsOpen;
        }

        [SerializeField] private ButtonComponent _powerButton;
        [SerializeField] private VRSocketInteractor[] _socketInteractors;
        [SerializeField] private Transform _door;
        
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
            
            if (_door.transform.rotation.z > 0.01f && !_isOpen)
            {
                _isOpen = true;
            }
            
            else if (_door.transform.rotation.z < 0.01f && _isOpen)
            {
                _isOpen = false;
                _gameManager.Game.CompleteTask(new DoorLabActivity(EDoor.AutoClaveDoor, EDoorActivity.Closed));
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
                _door.transform.rotation = new Quaternion(-0.7f, 0f, 0.5f, 0.7f);
            }
            else
            {
                _door.transform.rotation = new Quaternion(-0.7f, 0f, 0f, 0.7f);
            }
        }
    }
}