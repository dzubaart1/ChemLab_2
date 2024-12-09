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
        }
        
        [SerializeField] private GameObject _lights;
        [SerializeField] private ButtonComponent _lightButton;

        [CanBeNull] private GameManager _gameManager;

        private bool _isOn = false;
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
            
            _lightButton.ClickBtnEvent += OnLButtonClick;
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
        }

        private void Start()
        {
            OnSaveScene();
        }
        

        private void OnLButtonClick()
        {
            _lights.SetActive(_lightButton.IsOn);
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