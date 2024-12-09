using System.Collections.Generic;
using BioEngineerLab.Activities;
using Containers;
using Core;
using UnityEngine;
using Mechanics;
using UnityEngine.XR.Interaction.Toolkit;
using BioEngineerLab.Tasks.SideEffects;
using JetBrains.Annotations;

namespace BioEngineerLab.Machines
{
    [RequireComponent(typeof(Collider))]
    public class TeleportMachine : MonoBehaviour, ISaveable
    {
        private class SavedData
        {
            public List<VRGrabInteractable> HiddenGameObjects = new List<VRGrabInteractable>();
        }
        
        [SerializeField] private VRSocketInteractor _socketInteractor;
        [SerializeField] private ESocket _socketType;
        [SerializeField] private EMachine _machineType;
        [SerializeField] private GameObject _docObject;

        [CanBeNull] private GameManager _gameManager;
        
        private List<VRGrabInteractable> _hiddenGameObjects = new List<VRGrabInteractable>();
        private SavedData _savedData = new SavedData();

        private void Awake()
        {
            _gameManager = GameManager.Instance;
        }

        private void OnEnable()
        {
            _socketInteractor.selectEntered.AddListener(OnEnter);
            
            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.Game.LoadGameEvent += OnLoadScene;
            _gameManager.Game.SaveGameEvent += OnSaveScene;
            
            _gameManager.Game.SideEffectActivatedEvent += OnActivatedSideEffect;
        }

        private void OnDisable()
        {
            _socketInteractor.selectEntered.RemoveListener(OnEnter);
            
            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.Game.LoadGameEvent -= OnLoadScene;
            _gameManager.Game.SaveGameEvent -= OnSaveScene;
            
            _gameManager.Game.SideEffectActivatedEvent -= OnActivatedSideEffect;
        }

        private void Start()
        {
            OnSaveScene();
        }

        private void OnEnter(SelectEnterEventArgs args)
        {
            if (_gameManager == null)
            {
                return;
            }
            
            if (_socketInteractor.SelectedObject == null)
            {
                return;
            }
            
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

                _gameManager.Game.CompleteTask(new SocketSubstancesLabActivity(_socketType, ESocketActivity.Enter,
                    labContainers[0].GetSubstanceProperties()));
            }
            else
            {
                LabContainer labContainer = interactable.GetComponent<LabContainer>();
                
                interactable.gameObject.SetActive(false);
                _hiddenGameObjects.Add(interactable);

                _gameManager.Game.CompleteTask(new SocketSubstancesLabActivity(_socketType, ESocketActivity.Enter,
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