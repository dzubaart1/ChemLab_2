using BioEngineerLab.Core;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
namespace BioEngineerLab.Gameplay
{
    public class VRGrabInteractable : XRGrabInteractable, ISaveable
    {
        private struct SavedData
        {
            public Vector3 Position;
            public Quaternion Rotation;
        }

        private SaveService _saveService;

        private SavedData _savedData;

        protected override void Awake()
        {
            base.Awake();

            _saveService = Engine.GetService<SaveService>();
            _saveService.SaveSceneStateEvent += OnSaveScene;
            _saveService.LoadSceneStateEvent += OnLoadScene;

            _savedData = new SavedData();
        }

        private void Start()
        {
            OnSaveScene();
        }

        protected override void OnDestroy()
        {
            _saveService.SaveSceneStateEvent -= OnSaveScene;
            _saveService.LoadSceneStateEvent -= OnLoadScene;
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
                Debug.Log($"INTERACTABLE {gameObject.name}; Skip loadScene load position; ");
                return;
            }

            transform.position = _savedData.Position;
            transform.rotation = _savedData.Rotation;
        }

        public void LoadPosition()
        {
            Debug.Log($"INTERACTABLE {gameObject.name}; Load position and rotation");
            transform.position = _savedData.Position;
            transform.rotation = _savedData.Rotation;
        }
    }
}