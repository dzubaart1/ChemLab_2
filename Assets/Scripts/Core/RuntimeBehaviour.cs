using System;
using UnityEngine;

namespace Core
{
    public class RuntimeBehaviour : MonoBehaviour
    {
        public event Action BehaviourUpdateEvent;
        public event Action BehaviourLateUpdateEvent;
        public event Action BehaviourDestroyEvent;
        public event Action BehaviourStartEvent;

        private void Start()
        {
            BehaviourStartEvent?.Invoke();
        }

        private void Update()
        {
            BehaviourUpdateEvent?.Invoke();
        }

        private void LateUpdate()
        {
            BehaviourLateUpdateEvent?.Invoke();
        }

        private void OnDestroy()
        {
            BehaviourDestroyEvent?.Invoke();
        }
    }
}
