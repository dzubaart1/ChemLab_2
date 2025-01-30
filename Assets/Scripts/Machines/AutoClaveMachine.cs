using Core;
using Mechanics;
using Saveables;
using UI.Components;
using UnityEngine;
using System.Collections;

namespace Machines
{
    public class AutoClaveMachine : MonoBehaviour, ISaveableUI
    {
        private class SavedData
        {
            public bool IsPowerButtonOn;
            public bool IsPullButtonOn;
            public bool AnimationParamDoorOpened;
        }

        [Header("UIs")]
        [SerializeField] private ButtonComponent _powerButton;
        [SerializeField] private ButtonComponent _pullButton;

        [Header("Refs")]
        [SerializeField] private VRSocketInteractor[] _socketInteractors;
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _karetka;
        
        private SavedData _savedData = new SavedData();
        
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
            _pullButton.ClickBtnEvent += OnPullButtonClick;
        }

        private void OnDisable()
        {
            _pullButton.ClickBtnEvent -= OnPullButtonClick;
        }

        private void OnPullButtonClick()
        {
            _animator.Play(_pullButton.IsOn ? "Open" : "Close");
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

            if (_pullButton.IsOn)
            {
                _animator.Play("Open");
            }
            else
            {
                _animator.Play("Base");
                _karetka.localPosition = new Vector3(-0.1038f, 0.1058f, 0.06f);
            }
        }
    }
}