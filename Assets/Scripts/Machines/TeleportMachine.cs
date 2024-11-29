using System;
using System.Collections.Generic;
using Activities;
using BioEngineerLab.Activities;
using Containers;
using Core;
using Gameplay;
using Core.Services;
using UnityEngine;
using Mechanics;
using UnityEngine.PlayerLoop;
using UnityEngine.XR.Interaction.Toolkit;
using BioEngineerLab.Tasks.SideEffects;

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
        [SerializeField] private EMachine _machineType;
        [SerializeField] private GameObject _docObject;

        private void Awake()
        {
            _tasksService = Engine.GetService<TasksService>();
            _saveService = Engine.GetService<SaveService>();
        }

        private void OnEnable()
        {
            _saveService.LoadSceneStateEvent += OnLoadScene;
            _saveService.SaveSceneStateEvent += OnSaveScene;
            _tasksService.SideEffectActivatedEvent += OnActivatedSideEffect;
            
            _socketInteractor.selectEntered.AddListener(OnEnter);
        }

        private void OnDisable()
        {
            _saveService.LoadSceneStateEvent -= OnLoadScene;
            _saveService.SaveSceneStateEvent -= OnSaveScene;
            _tasksService.SideEffectActivatedEvent -= OnActivatedSideEffect;
            
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
            
            if (_socketType == ESocket.TeleportCellResSocket)
            {

                LabContainer[] labContainers = interactable.GetComponentsInChildren<LabContainer>();

                interactable.gameObject.SetActive(false);
                _hiddenGameObjects.Add(interactable);

                _tasksService.TryCompleteTask(new SocketSubstancesLabActivity(_socketType, ESocketActivity.Enter,
                    labContainers[0].GetSubstanceProperties()));
            }
            else
            {
                LabContainer labContainer = interactable.GetComponent<LabContainer>();
                
                interactable.gameObject.SetActive(false);
                _hiddenGameObjects.Add(interactable);

                _tasksService.TryCompleteTask(new SocketSubstancesLabActivity(_socketType, ESocketActivity.Enter,
                    labContainer.GetSubstanceProperties()));
            }
        }
        
        private void OnActivatedSideEffect(LabSideEffect sideEffect)
        {
            if (sideEffect is not SpawnDocLabSideEffect spawnDocLabSideEffect)
            {
                return;
            }
            
            spawnDocLabSideEffect = sideEffect as SpawnDocLabSideEffect;

            if (spawnDocLabSideEffect.MachineType == _machineType)
            {
                _docObject.SetActive(true);
            }
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