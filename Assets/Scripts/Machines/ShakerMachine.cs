using System.Collections;
using BioEngineerLab.Activities;
using Core;
using JetBrains.Annotations;
using Mechanics;
using UI.Components;
using UnityEngine;

namespace Machines
{
    public class ShakerMachine : MonoBehaviour, ISaveable
    {
        private class SavedData
        {
            public bool IsOpen;
            public bool IsShaking;
        }
        
        [SerializeField] private Transform _door;
        [SerializeField] private ButtonComponent _powerButton;
        [SerializeField] private ButtonComponent _rpmButton;
        
        [CanBeNull] private GameManager _gameManager;
        
        private bool _isOpen = false;
        private bool _isShaking = false;
        private SavedData _savedData = new SavedData();
        private Animator _animator;

        private void Awake()
        {
            _gameManager = GameManager.Instance;
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.Game.LoadGameEvent += OnLoadScene;
            _gameManager.Game.SaveGameEvent += OnSaveScene;
            
            _rpmButton.ClickBtnEvent += OnRpmButtonClick;
        }

        private void OnDisable()
        {
            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.Game.LoadGameEvent -= OnLoadScene;
            _gameManager.Game.SaveGameEvent -= OnSaveScene;
            
            _rpmButton.ClickBtnEvent -= OnRpmButtonClick;
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
            
            if (_door.transform.rotation.x < -0.01f && !_isOpen)
            {
                _isOpen = true;
                _gameManager.Game.CompleteTask(new DoorLabActivity(EDoor.ShakerDoor, EDoorActivity.Open));
            }
            
            else if (_door.transform.rotation.x > -0.01f && _isOpen)
            {
                _isOpen = false;
                _gameManager.Game.CompleteTask(new DoorLabActivity(EDoor.ShakerDoor, EDoorActivity.Closed));
            }
        }

        private void OnRpmButtonClick()
        {
            if (_powerButton.IsOn)
            {
                _animator.enabled = _rpmButton.IsOn;
                _isShaking = _rpmButton.IsOn;
            }
        }
        
        public void OnSaveScene()
        {
            _savedData.IsOpen = _isOpen;
            _savedData.IsShaking = _isShaking;
        }

        public void OnLoadScene()
        {
            if (_savedData.IsOpen)
            {
                _isOpen = true;
                _door.transform.rotation = new Quaternion(-0.5f, 0.5f, 0.5f, 0.5f);
            }
            else
            {
                _isOpen = false;
                _door.transform.rotation = new Quaternion(0, 0.7f, 0.7f, 0);
                Rigidbody rb = _door.GetComponent<Rigidbody>();
                rb.velocity = Vector3.zero;
            }

            _animator.enabled = _savedData.IsShaking;
            _isShaking = _savedData.IsShaking;
        }
    }
}