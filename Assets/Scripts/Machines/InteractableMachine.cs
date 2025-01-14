using Core;
using BioEngineerLab.Tasks.SideEffects;
using JetBrains.Annotations;
using Mechanics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using BioEngineerLab.Tasks.SideEffects;

namespace Machines
{
    public class InteractableMachine : MonoBehaviour, ISideEffectActivator
    {

        [SerializeField] private VRGrabInteractable _grabInteractable;
        [SerializeField] private EInteractable _interactableType;

        
        [CanBeNull] private GameManager _gameManager;
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