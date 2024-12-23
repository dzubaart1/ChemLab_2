using Core;
using BioEngineerLab.Tasks.SideEffects;
using JetBrains.Annotations;
using Mechanics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Machines
{
    public class SetInteractableListener : MonoBehaviour
    {

        [SerializeField] private VRGrabInteractable _grabInteractable;
        [SerializeField] private EInteractable _interactableType;

        
        [CanBeNull] private GameManager _gameManager;
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
            
            _gameManager.Game.SideEffectActivatedEvent += OnActivatedSideEffect;
        }
        
        private void OnDisable()
        {
            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.Game.SideEffectActivatedEvent -= OnActivatedSideEffect;
        }
        
        private void OnActivatedSideEffect(LabSideEffect sideEffect)
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
