using System;
using Activities;
using BioEngineerLab.Activities;
using Containers;
using Core;
using Core.Services;
using Mechanics;
using UI.Components;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

namespace Machines
{
    public class LaminBoxMachine : MonoBehaviour, ISaveable
    {
        private class SavedData
        {
            public bool IsOn;
        }

        private SaveService _saveService;
        private TasksService _tasksService;
        private CraftService _craftService;

        [SerializeField] private GameObject _lights;
        [SerializeField] private ButtonComponent _lightButton;

        private bool _isOn = false;
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
            
            _lightButton.ClickBtnEvent += OnLButtonClick;
        }

        private void OnDisable()
        {
            _saveService.LoadSceneStateEvent -= OnLoadScene;
            _saveService.SaveSceneStateEvent -= OnSaveScene;
            
            _lightButton.ClickBtnEvent -= OnLButtonClick;
        }

        private void Start()
        {
            OnSaveScene();
        }

        private void Update()
        {
            
        }

        private void OnLButtonClick()
        {
            _lights.SetActive(_lightButton.IsOn);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Key"))
            {
                _isOn = !_isOn;
                if (_isOn)
                {
                    _tasksService.TryCompleteTask(new MachineLabActivity(EMachineActivity.OnStart, EMachine.LaminBoxMachine));
                }
            }
        }

        public void OnSaveScene()
        {
            _savedData.IsOn = _isOn;
        }

        public void OnLoadScene()
        {
            
        }
    }
}