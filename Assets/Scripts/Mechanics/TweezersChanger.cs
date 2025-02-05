using System;
using BioEngineerLab.Activities;
using Core;
using JetBrains.Annotations;
using Mechanics;
using Saveables;
using UnityEngine;

namespace Machines
{
    public class TweezersChanger : MonoBehaviour
    {
        
        [Header("Refs")]
        [SerializeField] private VRGrabInteractable _vrGrabInteractable;
        [SerializeField] private MeshRenderer _openMesh;
        [SerializeField] private MeshRenderer _closeMesh;

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
            _closeMesh.enabled = true;
            _openMesh.enabled = false;
        }
        
        private void OnUngrab()
        {
            _closeMesh.enabled = false;
            _openMesh.enabled = true;
        }
    }
}