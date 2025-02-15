using System;
using System.Net.Mime;
using BioEngineerLab.Activities;
using Core;
using UnityEngine;
using UnityEngine.UI;
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
        [SerializeField] private Image _sygnalImage;
        
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
                _sygnalImage.color = Color.green;
            }
            else
            {
                _sygnalImage.color = Color.red;
            }

            _currentString = "";
        }
    }
}
