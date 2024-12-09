using Core;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Mechanics
{
    public class VRGrabInteractable : XRGrabInteractable, ISaveable
    {
        private struct SavedData
        {
            public Vector3 Position;
            public Quaternion Rotation;
        }

        [CanBeNull] private GameManager _gameManager;

        private SavedData _savedData = new SavedData();

        protected override void Awake()
        {
            base.Awake();

            _gameManager = GameManager.Instance;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.Game.SaveGameEvent += OnSaveScene;
            _gameManager.Game.LoadGameEvent += OnLoadScene;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.Game.SaveGameEvent -= OnSaveScene;
            _gameManager.Game.LoadGameEvent -= OnLoadScene;
        }

        private void Start()
        {
            OnSaveScene();
        }
        
        public void OnSaveScene()
        {
            _savedData.Position = transform.position;
            _savedData.Rotation = transform.rotation;
        }

        public void OnLoadScene()
        {
            if (firstInteractorSelecting is VRSocketInteractor)
            {
                return;
            }

            transform.position = _savedData.Position;
            transform.rotation = _savedData.Rotation;
        }

        public void LoadPosition()
        {
            transform.position = _savedData.Position;
            transform.rotation = _savedData.Rotation;
        }
    }
}