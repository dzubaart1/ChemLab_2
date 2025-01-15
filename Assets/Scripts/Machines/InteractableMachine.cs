using Core;
using BioEngineerLab.Tasks.SideEffects;
using JetBrains.Annotations;
using Mechanics;
using Saveables;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using BioEngineerLab.Tasks.SideEffects;

namespace Machines
{
    public class InteractableMachine : MonoBehaviour, ISideEffectActivator, ISaveableOther
    {
        private class SavedData
        {
            public bool IsActive = false;
        }
        
        [SerializeField] private VRGrabInteractable _grabInteractable;
        [SerializeField] private EInteractable _interactableType;
        
        [CanBeNull] private GameManager _gameManager;
        
        private SavedData _savedData = new SavedData();
        private bool _isActive;
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
            
            gameManager.CurrentBaseLocalManager.AddSideEffectActivator(this);
        }

        public void OnActivateSideEffect(LabSideEffect sideEffect)
        {
            if (sideEffect is not SetInteractableSideEffect setInteractableSideEffect)
            {
                return;
            }

            if (setInteractableSideEffect.InteractableObject != _interactableType)
            {
                return;
            }

            if (setInteractableSideEffect.IsInteractable)
            {
                InteractionLayerMask layers = _grabInteractable.interactionLayers;
                layers.value |= 2;
                _grabInteractable.interactionLayers = layers;
                
                _isActive = true;
            }
            else
            {
                InteractionLayerMask layers = _grabInteractable.interactionLayers;
                layers.value &= ~2;
                _grabInteractable.interactionLayers = layers;
                
                _isActive = false;
            }
        }
        
        public void Save()
        {
            _savedData.IsActive = _isActive;
        }
        
        public void Load()
        {
            _isActive = _savedData.IsActive;
            
            if (_isActive)
            {
                InteractionLayerMask layers = _grabInteractable.interactionLayers;
                layers.value |= 2;
                _grabInteractable.interactionLayers = layers;
            }
            else
            {
                InteractionLayerMask layers = _grabInteractable.interactionLayers;
                layers.value &= ~2;
                _grabInteractable.interactionLayers = layers;
            }
        }
    }
}