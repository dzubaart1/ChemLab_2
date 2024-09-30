using System.Collections;
using BioEngineerLab.Core;
using BioEngineerLab.Gameplay;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace BioEngineerLab.Containers
{
    [RequireComponent(typeof(Collider), typeof(VRGrabInteractable))]
    public class ContainerSubstanceTransfer : MonoBehaviour
    {
        [SerializeField] private VRSocketInteractor _cupSocket;
        
        public Container Container { get; private set; }
        
        private const float DELAY_TRIGGERED = 0.5f;
        
        private XRGrabInteractable _xrGrabInteractable;

        private SubstancesService _substancesService;

        private bool _isActivated;
        
        private void Awake()
        {
            _xrGrabInteractable = GetComponent<XRGrabInteractable>();
            Container = GetComponent<Container>();

            _substancesService = Engine.GetService<SubstancesService>();
        }

        private void OnTriggerStay(Collider other)
        {
            var targetContainer = other.GetComponent<ContainerSubstanceTransfer>();
            if(targetContainer is null || _xrGrabInteractable.interactorsSelecting.Count == 0)
            {
                //UnityEngine.Debug.Log("OnTriggerhere2");
                return;
            }

            if (_cupSocket != null && _cupSocket.firstInteractableSelected != null)
            {
                return;
            }
            
            if (targetContainer._cupSocket != null && targetContainer._cupSocket.firstInteractableSelected != null)
            {
                return;
            }

            var interactor = _xrGrabInteractable.interactorsSelecting[0];
            if(interactor is null)
            {
                //UnityEngine.Debug.Log("OnTriggerhere3");
                return;
            }

            var controller = interactor.transform.GetComponent<ActionBasedController>();
            if(controller is null)
            {
                //UnityEngine.Debug.Log("OnTriggerhere4");
                return;
            }

            if (!_isActivated & controller.activateAction.action.triggered)
            {
                //UnityEngine.Debug.Log("OnTriggerhere5");
                _substancesService.Transfer(Container, targetContainer.Container);
                StartCoroutine(StartDelayBetweenActivated());
            }
        }

        private IEnumerator StartDelayBetweenActivated()
        {
            _isActivated = true;
            yield return new WaitForSeconds(DELAY_TRIGGERED);
            _isActivated = false;
        }
    }
}