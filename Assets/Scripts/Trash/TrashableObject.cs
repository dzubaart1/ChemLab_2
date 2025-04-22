using System.Collections.Generic;
using BioEngineerLab.Activities;
using Containers;
using Core;
using JetBrains.Annotations;
using Mechanics;
using Machines;
using Saveables;
using UI.Components;
using UnityEngine;

namespace Trash
{
    public class TrashableObject : MonoBehaviour
    {
        [SerializeField] private ETrashableObject _trashableObjectType;

        public ETrashableObject TrashableObjectType => _trashableObjectType;

        public void SetTreshableActive(bool isActive)
        {
            MeshRenderer meshRenderer = GetComponentInChildren<MeshRenderer>();
            if (meshRenderer == null)
            {
                return;
            }

            Collider[] colliders = GetComponentsInChildren<Collider>();
            if (colliders == null || colliders.Length == 0)
            {
                return;
            }

            Rigidbody tempRB = GetComponentInChildren<Rigidbody>();
            if (tempRB == null)
            {
                return;
            }

            tempRB.useGravity = isActive;
            tempRB.isKinematic = !isActive;

            meshRenderer.enabled = isActive;
            foreach (var tempCollider in colliders)
            {
                tempCollider.enabled = isActive;
            }
        }
    }
}