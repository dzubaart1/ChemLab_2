using System;
using BioEngineerLab.Activities;
using Core;
using UnityEngine;
using UI.Components;
using UnityEngine.Serialization;

namespace BioEngineerLab.Machines
{
    public class Keyboard : MonoBehaviour
    {
        [Header("UIs")]
        [FormerlySerializedAs("_numberButtons")]
        [SerializeField] private KeyboardKey[] _keyboardKeys;
        [SerializeField] private ButtonComponent _enterButton;
        

        [FormerlySerializedAs("_currentPassword")]
        [Space]
        [Header("Others")]
        [SerializeField] private String _targetPassword;
        
        private String _currentString = "";

        private void OnEnable()
        {
            foreach (var button in _keyboardKeys)
            {
                button.ClickKeyboardKeyEvent += OnButtonClick;
            }

            _enterButton.ClickBtnEvent += OnEnterButtonClick;
        }

        private void OnDisable()
        {
            foreach (var button in _keyboardKeys)
            {
                button.ClickKeyboardKeyEvent -= OnButtonClick;
            }

            _enterButton.ClickBtnEvent -= OnEnterButtonClick;
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
            }

            _currentString = "";
        }
    }
}
