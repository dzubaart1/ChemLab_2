using System;
using Machines;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Core
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private HandsChanger HandsChanger;
        [SerializeField] private XRDirectInteractor _leftDirectInteractor;
        [SerializeField] private XRDirectInteractor _rightDirectInteractor;
        
        [SerializeField] private XRRayInteractor _leftRayInteractor;
        [SerializeField] private XRRayInteractor _rightRayInteractor;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            _rightDirectInteractor.onSelectEntered.AddListener(OnRightHandSelected);
            _leftDirectInteractor.onSelectEntered.AddListener(OnLeftHandSelected);
            
            _rightDirectInteractor.onSelectExited.AddListener(OnRightHandExited);
            _leftDirectInteractor.onSelectExited.AddListener(OnLeftHandExited);
        }

        private void OnDisable()
        {
            _rightDirectInteractor.onSelectEntered.RemoveListener(OnRightHandSelected);
            _leftDirectInteractor.onSelectEntered.RemoveListener(OnLeftHandSelected);
            
            _rightDirectInteractor.onSelectExited.RemoveListener(OnRightHandExited);
            _leftDirectInteractor.onSelectExited.RemoveListener(OnLeftHandExited);
        }


        private void OnRightHandSelected(XRBaseInteractable interactable)
        {
            _rightRayInteractor.enableUIInteraction = false;
        }
        
        private void OnRightHandExited(XRBaseInteractable interactable)
        {
            _rightRayInteractor.enableUIInteraction = true;
        }

        private void OnLeftHandSelected(XRBaseInteractable interactable)
        {
            _leftRayInteractor.enableUIInteraction = false;
        }

        private void OnLeftHandExited(XRBaseInteractable interactable)
        {
            _leftRayInteractor.enableUIInteraction = true;
        }

        public HandsChanger GetHandsChanger()
        {
            return HandsChanger;
        }

        public void ReleaseAllGrabbables()
        { 
            for (var i = _leftDirectInteractor.interactablesSelected.Count - 1; i >= 0; --i)
            {
                _leftDirectInteractor.interactionManager.SelectCancel(_leftDirectInteractor, _leftDirectInteractor.interactablesSelected[i]);
            }
            
            for (var i = _rightDirectInteractor.interactablesSelected.Count - 1; i >= 0; --i)
            {
                _rightDirectInteractor.interactionManager.SelectCancel(_rightDirectInteractor, _rightDirectInteractor.interactablesSelected[i]);
            }
        }
    }
}