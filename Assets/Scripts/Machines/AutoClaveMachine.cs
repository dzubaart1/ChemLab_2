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
            public bool IsOpened;
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
            _savedData.IsOpened = _isOpen;
        }

        public void LoadUIState()
        {
            _powerButton.SetIsOn(_savedData.IsPowerButtonOn);
            _pullButton.SetIsOn(_savedData.IsPullButtonOn);
            
            if (_savedData.IsOpened)
            {
                _isOpen = true;
                _door.transform.rotation = new Quaternion(-0.7f, 0, 0, 0.7f);
            }
            else
            {
                _isOpen = false;
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
            _door.GetComponent<Door>().SetIsOpen(false);
            _isOpen = false;
            _door.transform.rotation = new Quaternion(-0.5f, -0.5f, -0.5f, 0.5f);
        }
    }
}