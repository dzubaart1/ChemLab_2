using BioEngineerLab.Activities;
using BioEngineerLab.Containers;
using BioEngineerLab.Core;
using BioEngineerLab.Gameplay;
using UnityEngine;
using BioEngineerLab.UI.Components;

namespace BioEngineerLab.Machines
{
    [RequireComponent(typeof(Collider))]
    public class CentrifugaMachine : MonoBehaviour, ISaveable
    {
        private class SavedData
        {
            public bool IsPowered;
            public bool IsStarted;
        }

        [SerializeField] private ButtonComponent _powerButton;
        [SerializeField] private ButtonComponent _startButton;
        [SerializeField] private Animator _animator;
        [SerializeField] private VRSocketInteractor _socketInteractor1;
        [SerializeField] private VRSocketInteractor _socketInteractor2;

        private SaveService _saveService;
        private TasksService _tasksService;
        private CraftService _craftService;
        
        private SavedData _savedData = new SavedData();

        private void Awake()
        {
            _tasksService = Engine.GetService<TasksService>();
            _saveService = Engine.GetService<SaveService>();
            _craftService = Engine.GetService<CraftService>();
        }

        private void OnEnable()
        {
            _saveService.LoadSceneStateEvent += OnLoadScene;
            _saveService.SaveSceneStateEvent += OnSaveScene;
            
            _startButton.ClickBtnEvent += OnStartBtnClicked;
        }

        private void OnDisable()
        {
            _saveService.LoadSceneStateEvent -= OnLoadScene;
            _saveService.SaveSceneStateEvent -= OnSaveScene;
            
            _startButton.ClickBtnEvent -= OnStartBtnClicked;
        }

        private void Start()
        {
            OnSaveScene();
        }
        
        private void OnStartBtnClicked()
        {
            if (_socketInteractor1.SelectedObject == null | _socketInteractor2.SelectedObject == null)
            {
                return;
            }
            
            LabContainer labContainer1 = _socketInteractor1.SelectedObject.GetComponent<LabContainer>();
            LabContainer labContainer2 = _socketInteractor2.SelectedObject.GetComponent<LabContainer>();

            if (labContainer1 is null || labContainer2 is null)
            {
                return;
            }
            
            if(_startButton.IsOn & _powerButton.IsOn)
            {
                _tasksService.TryCompleteTask(new MachineLabActivity(EMachineActivity.OnStart, EMachine.CentrifugaMachine));
            }
            else
            {
                _craftService.Split(labContainer1);
                _craftService.Split(labContainer2);
                
                _tasksService.TryCompleteTask(new MachineLabActivity(EMachineActivity.OnFinish, EMachine.CentrifugaMachine));
            }
            
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
        
        public void OnSaveScene()
        {
            _savedData.IsPowered = _powerButton.IsOn;
            _savedData.IsStarted = _startButton.IsOn;
        }

        public void OnLoadScene()
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