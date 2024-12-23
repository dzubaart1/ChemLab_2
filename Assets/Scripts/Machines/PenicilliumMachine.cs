using Core;
using BioEngineerLab.Tasks.SideEffects;
using JetBrains.Annotations;
using Mechanics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Machines
{
    public class PenicilliumMachine : MonoBehaviour
    {

        [SerializeField] private VRGrabInteractable _grabInteractable;
        [SerializeField] private VRGrabInteractable _cap;
        [SerializeField] private VRGrabInteractable _plenka;

        
        [CanBeNull] private GameManager _gameManager;
        private void Awake()
        {
            _gameManager = GameManager.Instance;
        }

        private void OnEnable()
        {
            /*_grabInteractable.selectEntered.AddListener(OnSelectEntered);
            _grabInteractable.selectExited.AddListener(OnSelectExited);*/
            
            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.Game.SideEffectActivatedEvent += OnActivatedSideEffect;
        }
        
        private void OnDisable()
        {
            /*_grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
            _grabInteractable.selectExited.RemoveListener(OnSelectExited);*/
            
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

            if (setInteractableSideEffect.InteractableObject != EInteractable.Penicilliuminteractable)
            {
                return;
            }

            if (setInteractableSideEffect.IsInteractable)
            {
                InteractionLayerMask Caplayers = _cap.interactionLayers;
                Caplayers.value |= 2;
                _cap.interactionLayers = Caplayers;
            
                InteractionLayerMask plenkaLayers = _plenka.interactionLayers;
                plenkaLayers.value |= 2;
                _plenka.interactionLayers = plenkaLayers;
            }
            else
            {
                InteractionLayerMask Caplayers = _cap.interactionLayers;
                Caplayers.value &= ~2;
                _cap.interactionLayers = Caplayers;
            
                InteractionLayerMask plenkaLayers = _plenka.interactionLayers;
                plenkaLayers.value &= ~2;
                _plenka.interactionLayers = plenkaLayers;
            }
        }
    }
}
