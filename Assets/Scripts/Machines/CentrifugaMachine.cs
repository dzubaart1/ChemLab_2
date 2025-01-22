using BioEngineerLab.Activities;
using Containers;
using Core;
using Core.Services;
using Crafting;
using Mechanics;
using Saveables;
using UI.Components;
using UnityEngine;

namespace Machines
{
    public class CentrifugaMachine : MonoBehaviour, ISaveableUI
    {
        private class SavedData
        {
            public bool IsPowered;
            public bool IsStarted;
        }

        [Header("UIs")]
        [SerializeField] private ButtonComponent _powerButton;
        [SerializeField] private ButtonComponent _startButton;
        
        [Space]
        [Header("Refs")]
        [SerializeField] private Animator _animator;
        [SerializeField] private VRSocketInteractor _socketInteractor1;
        [SerializeField] private VRSocketInteractor _socketInteractor2;
        
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
            _powerButton.ClickBtnEvent += OnPowerBtnClicked;
            _startButton.ClickBtnEvent += OnStartBtnClicked;
        }

        private void OnDisable()
        {
            _powerButton.ClickBtnEvent -= OnPowerBtnClicked;
            _startButton.ClickBtnEvent -= OnStartBtnClicked;
        }

        private void OnPowerBtnClicked()
        {
        }
        
        private void OnStartBtnClicked()
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
            
            if (_socketInteractor1.SelectedObject == null || _socketInteractor2.SelectedObject == null)
            {
                return;
            }
            
            LabContainer labContainer1 = _socketInteractor1.SelectedObject.GetComponent<LabContainer>();
            LabContainer labContainer2 = _socketInteractor2.SelectedObject.GetComponent<LabContainer>();

            if (labContainer1 is null || labContainer2 is null)
            {
                return;
            }
            
            if(_startButton.IsOn)
            {
                gameManager.CurrentBaseLocalManager.OnActivityComplete(new MachineLabActivity(EMachineActivity.OnStart, EMachine.CentrifugaMachine));
                CheckAnimatorStatus();
                return;
            }

            if (!CraftTools.TryFindCraft(gameManager.CurrentBaseLocalManager.GetSOCrafts(), labContainer1.GetSubstanceProperties(), ECraft.Split, out SOLabCraft craftContainer1))
            {
                return;
            }
            
            if (!CraftTools.TryFindCraft(gameManager.CurrentBaseLocalManager.GetSOCrafts(), labContainer2.GetSubstanceProperties(), ECraft.Split, out SOLabCraft craftContainer2))
            {
                return;
            }
            
            CraftTools.ApplyCraft(craftContainer1.LabCraft, labContainer1);
            CraftTools.ApplyCraft(craftContainer2.LabCraft, labContainer2);
        
            gameManager.CurrentBaseLocalManager.OnActivityComplete(new MachineLabActivity(EMachineActivity.OnFinish, EMachine.CentrifugaMachine));
            CheckAnimatorStatus();
        }

        private void CheckAnimatorStatus()
        {
            if (_startButton.IsOn & _powerButton.IsOn)
            {
                _animator.enabled = true;
            }
            else
            {
                _animator.enabled = false;
            }
        }
        
        public void SaveUIState()
        {
            _savedData.IsPowered = _powerButton.IsOn;
            _savedData.IsStarted = _startButton.IsOn;
        }

        public void LoadUIState()
        {
            if (_savedData.IsPowered & !_powerButton.IsOn)
            {
                _powerButton.SetIsOn(_savedData.IsPowered);
            }
            
            if (!_savedData.IsPowered & _powerButton.IsOn)
            {
                _powerButton.SetIsOn(_savedData.IsPowered);
            }

            if (_savedData.IsStarted & !_startButton.IsOn)
            {
                _startButton.SetIsOn(_savedData.IsStarted);
            }
            
            if (!_savedData.IsStarted & _startButton.IsOn)
            {
                _startButton.SetIsOn(_savedData.IsStarted);
            }
            
            CheckAnimatorStatus();
        }
    }
}