using System;
using BioEngineerLab.Activities;
using Core;
using JetBrains.Annotations;
using Mechanics;
using Saveables;
using UnityEngine;

namespace Machines
{
    public class DoorHandlePress : MonoBehaviour
    {
        
        [Header("Refs")]
        [SerializeField] private VRGrabInteractable _vrGrabInteractable;
        [SerializeField] private Animator _doorAnimator;

        private void OnEnable()
        {
            _vrGrabInteractable.GrabbedEvent += OnGrab;
            _vrGrabInteractable.UngrabbedEvent += OnUngrab;
        }

        private void OnDisable()
        {
            _vrGrabInteractable.GrabbedEvent -= OnGrab;
            _vrGrabInteractable.UngrabbedEvent -= OnUngrab;
        }

        private void OnGrab()
        {
            _doorAnimator.Play("Press");
        }
        
        private void OnUngrab()
        {
            _doorAnimator.Play("Release");
        }
    }
}