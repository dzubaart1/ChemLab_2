using System;
using Containers;
using Core;
using Gameplay;
using TMPro;
using UnityEngine;
using Mechanics;
using UI.Components;
using Unity.VisualScripting;
using UnityEngine.Serialization;
using UnityEngine.UI;


namespace BioEngineerLab.Machines
{
    public class EnterDoorMachine : MonoBehaviour
    {
        [SerializeField] private ButtonComponent[] _numberButtons;
        [SerializeField] private ButtonComponent _enterButton;
        [SerializeField] private Material _material;
        [SerializeField] private GameObject _handlePrefab;

        private const String _passwordString = "123";
        private String _currentString = "";

        private bool _isOpen = false;

        private void Awake()
        {
            _material.color = Color.red;
        }
        private void OnEnable()
        {
            for (int i = 0; i < _numberButtons.Length; i++)
            {
                switch (i)
                {
                    case 0:
                    {
                        _numberButtons[i].ClickBtnEvent += On1ButtonClick;
                        break;
                    }
                    case 1:
                    {
                        _numberButtons[i].ClickBtnEvent += On2ButtonClick;
                        break;
                    }
                    case 2:
                    {
                        _numberButtons[i].ClickBtnEvent += On3ButtonClick;
                        break;
                    }
                    case 3:
                    {
                        _numberButtons[i].ClickBtnEvent += On4ButtonClick;
                        break;
                    }
                    case 4:
                    {
                        _numberButtons[i].ClickBtnEvent += On5ButtonClick;
                        break;
                    }
                    case 5:
                    {
                        _numberButtons[i].ClickBtnEvent += On6ButtonClick;
                        break;
                    }
                    case 6:
                    {
                        _numberButtons[i].ClickBtnEvent += On7ButtonClick;
                        break;
                    }
                    case 7:
                    {
                        _numberButtons[i].ClickBtnEvent += On8ButtonClick;
                        break;
                    }
                    case 8:
                    {
                        _numberButtons[i].ClickBtnEvent += On9ButtonClick;
                        break;
                    }
                }
            }

            _enterButton.ClickBtnEvent += OnEnterButtonClick;
        }

        private void OnDisable()
        {
            for (int i = 0; i < _numberButtons.Length; i++)
            {
                switch (i)
                {
                    case 0:
                    {
                        _numberButtons[i].ClickBtnEvent -= On1ButtonClick;
                        break;
                    }
                    case 1:
                    {
                        _numberButtons[i].ClickBtnEvent -= On2ButtonClick;
                        break;
                    }
                    case 2:
                    {
                        _numberButtons[i].ClickBtnEvent -= On3ButtonClick;
                        break;
                    }
                    case 3:
                    {
                        _numberButtons[i].ClickBtnEvent -= On4ButtonClick;
                        break;
                    }
                    case 4:
                    {
                        _numberButtons[i].ClickBtnEvent -= On5ButtonClick;
                        break;
                    }
                    case 5:
                    {
                        _numberButtons[i].ClickBtnEvent -= On6ButtonClick;
                        break;
                    }
                    case 6:
                    {
                        _numberButtons[i].ClickBtnEvent -= On7ButtonClick;
                        break;
                    }
                    case 7:
                    {
                        _numberButtons[i].ClickBtnEvent -= On8ButtonClick;
                        break;
                    }
                    case 8:
                    {
                        _numberButtons[i].ClickBtnEvent -= On9ButtonClick;
                        break;
                    }
                }
            }

            _enterButton.ClickBtnEvent -= OnEnterButtonClick;
        }

        private void OnButtonClick(int i)
        {
            _currentString += i.ToString();
        }

        private void On1ButtonClick()
        {
            _currentString += 1.ToString();
        }
        
        private void On2ButtonClick()
        {
            _currentString += 2.ToString();
        }

        private void On3ButtonClick()
        {
            _currentString += 3.ToString();
        }

        private void On4ButtonClick()
        {
            _currentString += 4.ToString();
        }

        private void On5ButtonClick()
        {
            _currentString += 5.ToString();
        }

        private void On6ButtonClick()
        {
            _currentString += 6.ToString();
        }

        private void On7ButtonClick()
        {
            _currentString += 7.ToString();
        }

        private void On8ButtonClick()
        {
            _currentString += 8.ToString();
        }

        private void On9ButtonClick()
        {
            _currentString += 9.ToString();
        }

        private void OnEnterButtonClick()
        {
            if (_currentString == _passwordString)
            {
                _isOpen = true;
                _handlePrefab.SetActive(true);
                _material.color = Color.green;
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
