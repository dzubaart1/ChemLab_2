using System;
using BioEngineerLab.Activities;
using BioEngineerLab.Containers;
using BioEngineerLab.Core;
using BioEngineerLab.Gameplay;
using UI.Components;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace BioEngineerLab.Machines
{
    public class StirringMachine : MonoBehaviour, ISaveable
    {
        private struct SavedData
        {
            public bool IsStart, IsFinish;
            public bool StirringBtnState, HeatingBtnState;
            public VRGrabInteractable Interactable;
        }
        
        [SerializeField] private VRSocketInteractor _socketInteractor;
        [SerializeField] private StirringMachineButtonComponent _stirringBtn;
        [SerializeField] private StirringMachineButtonComponent _heatingBtn;

        private bool _isStart = false, _isFinish = true;
        private bool _isLoadEnter = false, _isLoadFinish = false, _isLoadStart = false;
        private SavedData _savedData;

        private TasksService _tasksService;
        private SaveService _saveService;
        private SubstancesService _substancesService;

        private void Awake()
        {
            _substancesService = Engine.GetService<SubstancesService>();
            _tasksService = Engine.GetService<TasksService>();
            
            _saveService = Engine.GetService<SaveService>();
            _saveService.SaveSceneStateEvent += OnSaveScene;
            _saveService.LoadSceneStateEvent += OnLoadScene;
            
            _socketInteractor.selectEntered.AddListener(OnEnter);
            _socketInteractor.selectExited.AddListener(OnExit);

            _stirringBtn.OnClickButton += CheckMachineStates;
            _heatingBtn.OnClickButton += CheckMachineStates;

            _savedData = new SavedData();
        }

        private void Start()
        {
            OnSaveScene();
        }

        private void OnDestroy()
        {
            _socketInteractor.selectEntered.RemoveListener(OnEnter);
            _socketInteractor.selectExited.RemoveListener(OnExit);
            
            _stirringBtn.OnClickButton -= CheckMachineStates;
            _heatingBtn.OnClickButton -= CheckMachineStates;
        }

        private void OnEnter(SelectEnterEventArgs args)
        {
            if (_isLoadEnter)
            {
                _isLoadEnter = false;
                return;
            }
            
            _tasksService.TryCompleteTask(new MachineActivity(MachineActivityType.OnEnter, MachineType.StirringMachine));
            CheckMachineStates();
        }

        private void OnExit(SelectExitEventArgs args)
        {
            _tasksService.TryCompleteTask(new MachineActivity(MachineActivityType.OnExit, MachineType.StirringMachine));
        }
        
        private void CheckMachineStates()
        {
            if (_socketInteractor.firstInteractableSelected == null)
            {
                return;
            }
            
            if (_isStart & !_isFinish & (!_heatingBtn.IsOn & !_stirringBtn.IsOn))
            {
                FinishMachineWork();
                return;
            }
            
            if(_isFinish & !_isStart & _heatingBtn.IsOn & _stirringBtn.IsOn)
            {
                StartMachineWork();
            }
        }
        
        private void StartMachineWork()
        {
            _isStart = true;
            _isFinish = false;

            ToggleStirringAnimation(true);

            if (!_isLoadStart)
            {
                _tasksService.TryCompleteTask(new MachineActivity(MachineActivityType.OnStart,
                    MachineType.StirringMachine));
            }
        }

        private void FinishMachineWork()
        {
            _isStart = false;
            _isFinish = true;
            
            _substancesService.HeatStir(_socketInteractor.firstInteractableSelected.transform.GetComponent<Container>());
            ToggleStirringAnimation(false);

            if (!_isLoadFinish)
            {
                _tasksService.TryCompleteTask(new MachineActivity(MachineActivityType.OnFinish,
                    MachineType.StirringMachine));
            }
        }

        private void ToggleStirringAnimation(bool isEnable)
        {
            AnchorContainer anchorContainer = _socketInteractor.firstInteractableSelected.transform.GetComponent<AnchorContainer>();
            anchorContainer.Anchor.ToggleAnimate(isEnable);
        }

        public void OnSaveScene()
        {
            _savedData.IsFinish = _isFinish;
            _savedData.IsStart = _isStart;
            _savedData.HeatingBtnState = _heatingBtn.IsOn;
            _savedData.StirringBtnState = _stirringBtn.IsOn;
            _savedData.Interactable = _socketInteractor.firstInteractableSelected as VRGrabInteractable;
        }

        public void OnLoadScene()
        {
            _isFinish = _savedData.IsFinish;
            _isStart = _savedData.IsStart;
            _heatingBtn.OnLoadScene(_savedData.HeatingBtnState);
            _stirringBtn.OnLoadScene(_savedData.StirringBtnState);

            if (_savedData.Interactable != null)
            {
                _isLoadEnter = true;
            }
            
            _isLoadFinish = true;
            _isLoadStart = true;
            
            CheckMachineStates();
            
            _isLoadFinish = false;
            _isLoadStart = false;
        }
    }
}
