using System.Collections.Generic;
using BioEngineerLab.Activities;
using Containers;
using Core;
using JetBrains.Annotations;
using Mechanics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Machines
{
    public class SelectVanishMachine : MonoBehaviour, ISaveable
    {
        private class SavedData
        {
            public bool IsActive = true;
        }

        [SerializeField] private EMachine _machineType;
        [SerializeField] private GameObject _vanishPrefab;
        [SerializeField] private VRGrabInteractable _grabInteractable;

        [CanBeNull] private GameManager _gameManager;
        private SavedData _savedData = new SavedData();
        private bool _isActive = true;

        private void Awake()
        {
            _gameManager = GameManager.Instance;
        }

        private void OnEnable()
        {
            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.Game.LoadGameEvent += OnLoadScene;
            _gameManager.Game.SaveGameEvent += OnSaveScene;

            _grabInteractable.selectEntered.AddListener(OnSelectEntered);
        }

        private void OnDisable()
        {
            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.Game.LoadGameEvent -= OnLoadScene;
            _gameManager.Game.SaveGameEvent -= OnSaveScene;
            
            _grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
        }

        private void OnSelectEntered(SelectEnterEventArgs args)
        {
            _isActive = false;
            _vanishPrefab.SetActive(_isActive);
            
            _gameManager.Game.CompleteTask(new MachineLabActivity(EMachineActivity.OnEnter, _machineType));
        }
        

        public void OnSaveScene()
        {
            _savedData.IsActive = _isActive;
        }

        public void OnLoadScene()
        {
            if (_savedData.IsActive)
            {
                _isActive = true;
                _vanishPrefab.SetActive(true);
            }
            else
            {
                _isActive = false;
                _vanishPrefab.SetActive(false);
            }
        }
    }
}