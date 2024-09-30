using System;
using BioEngineerLab.Activities;
using BioEngineerLab.Core;
using BioEngineerLab.Gameplay;
using TMPro;
using UnityEngine;

namespace BioEngineerLab.UI.Components
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class DropdownComponent : MonoBehaviour, ISaveable
    {
        private struct SavedData
        {
            public int DropDownValue;
        }
        
        public DropdownType DropdownType;
        
        private TMP_Dropdown _dropdown;
        private SavedData _savedData;
        private bool _isLoadSceneValue;

        private TasksService _tasksService;
        private SaveService _saveService;

        private void Awake()
        {
            _tasksService = Engine.GetService<TasksService>();
            _saveService = Engine.GetService<SaveService>();
            _saveService.SaveSceneStateEvent += OnSaveScene;
            _saveService.LoadSceneStateEvent += OnLoadScene;

            _savedData = new SavedData();
            
            _dropdown = GetComponent<TMP_Dropdown>();
            _dropdown.onValueChanged.AddListener(OnSelectDropdownValue);
        }

        private void Start()
        {
            OnSaveScene();
        }

        private void OnDestroy()
        {
            _dropdown.onValueChanged.RemoveListener(OnSelectDropdownValue);
        }

        private void OnSelectDropdownValue(int value)
        {
            if (_isLoadSceneValue)
            {
                _isLoadSceneValue = false;
                return;
            }
            
            _tasksService.TryCompleteTask(new DropdownActivity(DropdownType, value));
        }

        public void OnSaveScene()
        {
            _savedData.DropDownValue = _dropdown.value;
        }

        public void OnLoadScene()
        {
            if (_dropdown.value == _savedData.DropDownValue)
            {
                return;
            }
            
            _isLoadSceneValue = true;
            _dropdown.value = _savedData.DropDownValue;
        }
    }
}