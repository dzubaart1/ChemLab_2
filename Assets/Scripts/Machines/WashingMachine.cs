using System.Collections.Generic;
using BioEngineerLab.Activities;
using Containers;
using Core;
using JetBrains.Annotations;
using Mechanics;
using UnityEngine;

namespace Machines
{
    [RequireComponent(typeof(Collider))]
    public class WashingMachine : MonoBehaviour, ISaveable
    {
        private class SavedData
        {
            public List<VRGrabInteractable> HiddenGameObjects = new List<VRGrabInteractable>();
        }

        [CanBeNull] private GameManager _gameManager;
        
        private List<VRGrabInteractable> _hiddenGameObjects = new List<VRGrabInteractable>();
        private SavedData _savedData = new SavedData();

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
        }

        private void OnDisable()
        {
            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.Game.LoadGameEvent -= OnLoadScene;
            _gameManager.Game.SaveGameEvent -= OnSaveScene;
        }

        private void Start()
        {
            OnSaveScene();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_gameManager == null)
            {
                return;
            }
            
            VRGrabInteractable interactable = other.GetComponentInParent<VRGrabInteractable>();
            LabContainer container = other.GetComponentInParent<LabContainer>();

            if (interactable == null | container == null)
            {
                return;
            }
            
            interactable.gameObject.SetActive(false);
            _hiddenGameObjects.Add(interactable);
            
            _gameManager.CompleteTask(new MachineLabActivity(EMachineActivity.OnEnter, EMachine.WashingMachine));
        }

        public void OnSaveScene()
        {
            _savedData.HiddenGameObjects.Clear();
            
            foreach (var gameObject in _hiddenGameObjects)
            {
                _savedData.HiddenGameObjects.Add(gameObject);
            }
        }

        public void OnLoadScene()
        {
            foreach (var interactable in _hiddenGameObjects)
            {
                interactable.gameObject.SetActive(true);
                interactable.LoadPosition();
            }

            foreach (var interactable in _savedData.HiddenGameObjects)
            {
                interactable.gameObject.SetActive(false);
            }
            
            _hiddenGameObjects.Clear();

            foreach (var interactable in _savedData.HiddenGameObjects)
            {
                _hiddenGameObjects.Add(interactable);
            }
        }
    }
}