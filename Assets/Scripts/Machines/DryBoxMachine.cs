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
    [RequireComponent(typeof(Collider))]
    public class DryBoxMachine : MonoBehaviour, ISaveable
    {
        private class SavedData
        {
            public bool IsOn;
            public bool IsOpen;
        }

        [FormerlySerializedAs("_button")] [SerializeField] private ButtonComponent _dryButton;
        [SerializeField] private VRSocketInteractor _socketInteractor;
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

            _dryButton.ClickBtnEvent += OnClickDryBtn;
        }

        private void OnDisable()
        {
            _saveService.LoadSceneStateEvent -= OnLoadScene;
            _saveService.SaveSceneStateEvent -= OnSaveScene;
            
            _dryButton.ClickBtnEvent -= OnClickDryBtn;
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
                _tasksService.TryCompleteTask(new DoorLabActivity(EDoor.DryMachineDoor, EDoorActivity.Closed));
            }
        }

        private void OnClickDryBtn()
        {
            if (_socketInteractor.SelectedObject == null)
            {
                return;
            }
            
            LabContainer container = _socketInteractor.SelectedObject.GetComponent<LabContainer>();

            if (container is null)
            {
                return;
            }
            
            if (_isOpen)
            {
                return;
            }
            
            _craftService.Dry(container);
        }
        public void OnSaveScene()
        {
            _savedData.IsOn = _dryButton;
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