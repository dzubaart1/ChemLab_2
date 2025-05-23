using System;
using BioEngineerLab.Activities;
using Core;
using JetBrains.Annotations;
using Mechanics;
using Saveables;
using UnityEngine;

namespace Machines
{
    public class PipetBoxCap : MonoBehaviour
    {
        
        [Header("Refs")]
        [SerializeField] private VRGrabInteractable _vrGrabInteractable;
        [SerializeField] private Animator _capAnimator;

        private void OnEnable()
        {
            _vrGrabInteractable.GrabbedEvent += OnGrab;
        }

        private void OnDisable()
        {
            _vrGrabInteractable.GrabbedEvent -= OnGrab;
        }

        private void OnGrab()
        {
            _capAnimator.Play("Open");
        }
    }
}