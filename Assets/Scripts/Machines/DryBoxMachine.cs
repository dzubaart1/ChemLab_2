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
    public class DryBoxMachine : MonoBehaviour, ISaveable
    {
        private class SavedData
        {
            public bool IsOn;
            public bool IsOpen;
        }

        [SerializeField] private ButtonComponent _button;
        [SerializeField] private VRSocketInteractor _socketInteractor;
        [SerializeField] private Transform _door;

        private SaveService _saveService;
        private TasksService _tasksService;
        private CraftService _craftService;

        private bool _isOn = false;
        private bool _isOpen = false;
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

            _button.OnClickButton += Dry;
        }

        private void OnDisable()
        {
            _saveService.LoadSceneStateEvent -= OnLoadScene;
            _saveService.SaveSceneStateEvent -= OnSaveScene;
            
            _button.OnClickButton -= Dry;
        }

        private void Start()
        {
            OnSaveScene();
        }

        private void Update()
        {
            if (_door.transform.rotation.z < 0.45f && !_isOpen)
            {
                _isOpen = true;
            }
            else if (_door.transform.rotation.z > 0.45f && _isOpen)
            {
                _isOpen = false;
                
                Debug.Log("Closed");
            }
        }

        private void Dry()
        {
            if (_isOn)
            {
                LabContainer labContainer = _socketInteractor.SelectedObject.GetComponent<LabContainer>();

                if (labContainer is null)
                {
                    return;
                }

                _craftService.Dry(labContainer);
                
                _isOn = false;
            }
            else
            {
                _isOn = true;
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