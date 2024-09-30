using BioEngineerLab.Containers;
using UnityEngine;

namespace BioEngineerLab.Gameplay
{
    [RequireComponent(typeof(Animator), typeof(VRGrabInteractable), typeof(Rigidbody))]
    public class Anchor : MonoBehaviour
    {
        [SerializeField] private Collider _collider;
        
        public Collider Collider { get; private set; }
        public Rigidbody Rigidbody { get; private set; }

        private Animator _animator;
        private VRGrabInteractable _grabInteractable;
        
        private void Awake()
        {
            Collider = _collider;
                
            Rigidbody = GetComponent<Rigidbody>();
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

        public void ToggleAnimate(bool isEnable)
        {
            _animator.enabled = isEnable;
        }
    }
}