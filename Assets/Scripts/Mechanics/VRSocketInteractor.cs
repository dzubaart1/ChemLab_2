using System;
using BioEngineerLab.Activities;
using Containers;
using Core;
using JetBrains.Annotations;
using Saveables;
using UnityEngine;
using UnityEngine.Serialization;
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
        
        [FormerlySerializedAs("_socketColliders")]
        [Header("Refs")]
        [SerializeField] private Collider[] _socketColliders;
        
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

        private bool _isTimerActive = false;
        private float _timer = 0f;
        
        private void Update()
        {
            if (!_isTimerActive)
            {
                return;
            }

            _timer += Time.deltaTime;

            if (_timer > _timerDelay)
            {
                _isTimerActive = false;
                _timer = 0f;
            }
        }

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

            if (_isStartEnter)
            {
                _isStartEnter = false;
                return;
            }

            if (_isTimerActive)
            {
                return;
            }

            EnteredTransformEvent?.Invoke(SelectedObject);
            
            if (_isEnterTaskSendable)
            {
                Debug.Log($" TRY COMPLETE ENTER!");
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

            Transform exitedTransform = exitedInteractable.transform;

            SocketCollisionsIgnored(exitedTransform, false);

            if (_isTimerActive)
            {
                return;
            }
            
            ExitedTransformEvent?.Invoke(exitedTransform);

            if (_isExitTaskSendable)
            {
                Debug.Log($" TRY COMPLETE EXIT!");
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
            
            for (var i = interactablesSelected.Count - 1; i >= 0; --i)
            {
                interactionManager.SelectCancel(this, interactablesSelected[i]);
            }

            _isTimerActive = true;
        }

        public void PutSavedInteractable()
        {
            if (_savedData.GrabbedObject == null)
            {
                return;
            }
            
            interactionManager.SelectEnter((IXRSelectInteractor)this, _savedData.GrabbedObject);

            _isTimerActive = false;
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

            foreach (var socketCollider in _socketColliders)
            {
                foreach (var targetObjectCollider in targetObjectColliders)
                {
                    Physics.IgnoreCollision(socketCollider, targetObjectCollider, isIgnored);
                }
            }
        }
    }
}