using BioEngineerLab.Activities;
using Core;
using Mechanics;
using Saveables;
using UnityEngine;
using System;
using Machines;
using UI.Components;
using Unity.VisualScripting;

using Database;

namespace Gameplay
{
    public class Potholder : MonoBehaviour, ISaveableOther
    {
        private class SavedData
        {
            public bool IsActive = true;
        }
        
        [Header("Refs")]
        [SerializeField] private MeshRenderer _potholderRenderer;
        [SerializeField] private VRGrabInteractable _grabInteractable;
        [SerializeField] private Collider _collider;
        [SerializeField] private ButtonComponent _button;
        
        [Space]
        [Header("Configs")]
        [SerializeField] private EMachine _machineType;
        
        private HandsChanger _handsChanger;
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

            _handsChanger = gameManager.PlayerSpawner.Player.HandsChanger;
        }
 
        private void OnEnable()
        {
            _grabInteractable.GrabbedEvent += OnGrab;
            _button.ClickBtnEvent += OnButtonClick;
        }

        private void OnDisable()
        {
            _grabInteractable.GrabbedEvent -= OnGrab;
            _button.ClickBtnEvent -= OnButtonClick;
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
            
            if (gameManager.IsSoundsOn)
            {
                AudioClip a = ResourcesDatabase.ReadSound("Grab");
                AudioSource.PlayClipAtPoint(a, transform.position, 0.7f);
            }
            
            _isActive = false;
            _potholderRenderer.enabled = _isActive;
            _collider.enabled = _isActive;
            _button.gameObject.SetActive(!_isActive);
            
            gameManager.CurrentBaseLocalManager.OnActivityComplete(new MachineLabActivity(EMachineActivity.OnEnter, _machineType));    
            
            _handsChanger.WearPotholder();
        }

        private void OnButtonClick()
        {
            _handsChanger.TakePotholderOff();
            
            _isActive = true;
            _potholderRenderer.enabled = _isActive;
            _collider.enabled = _isActive;
            _button.gameObject.SetActive(!_isActive);
        }

        public void Save()
        {
            _savedData.IsActive = _isActive;
        }

        public void Load()
        {
            _isActive = _savedData.IsActive;
            _potholderRenderer.enabled = _isActive;
            _collider.enabled = _isActive;
            _button.gameObject.SetActive(!_isActive);
        }
    }
}