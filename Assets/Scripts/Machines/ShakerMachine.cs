using System.Collections;
using BioEngineerLab.Activities;
using Core;
using Core.Services;
using Containers;
using Crafting;
using JetBrains.Annotations;
using Mechanics;
using UI.Components;
using UnityEngine;

namespace Machines
{
    public class ShakerMachine : MonoBehaviour, ISaveable
    {
        private class SavedData
        {
            public bool IsOpen;
            public bool IsShaking;
        }
        [SerializeField] private ButtonComponent _powerButton;
        [SerializeField] private ButtonComponent _rpmButton;

        [SerializeField] private VRSocketInteractor _socket1;
        [SerializeField] private VRSocketInteractor _socket2;   
        [SerializeField] private VRSocketInteractor _socket3;
        
        [CanBeNull] private GameManager _gameManager;
        
        private bool _isOpen = false;
        private bool _isShaking = false;
        private SavedData _savedData = new SavedData();
        private Animator _animator;

        private void Awake()
        {
            _gameManager = GameManager.Instance;
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.Game.LoadGameEvent += OnLoadScene;
            _gameManager.Game.SaveGameEvent += OnSaveScene;
            
            _rpmButton.ClickBtnEvent += OnRpmButtonClick;
        }

        private void OnDisable()
        {
            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.Game.LoadGameEvent -= OnLoadScene;
            _gameManager.Game.SaveGameEvent -= OnSaveScene;
            
            _rpmButton.ClickBtnEvent -= OnRpmButtonClick;
        }

        private void Start()
        {
            OnSaveScene();
        }

        private void OnRpmButtonClick()
        {
            if (_powerButton.IsOn)
            {
                _animator.enabled = _rpmButton.IsOn;
                _isShaking = _rpmButton.IsOn;

                if (!_isShaking)
                {
                    if (_socket1.SelectedObject == null || _socket2.SelectedObject == null || _socket3.SelectedObject == null)
                    {
                        return;
                    }
                    
                    LabContainer container1 = _socket1.SelectedObject.transform.GetComponent<LabContainer>();
                    LabContainer container2 = _socket2.SelectedObject.transform.GetComponent<LabContainer>();
                    LabContainer container3 = _socket3.SelectedObject.transform.GetComponent<LabContainer>();
                    
                    if (!CraftTools.TryFindCraft(_gameManager.Game.SOCrafts, container1.GetSubstanceProperties(), ECraft.HeatStir, out SOLabCraft labCraft1))
                    {
                        return;
                    }
                    CraftTools.ApplyCraft(labCraft1.LabCraft, container1);
                    
                    if (!CraftTools.TryFindCraft(_gameManager.Game.SOCrafts, container2.GetSubstanceProperties(), ECraft.HeatStir, out SOLabCraft labCraft2))
                    {
                        return;
                    }
                    CraftTools.ApplyCraft(labCraft2.LabCraft, container2);
                    
                    if (!CraftTools.TryFindCraft(_gameManager.Game.SOCrafts, container3.GetSubstanceProperties(), ECraft.HeatStir, out SOLabCraft labCraft3))
                    {
                        return;
                    }
                    CraftTools.ApplyCraft(labCraft3.LabCraft, container3);
                }
            }
        }
        
        public void OnSaveScene()
        {
            _savedData.IsShaking = _isShaking;
        }

        public void OnLoadScene()
        {
            _animator.enabled = _savedData.IsShaking;
            _isShaking = _savedData.IsShaking;
        }
    }
}