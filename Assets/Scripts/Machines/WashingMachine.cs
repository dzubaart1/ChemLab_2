using System.Collections.Generic;
using BioEngineerLab.Activities;
using Containers;
using Core;
using Mechanics;
using Saveables;
using UnityEngine;

namespace Machines
{
    [RequireComponent(typeof(Collider))]
    public class WashingMachine : MonoBehaviour, ISaveableOther
    {
        private class SavedData
        {
            public List<VRGrabInteractable> HiddenGameObjects = new List<VRGrabInteractable>();
        }
        
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
            LabContainer container = other.GetComponentInParent<LabContainer>();

            if (interactable == null | container == null)
            {
                return;
            }
            
            other.gameObject.SetActive(false);
            _hiddenGameObjects.Add(interactable);
            
            gameManager.CurrentBaseLocalManager.OnActivityComplete(new MachineLabActivity(EMachineActivity.OnEnter, EMachine.WashingMachine));
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