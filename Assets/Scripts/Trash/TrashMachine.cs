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
using Database;

namespace Trash
{
    [RequireComponent(typeof(Collider))]
    public class TrashMachine : MonoBehaviour, ISaveableOther
    {
        private class SavedData
        {
            public List<TrashableObject> HiddenGameObjects = new List<TrashableObject>();
        }

        [SerializeField] private ETrashType _trashType;
        [SerializeField] private ButtonComponent _button;
        [CanBeNull][SerializeField] private ParticleSystem _particleSystem;
        
        [CanBeNull] private HandsChanger _handsChanger;
        
        private SavedData _savedData = new SavedData();
        private List<TrashableObject> _hiddenGameObjects = new List<TrashableObject>();
        
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
                if (_handsChanger == null)
                {
                    return;
                }
                
                _handsChanger.TakeGlovesOff();
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
            
            TrashableObject trashableObject = other.GetComponentInParent<TrashableObject>();

            if (trashableObject == null)
            {
                return;
            }
            
            trashableObject.SetTreshableActive(false);
            _hiddenGameObjects.Add(trashableObject);

            if (_particleSystem != null)
            {
                _particleSystem.Play();
            }

            if (gameManager.IsSoundsOn)
            {
                switch (_trashType)
                {
                    case ETrashType.Sink:
                    {
                        AudioClip a = ResourcesDatabase.ReadSound("WashingMachine");
                        AudioSource.PlayClipAtPoint(a, transform.position);
                        break;
                    }
                    case ETrashType.PenSink:
                    {
                        AudioClip a = ResourcesDatabase.ReadSound("WashingMachine");
                        AudioSource.PlayClipAtPoint(a, transform.position);
                        break;
                    }
                    case ETrashType.PaperTray:
                    {
                        AudioClip a = ResourcesDatabase.ReadSound("PaperTray");
                        AudioSource.PlayClipAtPoint(a, transform.position);
                        break;
                    }
                    default:
                    {
                        AudioClip a = ResourcesDatabase.ReadSound("TrashMachine");
                        AudioSource.PlayClipAtPoint(a, transform.position);
                        break;
                    }
                }
            }
            
            gameManager.CurrentBaseLocalManager.OnActivityComplete(new TrashLabActivity(_trashType, trashableObject.TrashableObjectType));
        }

        public void Save()
        {
            _savedData.HiddenGameObjects.Clear();
            
            foreach (var trashableObject in _hiddenGameObjects)
            {
                _savedData.HiddenGameObjects.Add(trashableObject);
            }
        }

        public void Load()
        {
            foreach (var trashableObject in _hiddenGameObjects)
            {
                trashableObject.SetTreshableActive(true);
            }

            foreach (var trashableObject in _savedData.HiddenGameObjects)
            {
                trashableObject.SetTreshableActive(false);
            }
            
            _hiddenGameObjects.Clear();

            foreach (var interactable in _savedData.HiddenGameObjects)
            {
                _hiddenGameObjects.Add(interactable);
            }
        }
    }
}