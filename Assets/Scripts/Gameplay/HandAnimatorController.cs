using UnityEngine;
using UnityEngine.InputSystem;
using Core;
using BioEngineerLab.Tasks.SideEffects;

namespace Gameplay
{
    public class HandAnimatorController : MonoBehaviour, ISideEffectActivator
    {
        [SerializeField] private InputActionProperty _triggerAction;
        [SerializeField] private InputActionProperty _gripAction;

        private Animator _anim;

        public bool IsGrabbableAnimationActive = true;

        private void Start()
        {
            _anim = GetComponent<Animator>();
            
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
        }
    }
}