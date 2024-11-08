using BioEngineerLab.Activities;
using BioEngineerLab.Containers;
using BioEngineerLab.Core;
using BioEngineerLab.Gameplay;
using BioEngineerLab.UI.Components;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

namespace BioEngineerLab.Machines
{
    public class DozatorMachine : MonoBehaviour
    {
        
        [SerializeField] private VRSocketInteractor _socketInteractor;
        [SerializeField] private ButtonComponent _dozatorBtn;
        [SerializeField] private Text _text;

        private TasksService _tasksService;
        private SaveService _saveService;
        private CraftService _craftService;

        private XRGrabInteractable _xrGrabInteractable;
        private LabContainer _labContainer;
        private CupSocketLabContainer _cupSocketLabContainer;

        private bool _isAlreadyTriggered;
        private const float DELAY_TRIGGERED = 0.5f;

        private void Awake()
        {
            _craftService = Engine.GetService<CraftService>();
            _tasksService = Engine.GetService<TasksService>();
            _saveService = Engine.GetService<SaveService>();

            _xrGrabInteractable = GetComponent<XRGrabInteractable>();
        }

        private void Start()
        {

        }

        private void OnEnable()
        {
            _socketInteractor.selectEntered.AddListener(OnEnter);
            _socketInteractor.selectExited.AddListener(OnExit);

            _dozatorBtn.OnClickButton += DozatorData;
        }

        private void OnDisable()
        {
            _socketInteractor.selectEntered.RemoveListener(OnEnter);
            _socketInteractor.selectExited.RemoveListener(OnExit);

            _dozatorBtn.OnClickButton -= DozatorData;
        }

        private void OnEnter(SelectEnterEventArgs args)
        {
            if (_socketInteractor.SelectedObject == null)
            {
                return;
            }

            _labContainer = _socketInteractor.SelectedObject.GetComponent<LabContainer>();
            _cupSocketLabContainer = _socketInteractor.SelectedObject.GetComponent<CupSocketLabContainer>();
        }

        private void OnExit(SelectExitEventArgs args)
        {

        }

        private void DozatorData()
        {

        }
        private void OnTriggerStay(Collider other)
        {
            if (_xrGrabInteractable.interactorsSelecting.Count == 0)
            {
                return;
            }

            if (_labContainer is null || _cupSocketLabContainer is null)
            {
                return;
            }

            if (_cupSocketLabContainer.IsClosed())
            {
                return;
            }

            LabContainer targetLabContainer = other.GetComponent<LabContainer>();
            if (targetLabContainer is null)
            {
                return;
            }

            CupSocketLabContainer targetCupSocketLabContainerCup = other.GetComponent<CupSocketLabContainer>();
            if (targetCupSocketLabContainerCup is null)
            {
                return;
            }

            if (targetCupSocketLabContainerCup.IsClosed())
            {
                return;
            }

            IXRSelectInteractor interactor = _xrGrabInteractable.interactorsSelecting[0];
            if (interactor is null)
            {
                return;
            }

            ActionBasedController controller = interactor.transform.GetComponent<ActionBasedController>();
            if (controller is null)
            {
                return;
            }

            if (!_isAlreadyTriggered & controller.activateAction.action.triggered)
            {
                _craftService.Transfer(_labContainer, targetLabContainer);
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
