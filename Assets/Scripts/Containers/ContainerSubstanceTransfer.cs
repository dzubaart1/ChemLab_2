using System.Collections;
using BioEngineerLab.Core;
using BioEngineerLab.Gameplay;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace BioEngineerLab.Containers
{
    [RequireComponent(typeof(Collider), typeof(VRGrabInteractable), typeof(ContainerCupSocket))]
    public class ContainerSubstanceTransfer : MonoBehaviour
    {
        private const float DELAY_TRIGGERED = 0.5f;
        
        private CraftService _craftService;
        
        private XRGrabInteractable _xrGrabInteractable;
        private Container _container;
        private ContainerCupSocket _containerCupSocket;
        
        private bool _isAlreadyTriggered;
        
        private void Awake()
        {
            _xrGrabInteractable = GetComponent<XRGrabInteractable>();
            _container = GetComponent<Container>();
            _containerCupSocket = GetComponent<ContainerCupSocket>();

            _craftService = Engine.GetService<CraftService>();
        }

        private void OnTriggerStay(Collider other)
        {
            if (_xrGrabInteractable.interactorsSelecting.Count == 0)
            {
                return;
            }
            if (_containerCupSocket.IsClosed())
            {
                return;
            }
            
            ContainerSubstanceTransfer targetContainer = other.GetComponent<ContainerSubstanceTransfer>();
            if(targetContainer is null)
            {
                return;
            }

            ContainerCupSocket targetContainerCup = other.GetComponent<ContainerCupSocket>();
            if(targetContainerCup is null)
            {
                return;
            }

            if (targetContainerCup.IsClosed())
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
                _craftService.Transfer(_container, targetContainer._container);
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