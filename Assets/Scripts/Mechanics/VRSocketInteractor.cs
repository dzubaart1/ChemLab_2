using System;
using System.Collections;
using BioEngineerLab.Activities;
using Containers;
using Core;
using Core.Services;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Mechanics
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

        [SerializeField] private bool _isSubsctanceEnterSocket;
        [SerializeField] private bool _isSubsctanceExitSocket;

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

        [CanBeNull] private GameManager _gameManager;

        private SavedData _savedData = new SavedData();
        
        private bool _isLoadSceneEnter;
        private bool _isLoadSceneExit;
        [SerializeField] private bool _isStartEnter;

        protected override void Awake()
        {
            base.Awake();

            ESocket socket = _socketType;
            _gameManager = GameManager.Instance;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.Game.SaveGameEvent += OnSaveScene;
            _gameManager.Game.LoadGameEvent += OnLoadScene;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.Game.SaveGameEvent -= OnSaveScene;
            _gameManager.Game.LoadGameEvent -= OnLoadScene;
        }

        protected override void Start()
        {
            base.Start();

            OnSaveScene();
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);
            
            var other = args.interactableObject.transform.gameObject;

            if (args.interactableObject is null)
            {
                return;
            }
            
            SocketCollisionsIgnored(other, true);
            
            if (!_isEnterTaskSendable)
            {
                return;
            }

            if (SelectedObject == null)
            {
                return;
            }            
            
            if (_isStartEnter)
            {
                _isStartEnter = false;
                return;
            }

            if (_isLoadSceneEnter)
            {
                _isLoadSceneEnter = false;
                return;
            }
            
            SendTryTaskComplete(SelectedObject, ESocketActivity.Enter);
        }
        
        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);
            
            var other = args.interactableObject.transform.gameObject;

            if (args.interactableObject is null)
            {
                return;
            }
            
            SocketCollisionsIgnored(other, false);

            if (_gameManager == null)
            {
                return;
            }
            
            if (!_isExitTaskSendable)
            {
                return;
            }
            
            if (_isLoadSceneExit)
            {
                _isLoadSceneExit = false;
                return;
            }
            
            /*_gameManager.CompleteTask(new SocketLabActivity(_socketType, ESocketActivity.Exit));*/
            SendTryTaskComplete(args.interactableObject.transform, ESocketActivity.Exit);
        }
        
        private void SocketCollisionsIgnored(GameObject other, bool flag)
        {        
            GameObject parent = transform.parent.gameObject;
            var myColliders = parent.GetComponentsInChildren<Collider>(true);
            var theirColliders = other.GetComponentsInChildren<Collider>(true);
    
            foreach (var cA in myColliders)
            foreach (var cB in theirColliders)
                Physics.IgnoreCollision(cA, cB, flag);
        }
        
        private void SendTryTaskComplete(Transform objectTransform, ESocketActivity socketActivity)
        {
            if (_gameManager == null)
            {
                return;
            }
            
            if (_isSubsctanceEnterSocket && socketActivity == ESocketActivity.Enter ||
                _isSubsctanceExitSocket && socketActivity == ESocketActivity.Exit)
            {
                LabContainer labContainer = objectTransform.GetComponent<LabContainer>();
                if (labContainer == null)
                {
                    return;
                }
                
                _gameManager.CompleteTask(new SocketSubstancesLabActivity(_socketType, socketActivity, labContainer.GetSubstanceProperties()));
                return;
            }
            
            _gameManager.CompleteTask(new SocketLabActivity(_socketType, socketActivity));
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