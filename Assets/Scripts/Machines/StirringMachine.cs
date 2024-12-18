using System;
using BioEngineerLab.Activities;
using Containers;
using Core;
using Core.Services;
using Crafting;
using JetBrains.Annotations;
using Mechanics;
using UI.Components;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Machines
{
    public class StirringMachine : MonoBehaviour, ISaveable
    {
        private struct SavedData
        {
            public bool IsOnStirringBtn;
            public bool IsOnHeatingBtnState;
            public VRGrabInteractable Interactable;
        }
        
        [SerializeField] private VRSocketInteractor _socketInteractor;
        [SerializeField] private ButtonComponent _stirringBtn;
        [SerializeField] private ButtonComponent _heatingBtn;

        [CanBeNull] private GameManager _gameManager;
        
        private bool _isLoadEnter;
        private bool _isLoadExit;
        
        private SavedData _savedData = new SavedData();

        private void Awake()
        {
            _gameManager = GameManager.Instance;
        }

        private void Start()
        {
            OnSaveScene();
        }

        private void OnEnable()
        {
            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.Game.SaveGameEvent += OnSaveScene;
            _gameManager.Game.LoadGameEvent += OnLoadScene;

            _socketInteractor.selectEntered.AddListener(OnEnter);
            _socketInteractor.selectExited.AddListener(OnExit);

            _stirringBtn.ClickBtnEvent += CheckMachineStates;
            _heatingBtn.ClickBtnEvent += CheckMachineStates;
        }

        private void OnDisable()
        {
            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.Game.SaveGameEvent -= OnSaveScene;
            _gameManager.Game.LoadGameEvent -= OnLoadScene;

            _socketInteractor.selectEntered.RemoveListener(OnEnter);
            _socketInteractor.selectExited.RemoveListener(OnExit);

            _stirringBtn.ClickBtnEvent -= CheckMachineStates;
            _heatingBtn.ClickBtnEvent -= CheckMachineStates;
        }

        private void OnEnter(SelectEnterEventArgs args)
        {
            if (_isLoadEnter)
            {
                _isLoadEnter = false;
                return;
            }
            
            CheckAnimatorStatus();
        }

        private void OnExit(SelectExitEventArgs args)
        {
            if (_isLoadExit)
            {
                _isLoadExit = false;
                return;
            }
            
            CheckAnimatorStatus();
        }

        private void Update()
        {
            if (_socketInteractor is null)
            {
                return;
            }
            
            if (_socketInteractor.SelectedObject is null)
            {
                return;
            }
            
            LabContainer container = _socketInteractor.SelectedObject.GetComponent<LabContainer>();
            
            if (container == null)
            {
                return;
            }

            container.ChangeContainerType(EContainer.StirringContainer);
        }

        private void OnTriggerExit(Collider other)
        {
            LabContainer container = other.GetComponent<LabContainer>();
            
            if (container == null)
            {
                return;
            }

            if (container.ContainerType == EContainer.StirringContainer)
            {
                container.ChangeContainerType(EContainer.ChemicGlassContainer);
            }
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
            if (_gameManager == null)
            {
                return;
            }
            
            ToggleStirringAnimation(true);
            _gameManager.Game.CompleteTask(new MachineLabActivity(EMachineActivity.OnStart,
                EMachine.StirringMachine));
            
        }

        private void FinishMachineWork()
        {
            if (_gameManager == null)
            {
                return;
            }

            if (_socketInteractor.SelectedObject == null)
            {
                return;
            }

            LabContainer container = _socketInteractor.SelectedObject.transform.GetComponent<LabContainer>();

            if (!CraftTools.TryFindCraft(_gameManager.Game.SOCrafts, container.GetSubstanceProperties(), ECraft.HeatStir, out SOLabCraft labCraft))
            {
                _gameManager.Game.CompleteTask(new BadLabActivity());
            }
            
            CraftTools.ApplyCraft(labCraft.LabCraft, container);
            
            ToggleStirringAnimation(false);
            _gameManager.Game.CompleteTask(new MachineLabActivity(EMachineActivity.OnFinish,
                EMachine.StirringMachine));
        }

        private void ToggleStirringAnimation(bool isEnable)
        {
            if (_socketInteractor.SelectedObject == null)
            {
                return;
            }
            
            AnchorLabContainer anchorContainer = _socketInteractor.SelectedObject.GetComponent<AnchorLabContainer>();
            if (anchorContainer == null)
            {
                return;
            }
            
            anchorContainer.AnimateAnchor(isEnable);
        }

        public void OnSaveScene()
        {
            _savedData.IsOnHeatingBtnState = _heatingBtn.IsOn;
            _savedData.IsOnStirringBtn = _stirringBtn.IsOn;
            _savedData.Interactable = _socketInteractor.firstInteractableSelected as VRGrabInteractable;
        }

        public void OnLoadScene()
        {
            _heatingBtn.SetIsOn(_savedData.IsOnHeatingBtnState);
            _stirringBtn.SetIsOn(_savedData.IsOnStirringBtn);

            if (_savedData.Interactable != null & _socketInteractor.SelectedObject == null)
            {
                _isLoadEnter = true;
            }

            if (_savedData.Interactable == null & _socketInteractor.SelectedObject != null)
            {
                _isLoadExit = true;
            }
            
            CheckAnimatorStatus();
        }
    }
}
