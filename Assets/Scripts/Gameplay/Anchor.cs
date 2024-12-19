using System;
using Containers;
using Mechanics;
using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(Animator), typeof(VRGrabInteractable), typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class Anchor : MonoBehaviour
    {
        private Animator _animator;
        private VRGrabInteractable _grabInteractable;
        private Collider _collider;
        private Rigidbody _rigidbody;
        private bool _isGrabbed = false;
        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _rigidbody = GetComponent<Rigidbody>();
            _grabInteractable = GetComponent<VRGrabInteractable>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (_isGrabbed)
            {
                transform.position = transform.parent.position + Vector3.up;
            }
        } 

        private void OnTriggerStay(Collider other)
        {
            AnchorLabContainer anchorLabContainer = other.GetComponent<AnchorLabContainer>();
            
            if (anchorLabContainer is null || _grabInteractable.isSelected)
            {
                return;
            }
            
            anchorLabContainer.PutAnchor(this);
        }

        public void TogglePhysics(bool isOn)
        {
            _rigidbody.useGravity = isOn;
            _rigidbody.isKinematic = !isOn;
            
            _collider.enabled = isOn;
            
            _isGrabbed = isOn;
        }

        public void ToggleAnimate(bool isEnable)
        {
            _animator.enabled = isEnable;
        }
    }
}