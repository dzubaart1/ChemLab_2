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
    public class CentrifugaMachine : MonoBehaviour, ISaveable
    {
        private class SavedData
        {
            public bool IsPowered;
            public bool IsStarted;
        }

        [SerializeField] private ButtonComponent _powerButton;
        [SerializeField] private ButtonComponent _startButton;
        [SerializeField] private Animator _animator;
        [SerializeField] private VRSocketInteractor _socketInteractor1;
        [SerializeField] private VRSocketInteractor _socketInteractor2;
        
        [CanBeNull] private GameManager _gameManager;
        
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
            
            _startButton.ClickBtnEvent += OnStartBtnClicked;
        }

        private void OnDisable()
        {
            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.Game.LoadGameEvent += OnLoadScene;
            _gameManager.Game.SaveGameEvent += OnSaveScene;
            
            _startButton.ClickBtnEvent -= OnStartBtnClicked;
        }

        private void Start()
        {
            OnSaveScene();
        }
        
        private void OnStartBtnClicked()
        {
            if (_gameManager == null)
            {
                return;
            }
            
            if (_socketInteractor1.SelectedObject == null | _socketInteractor2.SelectedObject == null)
            {
                return;
            }
            
            LabContainer labContainer1 = _socketInteractor1.SelectedObject.GetComponent<LabContainer>();
            LabContainer labContainer2 = _socketInteractor2.SelectedObject.GetComponent<LabContainer>();

            if (labContainer1 is null || labContainer2 is null)
            {
                return;
            }
            
            if(_startButton.IsOn & _powerButton.IsOn)
            {
                _gameManager.Game.CompleteTask(new MachineLabActivity(EMachineActivity.OnStart, EMachine.CentrifugaMachine));
                CheckAnimatorStatus();
                return;
            }

            if (!CraftTools.TryFindCraft(_gameManager.Game.SOCrafts, labContainer1.GetSubstanceProperties(), ECraft.Split, out SOLabCraft craftContainer1))
            {
                _gameManager.Game.CompleteTask(new BadLabActivity());
                return;
            }
            
            if (!CraftTools.TryFindCraft(_gameManager.Game.SOCrafts, labContainer2.GetSubstanceProperties(), ECraft.Split, out SOLabCraft craftContainer2))
            {
                _gameManager.Game.CompleteTask(new BadLabActivity());
                return;
            }
            
            CraftTools.ApplyCraft(craftContainer1.LabCraft, labContainer1);
            CraftTools.ApplyCraft(craftContainer2.LabCraft, labContainer2);

            _gameManager.Game.CompleteTask(new MachineLabActivity(EMachineActivity.OnFinish, EMachine.CentrifugaMachine));
        
            CheckAnimatorStatus();
        }

        private void CheckAnimatorStatus()
        {
            if (_startButton.IsOn & _powerButton.IsOn)
            {
                _animator.enabled = true;
            }
            else
            {
                _animator.enabled = false;
            }
        }
        
        public void OnSaveScene()
        {
            _savedData.IsPowered = _powerButton.IsOn;
            _savedData.IsStarted = _startButton.IsOn;
        }

        public void OnLoadScene()
        {
            if (_savedData.IsPowered & !_powerButton.IsOn)
            {
                _powerButton.SetIsOn(_savedData.IsPowered);
            }
            
            if (!_savedData.IsPowered & _powerButton.IsOn)
            {
                _powerButton.SetIsOn(_savedData.IsPowered);
            }

            if (_savedData.IsStarted & !_startButton.IsOn)
            {
                _startButton.SetIsOn(_savedData.IsStarted);
            }
            
            if (!_savedData.IsStarted & _startButton.IsOn)
            {
                _startButton.SetIsOn(_savedData.IsStarted);
            }
            
            CheckAnimatorStatus();
        }
    }
}