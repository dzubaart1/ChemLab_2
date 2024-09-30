using System;
using System.Collections.Generic;
using BioEngineerLab.Activities;
using BioEngineerLab.Containers;
using BioEngineerLab.Core;
using BioEngineerLab.Gameplay;
using UnityEngine;

namespace BioEngineerLab.Machines
{
    [RequireComponent(typeof(Collider))]
    public class WashingMachine : MonoBehaviour, ISaveable
    {
        private struct SavedData
        {
            public List<VRGrabInteractable> HiddenGameObjects;
        }

        private SaveService _saveService;
        private TasksService _tasksService;
        
        private List<VRGrabInteractable> _hiddenGameObjects;
        private SavedData _savedData;

        private void Awake()
        {
            _tasksService = Engine.GetService<TasksService>();
            
            _saveService = Engine.GetService<SaveService>();
            _saveService.LoadSceneStateEvent += OnLoadScene;
            _saveService.SaveSceneStateEvent += OnSaveScene;
            
            _savedData = new SavedData();
            _savedData.HiddenGameObjects = new List<VRGrabInteractable>();
            _hiddenGameObjects = new List<VRGrabInteractable>();
        }

        private void Start()
        {
            OnSaveScene();
        }

        private void OnDestroy()
        {
            _saveService.LoadSceneStateEvent -= OnLoadScene;
            _saveService.SaveSceneStateEvent -= OnSaveScene;
        }

        private void OnTriggerEnter(Collider other)
        {
            VRGrabInteractable interactable = other.GetComponentInParent<VRGrabInteractable>();
            Container container = other.GetComponentInParent<Container>();

            if (interactable == null | container == null)
            {
                return;
            }
            
            interactable.gameObject.SetActive(false);
            _hiddenGameObjects.Add(interactable);
            
            _tasksService.TryCompleteTask(new WashingActivity(container.ContainerType));
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
                Debug.Log($"LOAD; WASHING; 1 {interactable.gameObject.name}");
                interactable.gameObject.SetActive(true);
                interactable.LoadPosition();
            }

            foreach (var interactable in _savedData.HiddenGameObjects)
            {
                Debug.Log($"LOAD; WASHING; 2 {interactable.gameObject.name}");
                interactable.gameObject.SetActive(false);
            }
            
            _hiddenGameObjects.Clear();

            foreach (var interactable in _savedData.HiddenGameObjects)
            {
                Debug.Log($"LOAD; WASHING; 3 {interactable.gameObject.name}");
                _hiddenGameObjects.Add(interactable);
            }
        }
    }
}