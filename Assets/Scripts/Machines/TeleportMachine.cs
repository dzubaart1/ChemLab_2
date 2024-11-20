using System;
using System.Collections.Generic;
using BioEngineerLab.Activities;
using BioEngineerLab.Containers;
using BioEngineerLab.Core;
using BioEngineerLab.Gameplay;
using BioEngineerLab.Tasks;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.XR.Interaction.Toolkit;

namespace BioEngineerLab.Machines
{
    [RequireComponent(typeof(Collider))]
    public class TeleportMachine : MonoBehaviour, ISaveable
    {
        private class SavedData
        {
            public List<VRGrabInteractable> HiddenGameObjects = new List<VRGrabInteractable>();
        }

        private SaveService _saveService;
        private TasksService _tasksService;
        
        private List<VRGrabInteractable> _hiddenGameObjects = new List<VRGrabInteractable>();
        private SavedData _savedData = new SavedData();

        [SerializeField] private VRSocketInteractor _socketInteractor;
        [SerializeField] private ESocket _socketType;
        

        private void Awake()
        {
            _tasksService = Engine.GetService<TasksService>();
            _saveService = Engine.GetService<SaveService>();
        }

        private void OnEnable()
        {
            _saveService.LoadSceneStateEvent += OnLoadScene;
            _saveService.SaveSceneStateEvent += OnSaveScene;
            
            _socketInteractor.selectEntered.AddListener(OnEnter);
        }

        private void OnDisable()
        {
            _saveService.LoadSceneStateEvent -= OnLoadScene;
            _saveService.SaveSceneStateEvent -= OnSaveScene;
            
            _socketInteractor.selectEntered.RemoveListener(OnEnter);
        }

        private void Start()
        {
            OnSaveScene();
        }

        private void OnEnter(SelectEnterEventArgs args)
        {
            VRGrabInteractable interactable = _socketInteractor.SelectedObject.GetComponent<VRGrabInteractable>();

            if (interactable == null)
            {
                return;
            }
            
            LabContainer [] labContainers = interactable.GetComponentsInChildren<LabContainer>();
            
            interactable.gameObject.SetActive(false);
            _hiddenGameObjects.Add(interactable);
            
            _tasksService.TryCompleteTask(new SocketSubstancesLabActivity(_socketType, ESocketActivity.Enter, labContainers[0].GetSubstanceProperties()));
        }

        private void OnTriggerEnter(Collider other)
        {
            
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