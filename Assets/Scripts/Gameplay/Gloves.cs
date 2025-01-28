using BioEngineerLab.Activities;
using Core;
using Mechanics;
using Saveables;
using UnityEngine;
using System;
using Machines;

namespace Gameplay
{
    public class Gloves : MonoBehaviour, ISaveableOther
    {
        private class SavedData
        {
            public bool IsActive = true;
        }

        public event Action GlovesTakeEvent;
        
        [Header("Refs")]
        [SerializeField] private MeshRenderer _meshRendererL;
        [SerializeField] private MeshRenderer _meshRendererR;
        [SerializeField] private VRGrabInteractable _grabInteractable;
        
        [Space]
        [Header("Configs")]
        [SerializeField] private EMachine _machineType;
        
        private HandsMachine _handsMachine;
        private SavedData _savedData = new SavedData();
        private bool _isActive = true;
        
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

            _handsMachine = FindObjectOfType<HandsMachine>();
        }
 
        private void OnEnable()
        {
            _grabInteractable.GrabbedEvent += OnGrab;
        }

        private void OnDisable()
        {
            _grabInteractable.GrabbedEvent -= OnGrab;
        }

        private void OnGrab()
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
            
            _isActive = false;
            _meshRendererL.enabled = _isActive;
            _meshRendererR.enabled = _isActive;
            GetComponent<Collider>().enabled = _isActive;
            
            gameManager.CurrentBaseLocalManager.OnActivityComplete(new MachineLabActivity(EMachineActivity.OnEnter, _machineType));    
            
            _handsMachine.WearGloves();
        }

        public void Save()
        {
            _savedData.IsActive = _isActive;
        }

        public void Load()
        {
            _isActive = _savedData.IsActive;
            _meshRendererL.enabled = _isActive;
            _meshRendererR.enabled = _isActive;
            GetComponent<Collider>().enabled = _isActive;
        }
    }
}