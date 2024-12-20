using System;
using BioEngineerLab.Activities;
using Containers;
using Core;
using Core.Services;
using Crafting;
using JetBrains.Annotations;
using Mechanics;
using UI.Components;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Machines
{
    public class PenicilliumMachine : MonoBehaviour
    {

        [SerializeField] private VRGrabInteractable _grabInteractable;
        [SerializeField] private VRGrabInteractable _cap;
        [SerializeField] private VRGrabInteractable _plenka;

        private void OnEnable()
        {
            _grabInteractable.selectEntered.AddListener(OnSelectEntered);
            _grabInteractable.selectExited.AddListener(OnSelectExited);
        }
        
        private void OnDisable()
        {
            _grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
            _grabInteractable.selectExited.RemoveListener(OnSelectExited);
        }

        private void OnSelectEntered(SelectEnterEventArgs obj)
        {
            if (!obj.interactorObject.transform.CompareTag("PenicilliumSocket"))
            {
                return;
            }
            InteractionLayerMask capLayers = _cap.interactionLayers;
            capLayers.value = 4194304;
            _cap.interactionLayers = capLayers;
            
            InteractionLayerMask plenkaLayers = _plenka.interactionLayers;
            plenkaLayers.value = 8388608;
            _plenka.interactionLayers = plenkaLayers;
        }

        private void OnSelectExited(SelectExitEventArgs obj)
        {
            if (!obj.interactorObject.transform.CompareTag("PenicilliumSocket"))
            {
                return;
            }
            InteractionLayerMask layers = _cap.interactionLayers;
            layers.value = 4194306;
            _cap.interactionLayers = layers;
            
            InteractionLayerMask plenkaLayers = _plenka.interactionLayers;
            plenkaLayers.value = 8388610;
            _plenka.interactionLayers = plenkaLayers;
        }
    }
}
