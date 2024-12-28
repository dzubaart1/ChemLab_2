using Core;
using Saveables;
using UnityEngine;
using UI.Components;

namespace BioEngineerLab.Machines
{
    public class BoxPanelMachine : MonoBehaviour, ISaveableUI
    {
        private struct SavedData
        {
            public bool IsBactLightOn;
            public bool IsCommonLightOn;
            public bool IsDLightOn;
            public bool IsDoorOpened;
        }
        
        [SerializeField] private ButtonComponent _bacteriumButton;
        [SerializeField] private ButtonComponent _lightButton;
        [SerializeField] private ButtonComponent _dlightButton;
        [SerializeField] private ButtonComponent _keyButton;
        [SerializeField] private GameObject _bacteriumLight;
        [SerializeField] private GameObject _commonLight;
        [SerializeField] private GameObject _dLight;
        
        private SavedData _savedData = new SavedData();
        
        private bool _isBactLightOn = true;
        private bool _isCommonLightOn = false;
        private bool _isDLightOn = true;
        private bool _isDoorOpened = false;
        
        private void Start()
        {
            GameManager gameManager = GameManager.Instance;
            if (gameManager == null)
            {
                return;
            }

            if (gameManager.CurrentBaseLocalManager == null)
            {
                return;
            }
            
            gameManager.CurrentBaseLocalManager.AddSaveableUI(this);
        }
        
        private void OnEnable()
        {
            _bacteriumButton.ClickBtnEvent += OnBacteriumButtonClicked;
            _lightButton.ClickBtnEvent += OnLightButtonClicked;
            _dlightButton.ClickBtnEvent += OnDLightButtonClicked;
            _keyButton.ClickBtnEvent += OnKeyButtonClicked;
        }

        private void OnDisable()
        {
            _bacteriumButton.ClickBtnEvent -= OnBacteriumButtonClicked;
            _lightButton.ClickBtnEvent -= OnLightButtonClicked;
            _dlightButton.ClickBtnEvent -= OnDLightButtonClicked;
            _keyButton.ClickBtnEvent -= OnKeyButtonClicked;
        }

        private void OnBacteriumButtonClicked()
        {
            _isBactLightOn = !_isBactLightOn;
            _bacteriumLight.SetActive(_isBactLightOn);
        }

        private void OnLightButtonClicked()
        {
            _isCommonLightOn = true;
            _isDLightOn = false;
            
            _commonLight.SetActive(_isCommonLightOn);
            _dLight.SetActive(_isDLightOn);
        }
        
        private void OnDLightButtonClicked()
        {
            _isCommonLightOn = false;
            _isDLightOn = true;
            
            _commonLight.SetActive(_isCommonLightOn);
            _dLight.SetActive(_isDLightOn);
        }

        private void OnKeyButtonClicked()
        {
            _isDoorOpened = !_isDoorOpened;
        }

        public void SaveUIState()
        {
            _savedData.IsBactLightOn = _isBactLightOn;
            _savedData.IsCommonLightOn = _isCommonLightOn;
        }

        public void LoadUIState()
        {
            _isBactLightOn = _savedData.IsBactLightOn;
            _bacteriumLight.SetActive(_isBactLightOn);
            
            _isCommonLightOn = _savedData.IsCommonLightOn;
            _commonLight.SetActive(_isCommonLightOn);
        }
    }
}
