using Activities;
using BioEngineerLab.Activities;
using Containers;
using Core;
using Core.Services;
using Mechanics;
using UI.Components;
using UnityEngine;
using UnityEngine.Serialization;

namespace Machines
{
    public class AutoClaveMachine : MonoBehaviour, ISaveable
    {
        private class SavedData
        {
            public bool IsOn;
            public bool IsOpen;
        }

        [FormerlySerializedAs("_button")] [SerializeField] private ButtonComponent _powerButton;
        [SerializeField] private VRSocketInteractor[] _socketInteractors;
        [SerializeField] private Transform _door;

        private SaveService _saveService;
        private TasksService _tasksService;
        private CraftService _craftService;
        
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

        private void Update()
        {
            if (_door.transform.rotation.z < 0.49f && !_isOpen)
            {
                _isOpen = true;
            }
            else if (_door.transform.rotation.z > 0.49f && _isOpen)
            {
                _isOpen = false;
                _tasksService.TryCompleteTask(new DoorLabActivity(EDoor.AutoClaveDoor, EDoorActivity.Closed));
            }
        }
        public void OnSaveScene()
        {
            _savedData.IsOpen = _isOpen;
        }

        public void OnLoadScene()
        {
            if (_savedData.IsOpen)
            {
                _door.transform.rotation = new Quaternion(0.5f, 0.5f, 0, 0.5f);
            }
            else
            {
                _door.transform.rotation = new Quaternion(0.5f, 0.5f, 0.5f, 0.5f);
            }
        }
    }
}