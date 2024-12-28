using System;
using BioEngineerLab.Activities;
using Containers;
using Core;
using JetBrains.Annotations;
using Saveables;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Mechanics
{
    public class VRSocketInteractor : XRSocketInteractor, ISaveableSocket
    {
        private struct SavedData
        {
            public VRGrabInteractable GrabbedObject;
        }

        public event Action<Transform> ExitedTransformEvent;
        public event Action<Transform> EnteredTransformEvent;
        
        [Header("Refs")]
        [SerializeField] private Collider[] _scoketColliders;
        
        [Space]
        [Header("Configs")]
        [SerializeField] private ESocket _socketType;
        [SerializeField] private bool _isEnterTaskSendable;
        [SerializeField] private bool _isExitTaskSendable;
        [SerializeField] private float _timerDelay = 1f;

        [SerializeField] private bool _isSubstanceSocket;
        [SerializeField] private bool _isStartEnter;

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

        public ESocket SocketType => _socketType;

        private SavedData _savedData = new SavedData();

        private bool _isLoadExit = false;
        private bool _isLoadEnter = false;

        private void Start()
        {
            GameManager gameManager = GameManager.Instance;
            if (gameManager == null)
            {
                return;
            }

            if (gameManager.CurrentBaseLocalManager == null)
            {
                return;
            }
            
            gameManager.CurrentBaseLocalManager.AddSaveableSocket(this);
        }

        protected override void OnSelectEntered(SelectEnterEventArgs args)
        {
            base.OnSelectEntered(args);

            if (SelectedObject == null)
            {
                return;
            }
            
            SocketCollisionsIgnored(SelectedObject, true);
            
            Debug.Log($"ENTER {transform.parent.name}");

            if (_isStartEnter)
            {
                _isStartEnter = false;
                return;
            }

            if (_isLoadEnter)
            {
                _isLoadEnter = false;
                return;
            }

            EnteredTransformEvent?.Invoke(SelectedObject);
            
            if (_isEnterTaskSendable)
            {
                SendTryTaskComplete(SelectedObject, ESocketActivity.Enter);
            }
        }

        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            base.OnSelectExited(args);

            IXRSelectInteractable exitedInteractable = args.interactableObject;

            if (exitedInteractable is null)
            {
                return;
            }
            
            Debug.Log($"EXIT {transform.parent.name}");

            Transform exitedTransform = exitedInteractable.transform;

            SocketCollisionsIgnored(exitedTransform, false);

            if (_isLoadExit)
            {
                _isLoadExit = false;
                return;
            }
            
            ExitedTransformEvent?.Invoke(exitedTransform);

            if (_isExitTaskSendable)
            {
                SendTryTaskComplete(exitedTransform, ESocketActivity.Exit);
            }
        }
        
        public void Save()
        {
            _savedData.GrabbedObject = firstInteractableSelected as VRGrabInteractable;
        }

        public void ReleaseAllLoad()
        {
            if (SelectedObject == null)
            {
                return;
            }
            
            VRGrabInteractable vrGrabInteractable = SelectedObject.GetComponentInChildren<VRGrabInteractable>();
            if (vrGrabInteractable == null)
            {
                return;
            }
            
            Debug.Log($"RELEASE LOAD {transform.name}");
            _isLoadExit = true;
            
            interactionManager.SelectCancel((IXRSelectInteractor)this, vrGrabInteractable);
            vrGrabInteractable.TurnOffCollidersInSeconds(1f);
            vrGrabInteractable.transform.position = Vector3.zero;
        }

        public void PutSavedInteractable()
        {
            if (_savedData.GrabbedObject == null)
            {
                return;
            }
            
            Debug.Log($"PUT SAVED LOAD {transform.parent.name}");

            _isLoadEnter = true;
            interactionManager.SelectEnter((IXRSelectInteractor)this, _savedData.GrabbedObject);
        }
        
        private void SendTryTaskComplete(Transform objectTransform, ESocketActivity socketActivity)
        {
            GameManager gameManager = GameManager.Instance;
            
            if (gameManager == null)
            {
                return;
            }

            if (gameManager.CurrentBaseLocalManager == null)
            {
                return;
            }

            if (_isSubstanceSocket)
            {
                LabContainer labContainer = objectTransform.GetComponent<LabContainer>();
                if (labContainer == null)
                {
                    return;
                }

                gameManager.CurrentBaseLocalManager.OnActivityComplete(new SocketSubstancesLabActivity(_socketType, socketActivity, labContainer.GetSubstanceProperties()));
                return;
            }

            if (!_isSubstanceSocket)
            {
                gameManager.CurrentBaseLocalManager.OnActivityComplete(new SocketLabActivity(_socketType, socketActivity));
                return;
            }
        }

        
        private void SocketCollisionsIgnored(Transform targetObject, bool isIgnored)
        {
            Collider[] targetObjectColliders = targetObject.GetComponentsInChildren<Collider>(true);

            foreach (var socketCollider in _scoketColliders)
            {
                foreach (var targetObjectCollider in targetObjectColliders)
                {
                    Physics.IgnoreCollision(socketCollider, targetObjectCollider, isIgnored);
                }
            }
        }
    }
}