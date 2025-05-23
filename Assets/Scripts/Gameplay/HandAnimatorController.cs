using UnityEngine;
using UnityEngine.InputSystem;
using Core;
using Saveables;
using BioEngineerLab.Tasks.SideEffects;

namespace Gameplay
{
    public class HandAnimatorController : MonoBehaviour, ISideEffectActivator, ISaveableOther
    {
        private class SavedData
        {
            public bool IsActive = true;
        }
        
        [SerializeField] private InputActionProperty _triggerAction;
        [SerializeField] private InputActionProperty _gripAction;

        private Animator _anim;
        private SavedData _savedData = new SavedData();

        public bool IsGrabbableAnimationActive = true;

        private void Start()
        {
            _anim = GetComponent<Animator>();
        }

        public void Init()
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
            gameManager.CurrentBaseLocalManager.AddSideEffectActivator(this);
            gameManager.CurrentBaseLocalManager.AddSaveableOther(this);
            IsGrabbableAnimationActive = true;
        }

        private void Update()
        {
            float triggerValue = _triggerAction.action.ReadValue<float>();
            float gripValue = _gripAction.action.ReadValue<float>();

            if (IsGrabbableAnimationActive)
            {
                _anim.SetFloat("Grip", gripValue);
            } 
            _anim.SetFloat("Trigger", triggerValue);
        }
        
        public void OnActivateSideEffect(LabSideEffect sideEffect)
        {
            if (sideEffect is not TriggerActivatorSideEffect triggerActivatorSideEffect)
            {
                return;
            }

            if (triggerActivatorSideEffect.TriggerType != ETriggerType.LeftHandTrigger)
            {
                return;
            }

            IsGrabbableAnimationActive = !triggerActivatorSideEffect.IsActive;
            _anim.SetFloat("Grip", 0);
        }
        
        public void Save()
        {
            _savedData.IsActive = IsGrabbableAnimationActive;
        }

        public void Load()
        {
            IsGrabbableAnimationActive = _savedData.IsActive;
        }
    }
}