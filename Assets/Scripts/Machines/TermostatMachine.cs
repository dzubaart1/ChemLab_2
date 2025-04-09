using System;
using BioEngineerLab.Activities;
using Containers;
using Core;
using Core.Services;
using Crafting;
using Mechanics;
using Saveables;
using TMPro;
using UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace Machines
{
    public class TermostatMachine : MonoBehaviour, ISaveableUI
    {
        private class SavedData
        {
            public bool IsPower;
            public bool IsHeating;
        }

        [Header("UIs")]
        [SerializeField] private ButtonComponent _powerButton;
        [SerializeField] private ButtonComponent _UpButton;
        [SerializeField] private ButtonComponent _PButton;
        [SerializeField] private TextMeshProUGUI _text;
        
        [Header("Refs")]
        [SerializeField] private VRSocketInteractor _socketInteractor1;
        [SerializeField] private VRSocketInteractor _socketInteractor2;
        [SerializeField] private Door _door;
        [SerializeField] private Light _heatingLight;
        
        private SavedData _savedData = new SavedData();
        private float _timer = 2.0f;
        private float _temperature;
        private bool _isHeating;
        
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
            _text.text = "";
        }
        
        private void OnEnable()
        {
            _door.DoorClosedEvent += OnDoorClosed;
            
            _powerButton.ClickBtnEvent += OnPowerButtonClick;
            _UpButton.ClickBtnEvent += OnUpButtonClick;
            _PButton.ClickBtnEvent += OnPButtonClick;
        }

        private void OnDisable()
        {
            _door.DoorClosedEvent -= OnDoorClosed;
            
            _powerButton.ClickBtnEvent -= OnPowerButtonClick;
            _UpButton.ClickBtnEvent -= OnUpButtonClick;
            _PButton.ClickBtnEvent -= OnPButtonClick;
        }

        private void Update()
        {
            if (_isHeating)
            {
                _timer -= Time.deltaTime;

                if (_timer <= 0)
                {
                    _temperature += 0.5f;
                    _text.text = _temperature.ToString("F1");
                    _timer = 2.0f;
                }

                if (_temperature >= 37.0f)
                {
                    _isHeating = false;
                    _heatingLight.enabled = false;
                }
            }
        }

        private void OnPowerButtonClick()
        {
            if (_powerButton.IsOn)
            {
                _temperature = 22.5f;
                _text.text = _temperature.ToString("F1");
                _heatingLight.enabled = true;
                _isHeating = true;
            }
            else
            {
                _text.text = "";
                _heatingLight.enabled = false;
                _isHeating = false;
            }
        }

        private void OnPButtonClick()
        {
            _isHeating = true;
        }

        private void OnUpButtonClick()
        {
            _text.text = "37,0";
            _isHeating = false;
        }

        private void OnDoorClosed()
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
            
            if (!_powerButton.IsOn)
            {
                return;
            }
            
            if (_socketInteractor1.SelectedObject == null)
            {
                return;
            }
            
            if (_socketInteractor2.SelectedObject == null)
            {
                return;
            }
            
            LabContainer container1 = _socketInteractor1.SelectedObject.GetComponent<LabContainer>();
            LabContainer container2 = _socketInteractor2.SelectedObject.GetComponent<LabContainer>();

            if (container1 is null || container2 == null)
            {
                return;
            }
            
            if (!CraftTools.TryFindCraft(gameManager.CurrentBaseLocalManager.GetSOCrafts(), container1.GetSubstanceProperties(), ECraft.Dry, out SOLabCraft craftContainer1))
            {
                return;
            }
            
            if (!CraftTools.TryFindCraft(gameManager.CurrentBaseLocalManager.GetSOCrafts(), container2.GetSubstanceProperties(), ECraft.Dry, out SOLabCraft craftContainer2))
            {
                return;
            }
            
            CraftTools.ApplyCraft(craftContainer1.LabCraft, container1);
            CraftTools.ApplyCraft(craftContainer2.LabCraft, container2);
        }
        
        public void SaveUIState()
        {
            _savedData.IsPower = _powerButton.IsOn;
            _savedData.IsHeating = _isHeating;
        }

        public void LoadUIState()
        {
            _powerButton.SetIsOn(_savedData.IsPower);
            _text.text = _powerButton.IsOn ? _temperature.ToString("F1") : "";
            
            _isHeating = _savedData.IsHeating;
            _heatingLight.enabled = _isHeating;
        }
    }
}