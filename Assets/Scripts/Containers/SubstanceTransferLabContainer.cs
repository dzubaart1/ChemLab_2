using System.Collections;
using Core;
using Core.Services;
using Mechanics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Containers
{
    [RequireComponent(typeof(Collider), typeof(VRGrabInteractable), typeof(CupSocketLabContainer))]
    public class SubstanceTransferLabContainer : MonoBehaviour
    {
        private const float DELAY_TRIGGERED = 0.5f;
        
        private CraftService _craftService;
        
        private XRGrabInteractable _xrGrabInteractable;
        private LabContainer _labContainer;
        private CupSocketLabContainer _cupSocketLabContainer;
        
        private bool _isAlreadyTriggered;
        
        private void Awake()
        {
            _xrGrabInteractable = GetComponent<XRGrabInteractable>();
            _labContainer = GetComponent<LabContainer>();
            _cupSocketLabContainer = GetComponent<CupSocketLabContainer>();
            _craftService = Engine.GetService<CraftService>();
        }

        private void OnTriggerStay(Collider other)
        {
            if (_xrGrabInteractable.interactorsSelecting.Count == 0)
            {
                return;
            }
            if (_cupSocketLabContainer.IsClosed())
            {
                return;
            }
            
            SubstanceTransferLabContainer targetSubstanceTransferLabContainer = other.GetComponent<SubstanceTransferLabContainer>();
            if(targetSubstanceTransferLabContainer is null)
            {
                return;
            }

            CupSocketLabContainer targetCupSocketLabContainerCup = other.GetComponent<CupSocketLabContainer>();
            if(targetCupSocketLabContainerCup is null)
            {
                return;
            }

            if (targetCupSocketLabContainerCup.IsClosed())
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
                _craftService.Transfer(_labContainer, targetSubstanceTransferLabContainer._labContainer);
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