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
    public class CentrifugaContainerMachine : MonoBehaviour, ISaveable
    {
        private class SavedData
        {
            public bool IsOpen;
        }

        [SerializeField] private ButtonComponent _button;

        private SaveService _saveService;
        private TasksService _tasksService;
        private Animator _animator;

        private bool _isOpen = true;
        private SavedData _savedData = new SavedData();

        private void Awake()
        {
            _tasksService = Engine.GetService<TasksService>();
            _saveService = Engine.GetService<SaveService>();
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _saveService.LoadSceneStateEvent += OnLoadScene;
            _saveService.SaveSceneStateEvent += OnSaveScene;

            _button.OnClickButton += PlayAnimation;
        }

        private void OnDisable()
        {
            _saveService.LoadSceneStateEvent -= OnLoadScene;
            _saveService.SaveSceneStateEvent -= OnSaveScene;
            
            _button.OnClickButton -= PlayAnimation;
        }

        private void Start()
        {
            OnSaveScene();
        }

        private void PlayAnimation()
        {
            if (!_isOpen)
            {
                _animator.Play("Open");
                _isOpen = true;
                
                _tasksService.TryCompleteTask(new MachineLabActivity(EMachineActivity.OnStart, EMachine.CentrifugaContainerMachine));
            }
            else
            {
                _animator.Play("Close");
                _isOpen = false;
                
                _tasksService.TryCompleteTask(new MachineLabActivity(EMachineActivity.OnFinish, EMachine.CentrifugaContainerMachine));
            }
        }
        public void OnSaveScene()
        {
            _savedData.IsOpen = _isOpen;
        }

        public void OnLoadScene()
        {
            if (_savedData.IsOpen)
            {
                _animator.Play("Open");
            }
            else
            {
                _animator.Play("Close");
            }
        }
    }
}