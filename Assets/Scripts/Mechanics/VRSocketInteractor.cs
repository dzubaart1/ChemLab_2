using System.Collections;
using BioEngineerLab.Activities;
using BioEngineerLab.Containers;
using BioEngineerLab.Core;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace BioEngineerLab.Gameplay
{
    public class VRSocketInteractor : XRSocketInteractor, ISaveable
    {
        private struct SavedData
        {
            public VRGrabInteractable GrabbedObject;
        }

        [Header("Configs")]
        [SerializeField] private ESocket _socketType;
        [SerializeField] private bool _isEnterTaskSendable;
        [SerializeField] private bool _isExitTaskSendable;
        
        [CanBeNull]
        public Transform SelectedObject
        {
            get
            {
                if (firstInteractableSelected == null)
                {
                    return null;
                }

                return firstInteractableSelected.transform;
            }
        }
        
        private TasksService _tasksService;
        private SaveService _saveService;

        private SavedData _savedData = new SavedData();
        private bool _isLoadSceneEnter;
        private bool _isLoadSceneExit;

        private bool _isStartEnter;

        protected override void Awake()
        {
            base.Awake();

            _tasksService = Engine.GetService<TasksService>();
            _saveService = Engine.GetService<SaveService>();

            _isStartEnter = startingSelectedInteractable != null;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            
            _saveService.SaveSceneStateEvent += OnSaveScene;
            _saveService.LoadSceneStateEvent += OnLoadScene;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            _saveService.SaveSceneStateEvent -= OnSaveScene;
            _saveService.LoadSceneStateEvent -= OnLoadScene;
        }

        protected override void Start()
        {
            base.Start();

            OnSaveScene();
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);

            if (args.interactableObject == null)
            {
                return;
            }

            LabContainer container = args.interactableObject.transform.GetComponent<LabContainer>();

            if (container == null)
            {
                return;
            }
            
            if (_isStartEnter)
            {
                _isStartEnter = !_isStartEnter;
                return;
            }

            if (_isLoadSceneEnter)
            {
                _isLoadSceneEnter = false;
                return;
            }

            if (_isEnterTaskSendable)
            {
                _tasksService.TryCompleteTask(new SocketSubstancesLabActivity(_socketType, ESocketActivity.Enter, container.GetSubstanceProperties()));
            }
        }

        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);

            if (args.interactableObject == null)
            {
                return;
            }

            LabContainer container = args.interactableObject.transform.GetComponent<LabContainer>();

            if (container == null)
            {
                return;
            }
            
            if (_isLoadSceneExit)
            {
                _isLoadSceneExit = false;
                return;
            }

            if (_isExitTaskSendable)
            {
                _tasksService.TryCompleteTask(new SocketSubstancesLabActivity(_socketType, ESocketActivity.Exit, container.GetSubstanceProperties()));
            }
        }

        public void OnSaveScene()
        {
            _savedData.GrabbedObject = firstInteractableSelected as VRGrabInteractable;
        }

        public void OnLoadScene()
        {
            VRGrabInteractable localGrabInteractable = firstInteractableSelected as VRGrabInteractable;

            if (_savedData.GrabbedObject == null && localGrabInteractable == null)
            {
                return;
            }

            if (_savedData.GrabbedObject == localGrabInteractable)
            {
                return;
            }

            if (_savedData.GrabbedObject == null && localGrabInteractable != null)
            {
                _isLoadSceneExit = true;

                StartCoroutine(ForceDeselect(localGrabInteractable, this));
                return;
            }

            if (_savedData.GrabbedObject != null && localGrabInteractable != null &&
                _savedData.GrabbedObject.isSelected)
            {
                _isLoadSceneExit = true;
                _isLoadSceneEnter = true;

                StartCoroutine(ForceDeselect(_savedData.GrabbedObject,
                    _savedData.GrabbedObject.firstInteractorSelecting));
                StartCoroutine(ForceDeselect(localGrabInteractable,
                    this));
                interactionManager.SelectEnter((IXRSelectInteractor)this, _savedData.GrabbedObject);
                return;
            }

            if (_savedData.GrabbedObject != null && localGrabInteractable != null &&
                !_savedData.GrabbedObject.isSelected)
            {
                _isLoadSceneExit = true;
                _isLoadSceneEnter = true;

                StartCoroutine(ForceDeselect(localGrabInteractable, this));
                interactionManager.SelectEnter((IXRSelectInteractor)this, _savedData.GrabbedObject);
                return;
            }

            if (_savedData.GrabbedObject != null && localGrabInteractable == null &&
                !_savedData.GrabbedObject.isSelected)
            {
                _isLoadSceneEnter = true;

                interactionManager.SelectEnter((IXRSelectInteractor)this, _savedData.GrabbedObject);
                return;
            }

            if (_savedData.GrabbedObject != null && firstInteractableSelected == null &&
                _savedData.GrabbedObject.isSelected)
            {
                _isLoadSceneEnter = true;

                StartCoroutine(ForceTransfer(_savedData.GrabbedObject,
                    _savedData.GrabbedObject.firstInteractorSelecting, this));
                return;
            }
        }

        private IEnumerator ForceTransfer(VRGrabInteractable interactable, IXRSelectInteractor from, IXRSelectInteractor to)
        {
            interactionManager.CancelInteractorSelection(from);
            interactable.transform.GetComponent<XRBaseInteractable>().enabled = false;
            yield return null;
            interactable.transform.GetComponent<XRBaseInteractable>().enabled = true;
            interactionManager.SelectEnter(to, interactable);
        }

        private IEnumerator ForceDeselect(VRGrabInteractable interactable, IXRSelectInteractor interactor)
        {
            interactionManager.CancelInteractorSelection(interactor);
            interactable.transform.GetComponent<XRBaseInteractable>().enabled = false;
            yield return null;
            interactable.transform.GetComponent<XRBaseInteractable>().enabled = true;

            interactable.LoadPosition();
        }
    }
}