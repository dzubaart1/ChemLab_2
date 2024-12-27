using Core;
using BioEngineerLab.Tasks.SideEffects;
using JetBrains.Annotations;
using Mechanics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Machines
{
    public class SetInteractableListener : MonoBehaviour, ISaveable
    {
        private struct SavedData
        {
            public bool Interactable;
        }

        [SerializeField] private VRGrabInteractable _grabInteractable;
        [SerializeField] private EInteractable _interactableType;
        
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

            _gameManager.Game.SaveGameEvent += OnSaveScene;
            _gameManager.Game.LoadGameEvent += OnLoadScene;
            _gameManager.Game.SideEffectActivatedEvent += OnActivatedSideEffect;
        }
        
        private void OnDisable()
        {
            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.Game.SaveGameEvent -= OnSaveScene;
            _gameManager.Game.LoadGameEvent -= OnLoadScene;
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

        public void OnSaveScene()
        {
            InteractionLayerMask layers = _grabInteractable.interactionLayers;
            _savedData.Interactable = (layers.value | 2) == 1;
        }

        public void OnLoadScene()
        {
            if (_savedData.Interactable)
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
