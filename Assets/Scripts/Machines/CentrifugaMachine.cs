using System;
using System.Collections.Generic;
using BioEngineerLab.Activities;
using BioEngineerLab.Containers;
using BioEngineerLab.Core;
using BioEngineerLab.Gameplay;
using UnityEngine;
using UnityEngine.UIElements;
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

        private bool _isPowered = false;
        private bool _isStarted = false;
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

            _powerButton.OnClickButton += PowerCentrifuga;
            _startButton.OnClickButton += StartCentrifuga;
        }

        private void OnDisable()
        {
            _saveService.LoadSceneStateEvent -= OnLoadScene;
            _saveService.SaveSceneStateEvent -= OnSaveScene;
            
            _powerButton.OnClickButton -= PowerCentrifuga;
            _startButton.OnClickButton -= StartCentrifuga;
        }

        private void Start()
        {
            OnSaveScene();
        }

        private void PowerCentrifuga()
        {
            _isPowered = !_isPowered;
        }

        private void StartCentrifuga()
        {
            if (!_isPowered)
            {
                return;
            }

            if (_isStarted)
            {
                _isStarted = false;
                _animator.enabled = false;
                
                _craftService.Split(_socketInteractor1.SelectedObject.GetComponent<LabContainer>());
                _craftService.Split(_socketInteractor2.SelectedObject.GetComponent<LabContainer>());
                
                _tasksService.TryCompleteTask(new MachineLabActivity(EMachineActivity.OnFinish, EMachine.CentrifugaMachine));
            }
            else
            {
                _isStarted = true;
                _animator.enabled = true;
                
                _tasksService.TryCompleteTask(new MachineLabActivity(EMachineActivity.OnStart, EMachine.CentrifugaMachine));
            }
        }
        public void OnSaveScene()
        {
            _savedData.IsPowered = _isPowered;
            _savedData.IsStarted = _isStarted;
        }

        public void OnLoadScene()
        {
            if (!_savedData.IsPowered)
            {
                return;
            }

            if (_savedData.IsStarted)
            {
                _animator.enabled = true;
            }
        }
    }
}