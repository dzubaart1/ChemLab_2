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
        [SerializeField] private Door _door;
        
        private SavedData _savedData = new SavedData();
        
        private bool _animationParamDoorOpened = false;
        private bool _isPulled = false;
        
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
            _isPulled = !_isPulled;
            _animator.Play(_isPulled ? "Open" : "Close");
        }

        public void SaveUIState()
        {
            _savedData.IsPowerButtonOn = _powerButton.IsOn;
            _savedData.IsPullButtonOn = _pullButton.IsOn;
            _savedData.AnimationParamDoorOpened = _animationParamDoorOpened;
        }

        public void LoadUIState()
        {
            _powerButton.SetIsOn(_savedData.IsPowerButtonOn);
            _pullButton.SetIsOn(_savedData.IsPullButtonOn);

            _animationParamDoorOpened = _savedData.AnimationParamDoorOpened;
            if (_savedData.AnimationParamDoorOpened)
            {
                _door.transform.rotation = new Quaternion(-0.7f, 0, 0, 0.7f);
            }
            else
            {
                if (_isPulled)
                {
                    StartCoroutine(PlayCloseAnimation());
                    _isPulled = false;
                }
                else
                {
                    _door.transform.rotation = new Quaternion(-0.5f, -0.5f, -0.5f, 0.5f);
                }
            }
        }

        private IEnumerator PlayCloseAnimation()
        {
            _door.transform.rotation = new Quaternion(-0.7f, 0, 0, 0.7f);
            _animator.Play("Close");
            
            yield return new WaitForSeconds(1.0f);
            
            _door.SetIsOpen(false);
            _animationParamDoorOpened = false;
            _door.transform.rotation = new Quaternion(-0.5f, -0.5f, -0.5f, 0.5f);
        }
    }
}