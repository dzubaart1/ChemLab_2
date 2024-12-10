using BioEngineerLab.Activities;
using Core;
using Core.Services;
using JetBrains.Annotations;
using UI.Components;
using UnityEngine;

namespace Machines
{
    public class LaminBoxMachine : MonoBehaviour, ISaveable
    {
        private class SavedData
        {
            public bool IsOn;
            public bool IsOpen;
        }
        
        [SerializeField] private GameObject _lights;
        [SerializeField] private ButtonComponent _lightButton;
        
        [SerializeField] private ButtonComponent _openButton;

        [CanBeNull] private GameManager _gameManager;

        private bool _isOn = false;
        private bool _isOpen = false;
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
            
            _lightButton.ClickBtnEvent += OnLButtonClick;
            _openButton.ClickBtnEvent += OnOpenButtonClick;
        }

        private void OnDisable()
        {
            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.Game.LoadGameEvent += OnLoadScene;
            _gameManager.Game.SaveGameEvent += OnSaveScene;
            
            _lightButton.ClickBtnEvent -= OnLButtonClick;
            _openButton.ClickBtnEvent -= OnOpenButtonClick;
        }

        private void Start()
        {
            OnSaveScene();
        }
        

        private void OnLButtonClick()
        {
            _lights.SetActive(_lightButton.IsOn);
        }

        private void OnOpenButtonClick()
        {
            if (_isOpen)
            {
                _animator.Play("Close");
                _isOpen = false;
            }
            else
            {
                _animator.Play("Open");
                _isOpen = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_gameManager == null)
            {
                return;
            }

            if (!other.CompareTag("Key"))
            {
                return;
            }
            
            _isOn = !_isOn;
            if (_isOn)
            {
                _gameManager.Game.CompleteTask(new MachineLabActivity(EMachineActivity.OnStart, EMachine.LaminBoxMachine));
            }
        }

        public void OnSaveScene()
        {
            _savedData.IsOn = _isOn;
        }

        public void OnLoadScene()
        {
            _lightButton.SetIsOn(_savedData.IsOn);
        }
    }
}