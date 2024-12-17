using System;
using BioEngineerLab.Activities;
using Core;
using UnityEngine;
using UI.Components;
using JetBrains.Annotations;

namespace BioEngineerLab.Machines
{
    public class KeyboardMachine : MonoBehaviour
    {
        [SerializeField] private KeyButtonComponent[] _numberButtons;
        [SerializeField] private ButtonComponent _enterButton;
        [SerializeField] private Material _material;
        [SerializeField] private GameObject _handlePrefab;

        [SerializeField] private String _currentPassword;

        [CanBeNull] private GameManager _gameManager;
        private String _currentString = "";
        private bool _isOpen = false;

        private void Awake()
        {
            _material.color = Color.red;
            _gameManager = GameManager.Instance;
        }
        private void OnEnable()
        {
            foreach (var button in _numberButtons)
            {
                button.ClickBtnEvent += () => OnButtonClick(button);
            }

            _enterButton.ClickBtnEvent += OnEnterButtonClick;
        }

        private void OnDisable()
        {
            foreach (var button in _numberButtons)
            {
                button.ClickBtnEvent -= () => OnButtonClick(button);
            }

            _enterButton.ClickBtnEvent -= OnEnterButtonClick;
        }

        private void OnButtonClick(KeyButtonComponent component)
        {
              _currentString += component.Value.ToString();
        }

        private void OnEnterButtonClick()
        {
            if (_currentString == _currentPassword)
            {
                _isOpen = true;
                _handlePrefab.SetActive(true);
                _material.color = Color.green;
                
                _gameManager.Game.CompleteTask(new MachineLabActivity(EMachineActivity.OnEnter, EMachine.KeyboardMachine));
            }
            else
            {
                _isOpen = false;
                _handlePrefab.SetActive(false);
                _material.color = Color.red;
            }

            _currentString = "";
        }
    }
}
