using Core;
using Mechanics;
using Saveables;
using UI.Components;
using UnityEngine;

namespace Machines
{
    public class AutoClaveMachine : MonoBehaviour, ISaveableUI
    {
        private class SavedData
        {
            public bool IsPowerButtonOn;
            public bool IsPullButtonOn;
        }

        [Header("UIs")]
        [SerializeField] private ButtonComponent _powerButton;
        [SerializeField] private ButtonComponent _pullButton;

        [Header("Refs")]
        [SerializeField] private VRSocketInteractor[] _socketInteractors;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _door;
        
        private SavedData _savedData = new SavedData();
        
        private bool _isOpen = false;
        private bool _isPulled = false;

        private void OnEnable()
        {
            _pullButton.ClickBtnEvent += OnPullButtonClick;
        }

        private void OnDisable()
        {
            _pullButton.ClickBtnEvent -= OnPullButtonClick;
        }

        private void OnPullButtonClick()
        {
            if (_isPulled)
            {
                _isPulled = false;
                _animator.Play("Close");
            }
            else
            {
                _isPulled = true;
                _animator.Play("Open");
            }
        }

        public void SaveUIState()
        {
            _savedData.IsPowerButtonOn = _powerButton.IsOn;
            _savedData.IsPullButtonOn = _pullButton.IsOn;
        }

        public void LoadUIState()
        {
            _powerButton.SetIsOn(_savedData.IsPowerButtonOn);
            _pullButton.SetIsOn(_savedData.IsPullButtonOn);
        }
    }
}