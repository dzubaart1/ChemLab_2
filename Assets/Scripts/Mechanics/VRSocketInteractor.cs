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
            public VRGrabInteractable LockedObject;
            public bool IsLocked;
        }

        public event Action<Transform> ExitedTransformEvent;
        public event Action<Transform> EnteredTransformEvent;
        
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
                if (firstInteractableSelected != null)
                {
                    return firstInteractableSelected.transform;;
                }

                if (_isLocked && _lockedObject != null)
                {
                    return _lockedObject.transform;
                }

                return null;
            }
        }

        public ESocket SocketType => _socketType;

        [CanBeNull] private VRGrabInteractable _lockedObject;
        
        private SavedData _savedData = new SavedData();

        private bool _isLocked = false;
        private bool _isTimerActive = false;
        private float _timer = 0f;
        
        private void Update()
        {
            if (_isTimerActive)
            {
                _timer += Time.deltaTime;

                if (_timer > _timerDelay)
                {
                    _isTimerActive = false;
                }
            }

            if (_isLocked && _lockedObject != null)
            {
                _lockedObject.transform.localPosition = Vector3.zero;
                _lockedObject.transform.localRotation = Quaternion.identity;
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
                Debug.Log($" TRY COMPLETE ENTER! {_socketType}");
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

            if (_isTimerActive)
            {
                return;
            }
            
            ExitedTransformEvent?.Invoke(exitedTransform);

            if (_isExitTaskSendable)
            {
                Debug.Log($" TRY COMPLETE EXIT! {gameObject.name}  {_socketType}");
                SendTryTaskComplete(exitedTransform, ESocketActivity.Exit);
            }
        }
        
        public void Save()
        {
            _savedData.IsLocked = _isLocked;
            _savedData.LockedObject = _lockedObject;
            _savedData.GrabbedObject = firstInteractableSelected as VRGrabInteractable;
        }

        public void ReleaseAllLoad()
        {
            if (SelectedObject == null)
            {
                return;
            }
            
            RestartTimer();
            
            for (var i = interactablesSelected.Count - 1; i >= 0; --i)
            {
                interactionManager.SelectCancel(this, interactablesSelected[i]);
            }
        }

        public void PutSavedInteractable()
        {
            if (_savedData.GrabbedObject == null)
            {
                return;
            }
            
            RestartTimer();
            
            interactionManager.FocusEnter(this, _savedData.GrabbedObject);
            if (CanHover((IXRHoverInteractable)_savedData.GrabbedObject))
            {
                interactionManager.HoverEnter(this, (IXRHoverInteractable)_savedData.GrabbedObject);   
            }
            
            _savedData.GrabbedObject.transform.position = attachTransform.position;
            _savedData.GrabbedObject.transform.rotation = attachTransform.rotation;
        }

        public void PutSavedLocks()
        {
            if (!_savedData.IsLocked)
            {
                return;
            }

            if (_savedData.LockedObject == null)
            {
                return;
            }
            
            MakeLock(_savedData.LockedObject);
        }

        public void ReleaseLocks()
        {
            if (!_isLocked)
            {
                return;
            }
            
            if (_lockedObject == null)
            {
                return;
            }
            
            MakeUnlock(_lockedObject);
        }

        public void LockSelected()
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
            
            MakeLock(vrGrabInteractable);
        }
        
        public void UnlockSelectedObject()
        {
            if (_lockedObject == null)
            {
                return;
            }
            
            MakeUnlock(_lockedObject);
        }

        private void MakeLock(VRGrabInteractable grabInteractable)
        {
            if (_isLocked)
            {
                return;
            }
            
            Rigidbody rigidbody = grabInteractable.GetComponentInChildren<Rigidbody>();
            if (rigidbody == null)
            {
                return;
            }

            Collider[] colliders = grabInteractable.GetComponentsInChildren<Collider>();
            if (colliders.Length == 0)
            {
                return;
            }
            
            RestartTimer();

            foreach (var collider in colliders)
            {
                if (!collider.isTrigger)
                {
                    collider.enabled = false;
                }
            }
            
            grabInteractable.enabled = false;
            rigidbody.isKinematic = true;
            
            socketActive = false;
            
            grabInteractable.transform.SetParent(attachTransform);
            
            _isLocked = true;
            _lockedObject = grabInteractable;
        }

        private void MakeUnlock(VRGrabInteractable grabInteractable)
        {
            if (!_isLocked)
            {
                return;
            }

            Rigidbody rigidbody = grabInteractable.GetComponentInChildren<Rigidbody>();
            if (rigidbody == null)
            {
                return;
            }

            Collider[] colliders = grabInteractable.GetComponentsInChildren<Collider>();
            if (colliders.Length == 0)
            {
                return;
            }

            Collider socketCollider = GetComponent<Collider>();
            if (socketCollider == null)
            {
                return;
            }
            
            RestartTimer();
            
            foreach (var collider in colliders)
            {
                collider.enabled = true;
            }
            
            grabInteractable.transform.SetParent(null);
            grabInteractable.enabled = true;
            
            rigidbody.isKinematic = false;
            
            socketActive = true;
            socketCollider.enabled = true;
            
            interactionManager.SelectEnter((IXRSelectInteractor)this, grabInteractable);

            _isLocked = false;
            _lockedObject = null;
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

        private void RestartTimer()
        {
            _isTimerActive = true;
            _timer = 0f;
        }
    }
}