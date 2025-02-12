using BioEngineerLab.Activities;
using Core;
using Saveables;
using UnityEngine;
using BioEngineerLab.Tasks.SideEffects;
using Mechanics;

namespace Machines
{
    [RequireComponent(typeof(Collider))]
    public class LeftHandGrabChecker : MonoBehaviour, ISaveableOther, ISideEffectActivator
    {
        private class SavedData
        {
            public bool IsActive;
        }
        
        [SerializeField] private VRGrabInteractable _grabInteractable;
        
        private SavedData _savedData = new SavedData();
        private Player _player;
        private bool _isActive = false;
        private bool _isGrabbed = false;
        
        private void Start()
        {
            GameManager gameManager = GameManager.Instance;
            if (gameManager == null)
            {
                return;
            }

            if (gameManager.CurrentBaseLocalManager == null)
            {
                return;
            }
            
            gameManager.CurrentBaseLocalManager.AddSaveableOther(this);
            gameManager.CurrentBaseLocalManager.AddSideEffectActivator(this);
            
            _player = gameManager.PlayerSpawner.Player;
            
            _player.LeftHandGrabbedEvent += OnLeftHandGrab;
        }

        private void OnEnable()
        {
            _grabInteractable.GrabbedEvent += OnGrab;
            _grabInteractable.UngrabbedEvent += OnUngrab;
        }

        private void OnDisable()
        {
            _player.LeftHandGrabbedEvent -= OnLeftHandGrab;
            _grabInteractable.GrabbedEvent -= OnGrab;
            _grabInteractable.UngrabbedEvent -= OnUngrab;
        }

        private void OnGrab()
        {
            if (_isGrabbed && _isActive)
            {
                GameManager gameManager = GameManager.Instance;
                if (gameManager == null)
                {
                    return;
                }

                if (gameManager.CurrentBaseLocalManager == null)
                {
                    return;
                }
                gameManager.CurrentBaseLocalManager.OnActivityComplete(new BadLabActivity());
            }
        }

        private void OnUngrab()
        {
            _isGrabbed = false;
        }

        private void OnLeftHandGrab()
        {
            _isGrabbed = true;
        }

        public void Save()
        {
            _savedData.IsActive = _isActive;
        }

        public void Load()
        {
            _isActive = _savedData.IsActive;
        }
        
        public void OnActivateSideEffect(LabSideEffect sideEffect)
        {
            if (sideEffect is not TriggerActivatorSideEffect triggerActivatorSideEffect)
            {
                return;
            }

            if (triggerActivatorSideEffect.TriggerType != ETriggerType.LeftHandTrigger)
            {
                return;
            }

            _isActive = triggerActivatorSideEffect.IsActive;
        }
    }
}