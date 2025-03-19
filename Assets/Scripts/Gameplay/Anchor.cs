using System;
using BioEngineerLab.Activities;
using Containers;
using Core;
using Mechanics;
using Saveables;
using UnityEngine;

namespace Gameplay
{
    public class Anchor : MonoBehaviour, ISaveableOther
    {
        private class SavedData
        {
            public bool IsAnimating;
        }
        
        [SerializeField] private Animator _animator;
        [SerializeField] private VRGrabInteractable _grabInteractable;
        [SerializeField] private Collider _collider;
        [SerializeField] private Rigidbody _rigidbody;

        public bool IsAnimating => _animator.enabled;

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
            
            gameManager.CurrentBaseLocalManager.AddSaveableOther(this);
        }

        private void OnTriggerStay(Collider other)
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
            
            LabContainer labContainer = other.GetComponent<LabContainer>();
            
            if (labContainer is null)
            {
                return;
            }

            if (_grabInteractable.isSelected)
            {
                return;
            }
            
            if(labContainer.TryPutAnchor(this))
            {
                gameManager.CurrentBaseLocalManager.OnActivityComplete(new AnchorLabActivity(labContainer.ContainerType));   
            }
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

        public void Save()
        {
            _savedData.IsAnimating = _animator.enabled;
        }

        public void Load()
        {
            _animator.enabled = _savedData.IsAnimating;
        }
    }
}