using UnityEngine;
using UnityEngine.InputSystem;

namespace Gameplay
{
    public class HandAnimatorController : MonoBehaviour
    {
        [SerializeField] private InputActionProperty _triggerAction;
        [SerializeField] private InputActionProperty _gripAction;

        private Animator _anim;

        private void Start()
        {
            _anim = GetComponent<Animator>();
        }

        private void Update()
        {
            float triggerValue = _triggerAction.action.ReadValue<float>();
            float gripValue = _gripAction.action.ReadValue<float>();

            _anim.SetFloat("Trigger", triggerValue);
            _anim.SetFloat("Grip", gripValue);
        }
    }
}