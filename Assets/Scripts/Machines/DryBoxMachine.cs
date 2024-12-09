using BioEngineerLab.Activities;
using Containers;
using Core;
using Core.Services;
using Crafting;
using JetBrains.Annotations;
using Mechanics;
using UI.Components;
using UnityEngine;

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

        [SerializeField] private ButtonComponent _dryButton;
        [SerializeField] private VRSocketInteractor _socketInteractor;
        [SerializeField] private Transform _door;

        [CanBeNull] private GameManager _gameManager;
        
        private bool _isOpen = false;
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

            _dryButton.ClickBtnEvent += OnClickDryBtn;
        }

        private void OnDisable()
        {
            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.Game.LoadGameEvent += OnLoadScene;
            _gameManager.Game.SaveGameEvent += OnSaveScene;
            
            _dryButton.ClickBtnEvent -= OnClickDryBtn;
        }

        private void Start()
        {
            OnSaveScene();
        }

        private void Update()
        {
            if (_gameManager == null)
            {
                return;
            }
            
            if (_door.transform.rotation.z < 0.49f && !_isOpen)
            {
                _isOpen = true;
            }
            else if (_door.transform.rotation.z > 0.49f && _isOpen)
            {
                _isOpen = false;
                _gameManager.Game.CompleteTask(new DoorLabActivity(EDoor.DryMachineDoor, EDoorActivity.Closed));
            }
        }

        private void OnClickDryBtn()
        {
            if (_gameManager == null)
            {
                return;
            }
            
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
            
            if (!CraftTools.TryFindCraft(_gameManager.Game.SOCrafts, container.GetSubstanceProperties(), ECraft.Dry, out SOLabCraft craftContainer))
            {
                _gameManager.Game.CompleteTask(new BadLabActivity());
                return;
            }
            
            CraftTools.ApplyCraft(craftContainer.LabCraft, container);
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