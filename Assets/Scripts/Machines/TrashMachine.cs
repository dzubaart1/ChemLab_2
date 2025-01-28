using System.Collections.Generic;
using BioEngineerLab.Activities;
using Core;
using JetBrains.Annotations;
using Mechanics;
using Saveables;
using UI.Components;
using Unity.VisualScripting;
using UnityEngine;

namespace Machines
{
    [RequireComponent(typeof(Collider))]
    public class TrashMachine : MonoBehaviour, ISaveableOther
    {
        private class SavedData
        {
            public List<VRGrabInteractable> HiddenGameObjects = new List<VRGrabInteractable>();
        }

        [SerializeField] private EMachine _machineType;
        [SerializeField] [CanBeNull] private ButtonComponent _button;
        
        private HandsMachine _handsMachine;
        private SavedData _savedData = new SavedData();
        private List<VRGrabInteractable> _hiddenGameObjects = new List<VRGrabInteractable>();
        
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
            if (_button == null)
            {
                return;
            }
            
            _button.ClickBtnEvent += OnButtonClicked;
        }

        private void OnDisable()
        {
            if (_button == null)
            {
                return;
            }
            
            _button.ClickBtnEvent -= OnButtonClicked;
        }

        private void OnButtonClicked()
        {
            if (_button.ButtonType == EButton.TrashGloversButton)
            {
                if (_handsMachine == null)
                {
                    return;
                }
                _handsMachine.TakeGlovesOff();
            }
        }

        private void OnTriggerEnter(Collider other)
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
            
            VRGrabInteractable interactable = other.GetComponentInParent<VRGrabInteractable>();

            if (interactable == null)
            {
                return;
            }
            
            interactable.gameObject.SetActive(false);
            _hiddenGameObjects.Add(interactable);
            
            gameManager.CurrentBaseLocalManager.OnActivityComplete(new MachineLabActivity(EMachineActivity.OnEnter, _machineType));
        }

        public void Save()
        {
            _savedData.HiddenGameObjects.Clear();
            
            foreach (var gameObject in _hiddenGameObjects)
            {
                _savedData.HiddenGameObjects.Add(gameObject);
            }
        }

        public void Load()
        {
            foreach (var interactable in _hiddenGameObjects)
            {
                interactable.gameObject.SetActive(true);
            }

            foreach (var interactable in _savedData.HiddenGameObjects)
            {
                interactable.gameObject.SetActive(false);
            }
            
            _hiddenGameObjects.Clear();

            foreach (var interactable in _savedData.HiddenGameObjects)
            {
                _hiddenGameObjects.Add(interactable);
            }
        }
    }
}