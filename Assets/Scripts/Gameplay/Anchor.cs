using Containers;
using Mechanics;
using UnityEngine;

namespace Gameplay
{
    public class Anchor : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private VRGrabInteractable _grabInteractable;
        [SerializeField] private Collider _collider;
        [SerializeField] private Rigidbody _rigidbody;

        public bool IsAnimating => _animator.enabled;
        
        private void OnTriggerStay(Collider other)
        {
            LabContainer labContainer = other.GetComponent<LabContainer>();
            
            if (labContainer is null)
            {
                return;
            }

            if (_grabInteractable.isSelected)
            {
                return;
            }
            
            labContainer.PutAnchor(this);
        }

        public void TogglePhysics(bool isOn)
        {
            _rigidbody.useGravity = isOn;
            _rigidbody.isKinematic = !isOn;
            
            _collider.enabled = isOn;
        }

        public void ToggleAnimate(bool isEnable)
        {
            _animator.enabled = isEnable;
        }
    }
}