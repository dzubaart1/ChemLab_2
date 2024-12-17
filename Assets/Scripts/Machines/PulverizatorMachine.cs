using System.Collections.Generic;
using System.Collections;
using BioEngineerLab.Activities;
using Containers;
using Core;
using JetBrains.Annotations;
using Mechanics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Machines
{
    [RequireComponent(typeof(Collider))]
    public class PulverizatorMachine : MonoBehaviour
    {
        
        private const float DELAY_TRIGGERED = 0.5f;
       
        [CanBeNull] private GameManager _gameManager;
        private XRGrabInteractable _xrGrabInteractable;
        private bool _isAlreadyTriggered = false;

        private void Awake()
        {
            _gameManager = GameManager.Instance;
            _xrGrabInteractable = GetComponent<XRGrabInteractable>();
        }

        private void OnEnable()
        {
            if (_gameManager == null)
            {
                return;
            }
            _xrGrabInteractable.selectEntered.AddListener(OnSelectEntered);
        }

        private void OnDisable()
        {
            if (_gameManager == null)
            {
                return;
            }
            _xrGrabInteractable.selectEntered.RemoveListener(OnSelectEntered);
        }
        
        private void OnSelectEntered(SelectEnterEventArgs args)
        {
            
        }

        private void OnTriggerStay(Collider other)
        {
            if (_xrGrabInteractable.interactorsSelecting.Count == 0)
            {
                return;
            }
            
            IXRSelectInteractor interactor = _xrGrabInteractable.interactorsSelecting[0];
            if(interactor is null)
            {
                return;
            }

            ActionBasedController controller = interactor.transform.GetComponent<ActionBasedController>();
            if(controller is null)
            {
                return;
            }

            if (!_isAlreadyTriggered & controller.activateAction.action.triggered)
            {
                if (other.CompareTag("LHand"))
                {
                    _gameManager.CompleteTask(new MachineLabActivity(EMachineActivity.OnEnter, EMachine.PulLHandMachine));
                }
                else if (other.CompareTag("RHand"))
                {
                    _gameManager.CompleteTask(new MachineLabActivity(EMachineActivity.OnEnter, EMachine.PulRHandMachine));
                }
                StartCoroutine(StartDelayBetweenActivated());
            }
        }
        private IEnumerator StartDelayBetweenActivated()
        {
            _isAlreadyTriggered = true;
            yield return new WaitForSeconds(DELAY_TRIGGERED);
            _isAlreadyTriggered = false;
        }
    }
}