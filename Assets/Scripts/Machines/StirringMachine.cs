using BioEngineerLab.Activities;
using Containers;
using Core;
using Core.Services;
using Crafting;
using Mechanics;
using Saveables;
using UI.Components;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Machines
{
    public class StirringMachine : MonoBehaviour, ISaveableUI
    {
        private struct SavedData
        {
            public bool IsOnStirringBtn;
            public bool IsOnHeatingBtnState;
        }
        
        [SerializeField] private VRSocketInteractor _socketInteractor;
        [SerializeField] private ButtonComponent _stirringBtn;
        [SerializeField] private ButtonComponent _heatingBtn;
        
        private SavedData _savedData = new SavedData();
        
        private bool _isLoadEnter;
        private bool _isLoadExit;
        
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
            _socketInteractor.EnteredTransformEvent += OnEnter;
            _socketInteractor.ExitedTransformEvent += OnExit;

            _stirringBtn.ClickBtnEvent += CheckMachineStates;
            _heatingBtn.ClickBtnEvent += CheckMachineStates;
        }

        private void OnDisable()
        {
            _socketInteractor.EnteredTransformEvent -= OnEnter;
            _socketInteractor.ExitedTransformEvent -= OnExit;

            _stirringBtn.ClickBtnEvent -= CheckMachineStates;
            _heatingBtn.ClickBtnEvent -= CheckMachineStates;
        }

        private void OnEnter(Transform obj)
        {
            if (_isLoadEnter)
            {
                _isLoadEnter = false;
                return;
            }
            
            LabContainer container = obj.GetComponentInChildren<LabContainer>();
            
            if (container == null)
            {
                return;
            }

            container.ChangeContainerType(EContainer.StirringContainer);
            
            CheckAnimatorStatus();
        }

        private void OnExit(Transform obj)
        {
            if (_isLoadExit)
            {
                _isLoadExit = false;
                return;
            }
            
            LabContainer container = obj.GetComponentInChildren<LabContainer>();
            
            if (container == null)
            {
                return;
            }

            container.ChangeContainerType(EContainer.ChemicGlassContainer);
            
            CheckAnimatorStatus();
        }

        private void CheckMachineStates()
        {
            if (_socketInteractor.SelectedObject == null)
            {
                return;
            }
            
            if (!_heatingBtn.IsOn & !_stirringBtn.IsOn)
            {
                FinishMachineWork();
                return;
            }
            
            if(_heatingBtn.IsOn & _stirringBtn.IsOn)
            {
                StartMachineWork();
            }
        }

        private void CheckAnimatorStatus()
        {
            ToggleStirringAnimation(_heatingBtn.IsOn & _stirringBtn.IsOn);
        }
        
        private void StartMachineWork()
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
            
            ToggleStirringAnimation(true);
            gameManager.CurrentBaseLocalManager.OnActivityComplete(new MachineLabActivity(EMachineActivity.OnStart,
                EMachine.StirringMachine));
        }

        private void FinishMachineWork()
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

            if (_socketInteractor.SelectedObject == null)
            {
                return;
            }

            LabContainer container = _socketInteractor.SelectedObject.transform.GetComponent<LabContainer>();
            ToggleStirringAnimation(false);

            if (!CraftTools.TryFindCraft(gameManager.CurrentBaseLocalManager.GetSOCrafts(), container.GetSubstanceProperties(), ECraft.HeatStir, out SOLabCraft labCraft))
            {
                gameManager.CurrentBaseLocalManager.OnActivityComplete(new BadLabActivity());
                return;
            }
            
            CraftTools.ApplyCraft(labCraft.LabCraft, container);
            
            gameManager.CurrentBaseLocalManager.OnActivityComplete(new MachineLabActivity(EMachineActivity.OnFinish, EMachine.StirringMachine));
        }

        private void ToggleStirringAnimation(bool isEnable)
        {
            if (_socketInteractor.SelectedObject == null)
            {
                return;
            }
            
            LabContainer labContainer = _socketInteractor.SelectedObject.GetComponent<LabContainer>();
            if (labContainer == null)
            {
                return;
            }
            
            labContainer.AnimateAnchor(isEnable);
        }

        public void SaveUIState()
        {
            _savedData.IsOnHeatingBtnState = _heatingBtn.IsOn;
            _savedData.IsOnStirringBtn = _stirringBtn.IsOn;
        }

        public void LoadUIState()
        {
            _heatingBtn.SetIsOn(_savedData.IsOnHeatingBtnState);
            _stirringBtn.SetIsOn(_savedData.IsOnStirringBtn);
            
            CheckAnimatorStatus();
        }
    }
}
