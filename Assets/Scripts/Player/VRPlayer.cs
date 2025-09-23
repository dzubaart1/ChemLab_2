using System;
using Gameplay;
using Machines;
using UI.TabletUI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;
using Database;

namespace Core
{
    public class VRPlayer : Player
    {
        [Header("Refs")]
        [SerializeField] private TabletUI _tabletUI;
        [SerializeField] private HandAnimatorController _rightHandAnimatorController;
        [SerializeField] private HandAnimatorController _leftHandAnimatorController;
        
        [Space]
        [Header("Interactors")]
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
            _rightDirectInteractor.selectEntered.AddListener(OnRightHandSelected);
            _leftDirectInteractor.selectEntered.AddListener(OnLeftHandSelected);
            
            _rightDirectInteractor.selectExited.AddListener(OnRightHandExited);
            _leftDirectInteractor.selectExited.AddListener(OnLeftHandExited);
        }

        private void OnDisable()
        {
            _rightDirectInteractor.selectEntered.RemoveListener(OnRightHandSelected);
            _leftDirectInteractor.selectEntered.RemoveListener(OnLeftHandSelected);
            
            _rightDirectInteractor.selectExited.RemoveListener(OnRightHandExited);
            _leftDirectInteractor.selectExited.RemoveListener(OnLeftHandExited);
        }
        
        public override void ReleaseAllGrabbables()
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

        public override void ReleaseHandle()
        {
            for (var i = _leftDirectInteractor.interactablesSelected.Count - 1; i >= 0; --i)
            {
                if (_leftDirectInteractor.interactablesSelected[i].transform.CompareTag("Handle"))
                {
                    _leftDirectInteractor.interactionManager.SelectCancel(_leftDirectInteractor, _leftDirectInteractor.interactablesSelected[i]);
                }
            }
            
            for (var i = _rightDirectInteractor.interactablesSelected.Count - 1; i >= 0; --i)
            {
                if (_rightDirectInteractor.interactablesSelected[i].transform.CompareTag("Handle"))
                {
                    _rightDirectInteractor.interactionManager.SelectCancel(_rightDirectInteractor, _rightDirectInteractor.interactablesSelected[i]);
                }
            }
        }

        public override void Init()
        {
            _rightHandAnimatorController.Init();
            _leftHandAnimatorController.Init();
            _handsChanger.Init();
            _tabletUI.Init();
        }

        private void OnRightHandSelected(SelectEnterEventArgs args)
        {
            _rightRayInteractor.enableUIInteraction = false;

            CallRightHandGrabbedEvent();
        }
        
        private void OnRightHandExited(SelectExitEventArgs args)
        {
            _rightRayInteractor.enableUIInteraction = true;
        }

        private void OnLeftHandSelected(SelectEnterEventArgs args)
        {
            _leftRayInteractor.enableUIInteraction = false;

            CallLeftHandGrabbedEvent();
        }

        private void OnLeftHandExited(SelectExitEventArgs args)
        {
            _leftRayInteractor.enableUIInteraction = true;
        }
    }
}