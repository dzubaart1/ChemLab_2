using System;
using System.Collections;
using BioEngineerLab.Activities;
using Core;
using JetBrains.Annotations;
using Mechanics;
using UI.Components;
using UnityEngine;

namespace Machines
{
    public class DoorMachine : MonoBehaviour, ISaveable
    {
        public event Action DoorClosedEvent;
        private class SavedData
        {
            public bool IsOpen;
        }
        
        [Header("Transforms")]
        [SerializeField] private Transform _door;
        [SerializeField] private Quaternion _closed;
        [SerializeField] private Quaternion _opened;
        
        [Space]
        [Header("Door Type")]
        [SerializeField] EDoor _doorType;
        
        [Space]
        [Header ("Sendable parameters")]
        [SerializeField] private bool _isOpenTaskSendable  = true;
        [SerializeField] private bool _isCloseTaskSendable  = true;
        
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
            
            if (!IsRotationEqual(_closed, 0.01f) && !_isOpen)
            {
                _isOpen = true;
                if (_isOpenTaskSendable)
                {
                    _gameManager.Game.CompleteTask(new DoorLabActivity(_doorType, EDoorActivity.Open));
                }
            }
            
            else if (IsRotationEqual(_closed, 0.01f) && _isOpen)
            {
                _isOpen = false;
                if (_isCloseTaskSendable)
                {
                    _gameManager.Game.CompleteTask(new DoorLabActivity(_doorType, EDoorActivity.Closed));
                }
                DoorClosedEvent?.Invoke();
            }
        }

        private bool IsRotationEqual(Quaternion q, float accuracy)
        {
            bool result = true;
            
            result &= (Math.Abs(_door.transform.rotation.x - q.x) < accuracy);
            
            result &= (Math.Abs(_door.transform.rotation.y - q.y) < accuracy);
            
            result &= (Math.Abs(_door.transform.rotation.z - q.z) < accuracy);
            
            return result;
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
                _door.transform.rotation = _opened;
            }
            else
            {
                _isOpen = false;
                _door.transform.rotation = _closed;
                Rigidbody rb = _door.GetComponent<Rigidbody>();
                rb.velocity = Vector3.zero;
            }
        }
    }
}