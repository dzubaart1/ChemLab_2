using BioEngineerLab.Activities;
using Core;
using Saveables;
using UI.Components;
using UnityEngine;

namespace Machines
{
    public class LaminBoxMachine : MonoBehaviour, ISaveableUI
    {
        private class SavedData
        {
            public bool IsLight;
            public bool IsOpen;
        }
        
        [Header("Refs")]
        [SerializeField] private GameObject _light;
        [SerializeField] private Animator _animator;
        
        [Space]
        [Header("UIs")]
        [SerializeField] private ButtonComponent _lightButton;
        [SerializeField] private ButtonComponent _openButton;

        [Space]
        [Header("Configs")]
        [SerializeField] private string _openAnimatorState = "Open";
        [SerializeField] private string _closeAnimatorState = "Close";
        
        private SavedData _savedData = new SavedData();
        
        private bool _isOpen = false;
        
        private void OnEnable()
        {
            _lightButton.ClickBtnEvent += OnLButtonClick;
            _openButton.ClickBtnEvent += OnOpenButtonClick;
        }

        private void OnDisable()
        {
            _lightButton.ClickBtnEvent -= OnLButtonClick;
            _openButton.ClickBtnEvent -= OnOpenButtonClick;
        }

        private void OnLButtonClick()
        {
            _light.SetActive(_lightButton.IsOn);
        }

        private void OnOpenButtonClick()
        {
            _isOpen = !_isOpen;
            _animator.Play(_isOpen ? _openAnimatorState : _closeAnimatorState);
        }

        private void OnTriggerEnter(Collider other)
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

            if (!other.CompareTag("Key"))
            {
                return;
            }
            
            gameManager.CurrentBaseLocalManager.OnActivityComplete(new MachineLabActivity(EMachineActivity.OnStart, EMachine.LaminBoxMachine));
        }
        
        public void SaveUIState()
        {
            _savedData.IsLight = _lightButton.IsOn;
        }

        public void LoadUIState()
        {
            _lightButton.SetIsOn(_savedData.IsLight);
            _isOpen = _savedData.IsOpen;
            
            _animator.Play(_isOpen ? _openAnimatorState : _closeAnimatorState);
        }
    }
}