using System;
using BioEngineerLab.Activities;
using Core;
using UnityEngine;
using UI.Components;
using JetBrains.Annotations;

namespace BioEngineerLab.Machines
{
    public class BoxPanelMachine : MonoBehaviour, ISaveable
    {
        private struct SavedData
        {
            public bool IsBactLightOn;
            public bool IsCommonLightOn;
        }
        
        [SerializeField] private ButtonComponent _bacteriumButton;
        [SerializeField] private ButtonComponent _lightButton;
        [SerializeField] private GameObject _bacteriumLight;
        [SerializeField] private GameObject _commonLight;
        
        private GameManager _gameManager;
        private SavedData _savedData = new SavedData();
        private bool _isBactLightOn = true;
        private bool _isCommonLightOn = false;
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
            
            _gameManager.Game.SaveGameEvent += OnSaveScene;
            _gameManager.Game.LoadGameEvent += OnLoadScene;
            
            _bacteriumButton.ClickBtnEvent += OnBacteriumButtonClicked;
            _lightButton.ClickBtnEvent += OnLightButtonClicked;
        }

        private void OnDisable()
        {
            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.Game.SaveGameEvent -= OnSaveScene;
            _gameManager.Game.LoadGameEvent -= OnLoadScene;
            
            _bacteriumButton.ClickBtnEvent -= OnBacteriumButtonClicked;
            _lightButton.ClickBtnEvent -= OnLightButtonClicked;
        }

        private void OnBacteriumButtonClicked()
        {
            _isBactLightOn = !_isBactLightOn;
            _bacteriumLight.SetActive(_isBactLightOn);
        }

        private void OnLightButtonClicked()
        {
            _isCommonLightOn = !_isCommonLightOn;
            _commonLight.SetActive(_isCommonLightOn);
        }

        public void OnSaveScene()
        {
            _savedData.IsBactLightOn = _isBactLightOn;
            _savedData.IsCommonLightOn = _isCommonLightOn;
        }
        
        public void OnLoadScene()
        {
            _isBactLightOn = _savedData.IsBactLightOn;
            _bacteriumLight.SetActive(_isBactLightOn);
            
            _isCommonLightOn = _savedData.IsCommonLightOn;
            _commonLight.SetActive(_isCommonLightOn);
        }
    }
}
