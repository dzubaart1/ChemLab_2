using System;
using Gameplay;
using Machines;
using UI.TabletUI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;
using Database;

namespace Core
{
    public abstract class Player : MonoBehaviour
    {
        public event Action LeftHandGrabbedEvent;
        public event Action RightHandGrabbedEvent;

        [SerializeField] protected HandsChanger _handsChanger;
        public HandsChanger HandsChanger => _handsChanger;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        protected void CallRightHandGrabbedEvent()
        {
            RightHandGrabbedEvent?.Invoke();
        }

        protected void CallLeftHandGrabbedEvent()
        {
            LeftHandGrabbedEvent?.Invoke();
        }

        public abstract void ReleaseAllGrabbables();

        public abstract void ReleaseHandle();

        public abstract void Init();
    }
}