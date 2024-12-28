using Mechanics;
using Saveables;
using UI.Components;
using UnityEngine;
using UnityEngine.Serialization;

namespace Machines
{
    [RequireComponent(typeof(Collider))]
    public class ScannerMachine : MonoBehaviour, ISaveableUI
    {
        private class SavedData
        {
            public bool IsStarted;
        }
        
        [Header("UIs")]
        [SerializeField] private ButtonComponent _scannerActivateButton;
        [SerializeField] private GameObject _canvas;
        
        [Header("Refs")]
        [SerializeField] private Animator _animator;
        [SerializeField] private VRSocketInteractor _socketInteractor;
        [SerializeField] private Transform _cup;
        
        private SavedData _savedData = new SavedData();

        private bool _isOpen;
        
        private void Update()
        {
            if (_cup.transform.rotation.x < -0.1)
            {
                _isOpen = true;
            }
            else
            {
                _isOpen = false;
            }

            if (!_isOpen)
            {
                _canvas.SetActive(_scannerActivateButton.IsOn);
            }
            else
            {
                _canvas.SetActive(false);
            }
        }

        private void OnEnable()
        {
            _scannerActivateButton.ClickBtnEvent += OnScannerBtnClicked;
        }

        private void OnDisable()
        {
            _scannerActivateButton.ClickBtnEvent -= OnScannerBtnClicked;
        }

        private void OnScannerBtnClicked()
        {
            if (_socketInteractor.SelectedObject is null)
            {
                return;
            }
            
            _animator.Play(_scannerActivateButton.IsOn ? "ButtonOn" : "ButtonOff");
        }

        public void SaveUIState()
        {
            _savedData.IsStarted = _scannerActivateButton.IsOn;
        }

        public void LoadUIState()
        {
            if (!_savedData.IsStarted & _scannerActivateButton.IsOn)
            {
                _scannerActivateButton.SetIsOn(_savedData.IsStarted);
                _animator.Play(_scannerActivateButton.IsOn ? "ButtonOn" : "ButtonOff");
            }
            
            if (_savedData.IsStarted & !_scannerActivateButton.IsOn)
            {
                _scannerActivateButton.SetIsOn(_savedData.IsStarted);
                _animator.Play(_scannerActivateButton.IsOn ? "ButtonOn" : "ButtonOff");
            }
        }
    }
}