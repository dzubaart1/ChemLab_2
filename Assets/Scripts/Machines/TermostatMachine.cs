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
    public class TermostatMachine : MonoBehaviour, ISaveable
    {
        private class SavedData
        {
            public bool IsOpen = false;
        }

        [SerializeField] private ButtonComponent _powerButton;
        [SerializeField] private VRSocketInteractor _socketInteractor1;
        [SerializeField] private VRSocketInteractor _socketInteractor2;
        [SerializeField] private DoorMachine _doorMachine;

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

            _doorMachine.DoorClosedEvent += Dry;
        }

        private void OnDisable()
        {
            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.Game.LoadGameEvent -= OnLoadScene;
            _gameManager.Game.SaveGameEvent -= OnSaveScene;
            
            _doorMachine.DoorClosedEvent -= Dry;
        }

        private void Start()
        {
            OnSaveScene();
        }

        private void Dry()
        {
            if (!_powerButton.IsOn)
            {
                return;
            }
            
            if (_isOpen)
            {
                return;
            }
            
            if (_gameManager == null)
            {
                return;
            }
            
            if (_socketInteractor1.SelectedObject == null)
            {
                return;
            }
            
            LabContainer container1 = _socketInteractor1.SelectedObject.GetComponent<LabContainer>();

            if (container1 is null)
            {
                return;
            }
            
            if (!CraftTools.TryFindCraft(_gameManager.Game.SOCrafts, container1.GetSubstanceProperties(), ECraft.Dry, out SOLabCraft craftContainer1))
            {
                _gameManager.Game.CompleteTask(new BadLabActivity());
                return;
            }
            
            CraftTools.ApplyCraft(craftContainer1.LabCraft, container1);
            
            if (_socketInteractor2.SelectedObject == null)
            {
                return;
            }
            
            LabContainer container2 = _socketInteractor1.SelectedObject.GetComponent<LabContainer>();

            if (container2 is null)
            {
                return;
            }
            
            if (!CraftTools.TryFindCraft(_gameManager.Game.SOCrafts, container2.GetSubstanceProperties(), ECraft.Dry, out SOLabCraft craftContainer2))
            {
                _gameManager.Game.CompleteTask(new BadLabActivity());
                return;
            }
            
            CraftTools.ApplyCraft(craftContainer2.LabCraft, container2);
        }
        
        public void OnSaveScene()
        {
            _savedData.IsOpen = _isOpen;
        }

        public void OnLoadScene()
        {
            
        }
    }
}