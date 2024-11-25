using Activities;
using Containers;
using Core;
using Core.Services;
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

        private bool _isLoadEnter;
        private bool _isLoadExit;
        
        private SavedData _savedData = new SavedData();

        private TasksService _tasksService;
        private SaveService _saveService;
        private CraftService _craftService;

        private void Awake()
        {
            _craftService = Engine.GetService<CraftService>();
            _tasksService = Engine.GetService<TasksService>();
            _saveService = Engine.GetService<SaveService>();
        }

        private void Start()
        {
            OnSaveScene();
        }

        private void OnEnable()
        {
            _saveService.SaveSceneStateEvent += OnSaveScene;
            _saveService.LoadSceneStateEvent += OnLoadScene;

            _socketInteractor.selectEntered.AddListener(OnEnter);
            _socketInteractor.selectExited.AddListener(OnExit);

            _stirringBtn.ClickBtnEvent += CheckMachineStates;
            _heatingBtn.ClickBtnEvent += CheckMachineStates;
        }

        private void OnDisable()
        {
            _saveService.SaveSceneStateEvent -= OnSaveScene;
            _saveService.LoadSceneStateEvent -= OnLoadScene;

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
            ToggleStirringAnimation(true);
            _tasksService.TryCompleteTask(new MachineLabActivity(EMachineActivity.OnStart,
                EMachine.StirringMachine));
            
        }

        private void FinishMachineWork()
        {
            _craftService.HeatStir(_socketInteractor.firstInteractableSelected.transform.GetComponent<LabContainer>());
            ToggleStirringAnimation(false);
            
            _tasksService.TryCompleteTask(new MachineLabActivity(EMachineActivity.OnFinish,
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
