using System;
using System.Collections;
using Core;
using Saveables;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Mechanics
{
    public class VRGrabInteractable : XRGrabInteractable, ISaveableGrabInteractable
    {
        public event Action GrabbedEvent;
        public event Action UngrabbedEvent;
        
        private struct SavedData
        {
            public Vector3 Position;
            public Quaternion Rotation;
        }

        private SavedData _savedData = new SavedData();

        [SerializeField] private bool _isSaveble = true;
        
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
            
            if (_isSaveble)
            {
                gameManager.CurrentBaseLocalManager.AddGrabInteractables(this);
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            
            selectEntered.AddListener(OnGrab);
            selectExited.AddListener(OnUnGrab);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            selectEntered.RemoveListener(OnGrab);
            selectExited.RemoveListener(OnUnGrab);
        }

        public void Save()
        {
            _savedData.Position = transform.position;
            _savedData.Rotation = transform.rotation;
        }

        public void LoadSavedTransform()
        {
            if (!_isSaveble)
            {
                return;
            }
            
            Rigidbody rigidbody = GetComponentInChildren<Rigidbody>();
            if (rigidbody == null)
            {
                return;
            }

            rigidbody.isKinematic = false;

            transform.position = _savedData.Position;
            transform.rotation = _savedData.Rotation;

            rigidbody.position = _savedData.Position;
            rigidbody.rotation = _savedData.Rotation;
        }
        
        private void OnGrab(SelectEnterEventArgs args)
        {
            GrabbedEvent?.Invoke();
        }

        private void OnUnGrab(SelectExitEventArgs args)
        {
            UngrabbedEvent?.Invoke();
        }
    }
}