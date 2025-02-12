using Core;
using Mechanics;
using Saveables;
using UI.Components;
using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

namespace Machines
{
    public class AutoClaveMachine : MonoBehaviour, ISaveableUI
    {
        private class SavedData
        {
            public bool IsPowerButtonOn;
            public bool IsPullButtonOn;
            public bool AnimationParamDoorOpened;
            public bool IsWorking;
        }

        [Header("UIs")]
        [SerializeField] private ButtonComponent _powerButton;
        [SerializeField] private ButtonComponent _startButton;
        [SerializeField] private ButtonComponent _pullButton;

        [Header("Refs")]
        [SerializeField] private VRSocketInteractor[] _socketInteractors;
        [SerializeField] private Animator _mainAnimator;
        [SerializeField] private Animator _arrowAnimator;
        [SerializeField] private Transform _karetka;
        [SerializeField] private Transform _arrow;
        [SerializeField] Door _door;
        
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
            _startButton.ClickBtnEvent += OnStartButtonClick;
            _door.DoorOpenedEvent += OnDoorOpened;
        }

        private void OnDisable()
        {
            _pullButton.ClickBtnEvent -= OnPullButtonClick;
            _startButton.ClickBtnEvent -= OnStartButtonClick;
            _door.DoorOpenedEvent -= OnDoorOpened;
        }

        private void OnPullButtonClick()
        {
            _mainAnimator.Play(_pullButton.IsOn ? "Open" : "Close");
        }

        private void OnStartButtonClick()
        {
            if (!_powerButton.IsOn)
            {
                _powerButton.SetIsOn(false);
                return;
            }

            if (_door.IsOpen)
            {
                _powerButton.SetIsOn(false);
                return;
            }
            
            _arrowAnimator.Play("ArrowRight");
        }
        
        private void OnDoorOpened()
        {
            if (!_startButton.IsOn)
            {
                return;
            }
            
            _startButton.SetIsOn(false);
            _arrowAnimator.Play("ArrowLeft");
        }

        public void SaveUIState()
        {
            _savedData.IsPowerButtonOn = _powerButton.IsOn;
            _savedData.IsPullButtonOn = _pullButton.IsOn;
            _savedData.IsWorking = _startButton.IsOn;
        }

        public void LoadUIState()
        {
            _powerButton.SetIsOn(_savedData.IsPowerButtonOn);
            _pullButton.SetIsOn(_savedData.IsPullButtonOn);
            _startButton.SetIsOn(_savedData.IsWorking);

            if (_pullButton.IsOn)
            {
                _mainAnimator.Play("Open");
            }
            else
            {
                _mainAnimator.Play("Base");
                _karetka.localPosition = new Vector3(-0.1038f, 0.1058f, 0.06f);
            }

            if (_startButton.IsOn)
            {
                _arrowAnimator.Play("ArrowRight");
            }
            else
            {
                _arrowAnimator.Play("Base");
                _arrow.localRotation = Quaternion.Euler(0, 0, -4);
            }
        }
    }
}