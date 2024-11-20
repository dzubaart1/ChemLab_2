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
        private class SavedData
        {
            public List<VRGrabInteractable> HiddenGameObjects = new List<VRGrabInteractable>();
        }

        private SaveService _saveService;
        private TasksService _tasksService;
        
        private List<VRGrabInteractable> _hiddenGameObjects = new List<VRGrabInteractable>();
        private SavedData _savedData = new SavedData();

        private void Awake()
        {
            _tasksService = Engine.GetService<TasksService>();
            _saveService = Engine.GetService<SaveService>();
        }

        private void OnEnable()
        {
            _saveService.LoadSceneStateEvent += OnLoadScene;
            _saveService.SaveSceneStateEvent += OnSaveScene;
        }

        private void OnDisable()
        {
            _saveService.LoadSceneStateEvent -= OnLoadScene;
            _saveService.SaveSceneStateEvent -= OnSaveScene;
        }

        private void Start()
        {
            OnSaveScene();
        }

        private void OnTriggerEnter(Collider other)
        {
            VRGrabInteractable interactable = other.GetComponentInParent<VRGrabInteractable>();
            LabContainer container = other.GetComponentInParent<LabContainer>();

            if (interactable == null | container == null)
            {
                return;
            }
            
            interactable.gameObject.SetActive(false);
            _hiddenGameObjects.Add(interactable);
            
            _tasksService.TryCompleteTask(new MachineLabActivity(EMachineActivity.OnEnter, EMachine.WashingMachine));
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