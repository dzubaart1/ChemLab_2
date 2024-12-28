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

        [Space]
        [Header("Views")]
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private Material _errorMaterial;
        [SerializeField] private Material _successMaterial;
        
        [Space]
        [Header("Refs")]
        [SerializeField] private GameObject _handlePrefab;

        [FormerlySerializedAs("_currentPassword")]
        [Space]
        [Header("Others")]
        [SerializeField] private String _targetPassword;
        
        private String _currentString = "";

        private void Start()
        {
            _meshRenderer.material = _errorMaterial;
        }

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
                _handlePrefab.SetActive(true);
                _meshRenderer.material = _successMaterial;
                
                gameManager.CurrentBaseLocalManager.OnActivityComplete(new MachineLabActivity(EMachineActivity.OnEnter, EMachine.KeyboardMachine));
            }
            else
            {
                _handlePrefab.SetActive(false);
                _meshRenderer.material = _errorMaterial;
            }

            _currentString = "";
        }
    }
}
