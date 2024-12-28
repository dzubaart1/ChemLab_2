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
            
            gameManager.CurrentBaseLocalManager.AddGrabInteractables(this);
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
            Debug.Log($"TRANSFORM LOAD {transform.parent.name}");
            transform.position = _savedData.Position;
            transform.rotation = _savedData.Rotation;
        }

        public void TurnOffCollidersInSeconds(float sec)
        {
            StartCoroutine(Delay(sec));
        }
        
        private IEnumerator Delay(float sec)
        {
            Collider[] targetColliders = GetComponentsInChildren<Collider>();

            foreach (var c in targetColliders)
            {
                c.enabled = false;
            }
            
            yield return new WaitForSeconds(sec);
            
            foreach (var c in targetColliders)
            {
                c.enabled = true;
            }
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