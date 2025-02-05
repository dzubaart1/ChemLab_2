using System;
using System.Net.Mime;
using BioEngineerLab.Activities;
using Core;
using Machines;
using Mechanics;
using Saveables;
using UnityEngine;
using UnityEngine.UI;
using UI.Components;
using UnityEngine.Serialization;

namespace BioEngineerLab.Machines
{
    public class Keyboard : MonoBehaviour, ISaveableOther
    {
        private class SavedData
        {
            public bool IsDoorActive;
        }
        
        [Header("UIs")]
        [FormerlySerializedAs("_numberButtons")]
        [SerializeField] private KeyboardKey[] _keyboardKeys;
        [SerializeField] private ButtonComponent _enterButton;
        [SerializeField] private ButtonComponent _keyButton;
        

        [FormerlySerializedAs("_currentPassword")]
        [Space]
        [Header("Others")]
        [SerializeField] private String _targetPassword;
        [SerializeField] private Image _sygnalImage;
        [SerializeField] private VRGrabInteractable _doorInteractable;
        [SerializeField] private Door _door;
        
        private String _currentString = "";
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
            
            gameManager.CurrentBaseLocalManager.AddSaveableOther(this);
        }
        private void OnEnable()
        {
            foreach (var button in _keyboardKeys)
            {
                button.ClickKeyboardKeyEvent += OnButtonClick;
            }

            _enterButton.ClickBtnEvent += OnEnterButtonClick;
            _keyButton.ClickBtnEvent += OnKeyButtonClick;
            _door.DoorClosedEvent += OnDoorClosed;
        }

        private void OnDisable()
        {
            foreach (var button in _keyboardKeys)
            {
                button.ClickKeyboardKeyEvent -= OnButtonClick;
            }

            _enterButton.ClickBtnEvent -= OnEnterButtonClick;
            _keyButton.ClickBtnEvent -= OnKeyButtonClick;
            _door.DoorClosedEvent -= OnDoorClosed;
        }

        private void OnButtonClick(int value)
        {
            _currentString += value.ToString();
        }

        private void OnEnterButtonClick()
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
            
            if (_currentString == _targetPassword)
            {
                gameManager.CurrentBaseLocalManager.OnActivityComplete(new MachineLabActivity(EMachineActivity.OnEnter, EMachine.KeyboardMachine));
                _sygnalImage.color = Color.green;
                _doorInteractable.enabled = true;
            }
            else
            {
                _sygnalImage.color = Color.red;
            }

            _currentString = "";
        }

        private void OnKeyButtonClick()
        {
            _doorInteractable.enabled = true;
        }

        private void OnDoorClosed()
        {
            _doorInteractable.enabled = false;
        }
        
        public void Save()
        {
            _savedData.IsDoorActive = _doorInteractable.enabled;
        }

        public void Load()
        {
            _doorInteractable.enabled = _savedData.IsDoorActive;
        }
    }
}
