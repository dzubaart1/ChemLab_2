using BioEngineerLab.Activities;
using BioEngineerLab.Containers;
using BioEngineerLab.Core;
using BioEngineerLab.Gameplay;
using BioEngineerLab.UI.Components;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace BioEngineerLab.Machines
{
    public class StirringMachine : MonoBehaviour, ISaveable
    {
        private struct SavedData
        {
            public bool IsStart;
            public bool IsFinish;
            public bool IsOnStirringBtn;
            public bool IsOnHeatingBtnState;
            public VRGrabInteractable Interactable;
        }
        
        [SerializeField] private VRSocketInteractor _socketInteractor;
        [SerializeField] private ButtonComponent _stirringBtn;
        [SerializeField] private ButtonComponent _heatingBtn;

        private bool _isStart = false;
        private bool _isFinish = true;
        private bool _isLoadEnter = false;
        private bool _isLoadFinish = false;
        private bool _isLoadStart = false;
        
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

            _stirringBtn.OnClickButton += CheckMachineStates;
            _heatingBtn.OnClickButton += CheckMachineStates;
        }

        private void OnDisable()
        {
            _saveService.SaveSceneStateEvent -= OnSaveScene;
            _saveService.LoadSceneStateEvent -= OnLoadScene;

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
            
            CheckMachineStates();
        }

        private void OnExit(SelectExitEventArgs args)
        {
            //TODO: Is Load Exit
            
            CheckMachineStates();
        }

        private void CheckMachineStates()
        {
            if (_socketInteractor.SelectedObject == null)
            {
                return;
            }
            
            if (_isStart & !_isFinish & (!_heatingBtn.IsOn & !_stirringBtn.IsOn))
            {
                FinishMachineWork();
                return;
            }
            
            if(_isFinish & !_isStart & (_heatingBtn.IsOn & _stirringBtn.IsOn))
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
                _tasksService.TryCompleteTask(new MachineLabActivity(EMachineActivity.OnStart,
                    EMachine.StirringMachine));
            }
        }

        private void FinishMachineWork()
        {
            _isStart = false;
            _isFinish = true;
            
            _craftService.HeatStir(_socketInteractor.firstInteractableSelected.transform.GetComponent<LabContainer>());
            ToggleStirringAnimation(false);

            if (!_isLoadFinish)
            {
                _tasksService.TryCompleteTask(new MachineLabActivity(EMachineActivity.OnFinish,
                    EMachine.StirringMachine));
            }
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
            _savedData.IsFinish = _isFinish;
            _savedData.IsStart = _isStart;
            _savedData.IsOnHeatingBtnState = _heatingBtn.IsOn;
            _savedData.IsOnStirringBtn = _stirringBtn.IsOn;
            _savedData.Interactable = _socketInteractor.firstInteractableSelected as VRGrabInteractable;
        }

        public void OnLoadScene()
        {
            _isFinish = _savedData.IsFinish;
            _isStart = _savedData.IsStart;
            _heatingBtn.SetIsOn(_savedData.IsOnHeatingBtnState);
            _stirringBtn.SetIsOn(_savedData.IsOnStirringBtn);

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
