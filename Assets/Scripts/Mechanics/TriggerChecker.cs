using BioEngineerLab.Activities;
using Core;
using Saveables;
using UnityEngine;
using BioEngineerLab.Tasks.SideEffects;

namespace Machines
{
    [RequireComponent(typeof(Collider))]
    public class TriggerChecker : MonoBehaviour, ISaveableOther, ISideEffectActivator
    {
        private class SavedData
        {
            public bool IsActive;
        }
        
        [Header("References")]
        [SerializeField] private Collider _triggerCollider;
        
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
            gameManager.CurrentBaseLocalManager.AddSideEffectActivator(this);
        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log(other.transform.name);
            GameManager gameManager = GameManager.Instance;
            if (gameManager == null)
            {
                return;
            }

            if (gameManager.CurrentBaseLocalManager == null)
            {
                return;
            }

            if (!other.gameObject.activeSelf)
            {
                return;
            }

            if (!other.CompareTag("LHand") && !other.CompareTag("RHand"))
            {
                return;
            }
            
            gameManager.CurrentBaseLocalManager.OnActivityComplete(new BadLabActivity());
        }

        public void Save()
        {
            _savedData.IsActive = _triggerCollider.enabled;
        }

        public void Load()
        {
            _triggerCollider.enabled = _savedData.IsActive;
        }
        
        public void OnActivateSideEffect(LabSideEffect sideEffect)
        {
            if (sideEffect is not TriggerActivatorSideEffect triggerActivatorSideEffect)
            {
                return;
            }

            _triggerCollider.enabled = triggerActivatorSideEffect.IsActive;
        }
    }
}