using System.Collections.Generic;
using BioEngineerLab.Activities;
using Containers;
using Core;
using UnityEngine;
using Mechanics;
using Saveables;

namespace BioEngineerLab.Machines
{
    [RequireComponent(typeof(Collider))]
    public class TeleportMachine : MonoBehaviour, ISaveableOther
    {
        private class SavedData
        {
            public List<Transform> HiddenGameObjects = new List<Transform>();
        }
        
        [SerializeField] private VRSocketInteractor _socketInteractor;
        
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

        private void OnEnable()
        {
            _socketInteractor.EnteredTransformEvent += OnEnterTransform;
        }

        private void OnDisable()
        {
            _socketInteractor.EnteredTransformEvent -= OnEnterTransform;
        }

        private void OnEnterTransform(Transform enterTransform)
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
            
            LabContainer[] labContainers = enterTransform.GetComponentsInChildren<LabContainer>();

            if (labContainers.Length == 0)
            {
                return;
            }

            enterTransform.gameObject.SetActive(false);
            _hiddenGameObjects.Add(enterTransform);

            gameManager.CurrentBaseLocalManager.OnActivityComplete(
                new SocketSubstancesLabActivity(_socketInteractor.SocketType, ESocketActivity.Enter, labContainers[0].GetSubstanceProperties()));
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