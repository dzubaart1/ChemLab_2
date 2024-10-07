using BioEngineerLab.Containers;
using UnityEngine;

namespace BioEngineerLab.Gameplay
{
    [RequireComponent(typeof(Animator), typeof(VRGrabInteractable), typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class Anchor : MonoBehaviour
    {
        private Animator _animator;
        private VRGrabInteractable _grabInteractable;
        private Collider _collider;
        private Rigidbody _rigidbody;
        
        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _rigidbody = GetComponent<Rigidbody>();
            _grabInteractable = GetComponent<VRGrabInteractable>();
            _animator = GetComponent<Animator>();
        }

        private void OnTriggerStay(Collider other)
        {
            AnchorContainer anchorContainer = other.GetComponent<AnchorContainer>();
            
            if (anchorContainer is null || _grabInteractable.isSelected)
            {
                return;
            }
            
            anchorContainer.PutAnchor(this);
        }

        public void TogglePhysics(bool isOn)
        {
            _rigidbody.isKinematic = !isOn;
            _collider.enabled = isOn;
        }

        public void ToggleAnimate(bool isEnable)
        {
            _animator.enabled = isEnable;
        }
    }
}