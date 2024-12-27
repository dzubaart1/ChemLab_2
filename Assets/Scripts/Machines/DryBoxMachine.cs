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
            public bool IsOpen = false;
        }

        [SerializeField] private ButtonComponent _dryButton;
        [SerializeField] private VRSocketInteractor _socketInteractor;

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
            
        }

        public void OnLoadScene()
        {
            
        }
    }
}