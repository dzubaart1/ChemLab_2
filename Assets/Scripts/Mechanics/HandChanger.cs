using System;
using System;
using System.Linq;
using Core;
using Gameplay;
using JetBrains.Annotations;
using Saveables;
using UI.Components;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Machines
{
    [RequireComponent(typeof(Collider))]
    public class HandsChanger : MonoBehaviour, ISaveableOther
    {
        private class SavedData
        {
            public bool IsGloves;
            public bool IsPotholder;
        }

        [Header("Materials")]
        [SerializeField] private Material _glovesMaterial;
        [SerializeField] private Material _handsMaterial;
        [SerializeField] private Material _potholderMaterial;
        
        [Header("References")]
        [SerializeField] private SkinnedMeshRenderer _rightHandMesh;
        [SerializeField] private SkinnedMeshRenderer _leftHandMesh;
        
        private bool _isGloves = false;
        private bool _isPotholder = false;
        private SavedData _savedData = new SavedData();

        public void Init()
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

            if (gameManager.CurrentLab == ELab.Lab1)
            {
                WearGloves();
            }
            else
            {
                TakeGlovesOff();
            }
        }

        public void WearGloves()
        {
            if (_isPotholder)
            {
                _rightHandMesh.materials = new Material[] {_glovesMaterial, _potholderMaterial};
            }
            else
            {
                _rightHandMesh.materials = new Material[] {_glovesMaterial};
            }
            _leftHandMesh.material = _glovesMaterial;
            
            _isGloves = true;
        }

        public void TakeGlovesOff()
        {
            if (_isPotholder)
            {
                _rightHandMesh.materials = new Material[] {_handsMaterial, _potholderMaterial};
            }
            else
            {
                _rightHandMesh.materials = new Material[] {_handsMaterial};
            }
            _leftHandMesh.material = _handsMaterial;
            
            _isGloves = false;
        }

        public void WearPotholder()
        {
            _rightHandMesh.materials = new Material[] {_isGloves ? _glovesMaterial : _handsMaterial, _potholderMaterial};
            
            _isPotholder = true;
        }

        public void TakePotholderOff()
        {
            _rightHandMesh.materials = new Material[] {_isGloves ? _glovesMaterial : _handsMaterial};
            
            _isPotholder = false;
        }

        public void Save()
        {
            _savedData.IsGloves = _isGloves;
            _savedData.IsPotholder = _isPotholder;
        }

        public void Load()
        {
            _isGloves = _savedData.IsGloves;
            _isPotholder = _savedData.IsPotholder;
            
            if (_isPotholder)
            {
                _rightHandMesh.materials = new Material[] {_isGloves ? _glovesMaterial : _handsMaterial, _potholderMaterial};
            }
            else
            {
                _rightHandMesh.materials = new Material[] {_isGloves ? _glovesMaterial : _handsMaterial};
            }
            _leftHandMesh.material = _isGloves ? _glovesMaterial : _handsMaterial;
        }
    }
}