using BioEngineerLab.Activities;
using Core;
using Mechanics;
using Saveables;
using UI.Components;
using UnityEngine;

namespace Gameplay
{
    public class Coat : MonoBehaviour, ISaveableOther
    {
        private class SavedData
        {
            public bool IsActive = true;
        }
        
        [Header("Refs")]
        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private VRGrabInteractable _grabInteractable;
        [SerializeField] private ButtonComponent _returnBtn;
        
        [Space]
        [Header("Configs")]
        [SerializeField] private EMachine _machineType;

        private SavedData _savedData = new SavedData();
        
        private bool _isActive = true;
 
        private void OnEnable()
        {
            _returnBtn.ClickBtnEvent += OnReturnBtnClicked;
            _grabInteractable.GrabbedEvent += OnGrab;
        }

        private void OnDisable()
        {
            _returnBtn.ClickBtnEvent -= OnReturnBtnClicked;
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
            _meshRenderer.enabled = _isActive;
            
            gameManager.CurrentBaseLocalManager.OnActivityComplete(new MachineLabActivity(EMachineActivity.OnEnter, _machineType));            
        }

        public void Save()
        {
            _savedData.IsActive = _isActive;
        }

        public void Load()
        {
            _isActive = _savedData.IsActive;
            _meshRenderer.enabled = _isActive;
        }

        private void OnReturnBtnClicked()
        {
            _isActive = true;
            _meshRenderer.enabled = _isActive;
        }
    }
}