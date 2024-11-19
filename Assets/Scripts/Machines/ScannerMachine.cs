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
    public class ScannerMachine : MonoBehaviour, ISaveable
    {
        private class SavedData
        {
            public bool IsOn;
            public bool IsStarted;
        }

        [SerializeField] private ButtonComponent _button;
        [SerializeField] private Animator _animator;
        [SerializeField] private GameObject _canvas;
        [SerializeField] private VRSocketInteractor _socketInteractor;

        private SaveService _saveService;
        private TasksService _tasksService;
        private CraftService _craftService;

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

            _button.OnClickButton += ScannerOn;
        }

        private void OnDisable()
        {
            _saveService.LoadSceneStateEvent -= OnLoadScene;
            _saveService.SaveSceneStateEvent -= OnSaveScene;
            
            _button.OnClickButton -= ScannerOn;
        }

        private void Start()
        {
            OnSaveScene();
        }

        private void ScannerOn()
        {
            if (_socketInteractor.SelectedObject is null)
            {
                return;
            }
            
            _isOn = !_isOn;

            if (_isOn)
            {
                _animator.Play("ButtonOn");
                _canvas.SetActive(true);
            }
            else
            {
                _animator.Play("ButtonOff");
                _canvas.SetActive(false);
            }
        }
        public void OnSaveScene()
        {
            _savedData.IsOn = _isOn;
        }

        public void OnLoadScene()
        {
            if (_savedData.IsOn)
            {
                _animator.Play("ButtonOn");
            }
            else
            {
                _animator.Play("ButtonOff");
            }
        }
    }
}