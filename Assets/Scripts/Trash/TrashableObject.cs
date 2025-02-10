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
    }
}