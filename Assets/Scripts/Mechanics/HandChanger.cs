using System;
using System;
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
        }

        [Header("Materials")]
        [SerializeField] private Material _glovesMaterial;
        [SerializeField] private Material _handsMaterial;
        
        [Header("References")]
        [SerializeField] private SkinnedMeshRenderer _rightHandMesh;
        [SerializeField] private SkinnedMeshRenderer _leftHandMesh;
        
        [CanBeNull] private Gloves _gloves;
        [CanBeNull] private ButtonComponent _button;
        
        private bool _isGloves = false;
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

        

        public void WearGloves()
        {
            _rightHandMesh.material = _glovesMaterial;
            _leftHandMesh.material = _glovesMaterial;
            
            _isGloves = true;
        }

        public void TakeGlovesOff()
        {
            _rightHandMesh.material = _handsMaterial;
            _leftHandMesh.material = _handsMaterial;
            
            _isGloves = false;
        }

        public void Save()
        {
            _savedData.IsGloves = _isGloves;
        }

        public void Load()
        {
            _isGloves = _savedData.IsGloves;
            
            _rightHandMesh.material = _leftHandMesh.material = _isGloves ? _glovesMaterial : _handsMaterial;
        }
    }
}