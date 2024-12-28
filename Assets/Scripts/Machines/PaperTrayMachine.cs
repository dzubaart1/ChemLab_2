using System.Collections.Generic;
using BioEngineerLab.Activities;
using Core;
using Saveables;
using UnityEngine;

namespace Machines
{
    [RequireComponent(typeof(Collider))]
    public class PaperTrayMachine : MonoBehaviour, ISaveableOther
    {
        private class SavedData
        {
            public List<Transform> HiddenGameObjects = new List<Transform>();
        }
        
        private SavedData _savedData = new SavedData();
        
        private List<Transform> _hiddenGameObjects = new List<Transform>();
        
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
            
            other.gameObject.SetActive(false);
            _hiddenGameObjects.Add(other.transform);
            
            gameManager.CurrentBaseLocalManager.OnActivityComplete(new MachineLabActivity(EMachineActivity.OnEnter, EMachine.PaperTrayMachine));
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