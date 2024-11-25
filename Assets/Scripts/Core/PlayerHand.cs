using Configurations;
using Core.Services;
using UnityEngine;

namespace Core
{
    public class PlayerHand : MonoBehaviour, ISaveable
    {
        [SerializeField] private PlayerHandType _handType;
        [SerializeField] private SkinnedMeshRenderer _skinnedMeshRenderer;
    
        private struct SavedData
        {
            public Material HandMaterial;
        }
    
        public enum PlayerHandType
        {
            Left,
            Right
        }
    
        public PlayerHandType HandType { get; private set; }

        private HandModelsService _modelsService;
        private SaveService _saveService;

        private SavedData _savedData = new SavedData();
    
        private void Awake()
        {
            _modelsService = Engine.GetService<HandModelsService>();
            _saveService = Engine.GetService<SaveService>();
            _saveService.LoadSceneStateEvent += OnLoadScene;
            _saveService.SaveSceneStateEvent += OnSaveScene;

            HandType = _handType;
        }

        private void Start()
        {
            OnSaveScene();
        }

        public void ToggleHandModel(HandModelConfiguration.HandModelType handModelType)
        {
            Material targetMaterial =
                _modelsService.Configuration.HandModelSettingsList.Find(model => model.HandModelType == handModelType).HandModelMaterial;

            if (targetMaterial == null)
            {
                return;
            }

            _skinnedMeshRenderer.material = targetMaterial;
        }

        public void OnSaveScene()
        {
            _savedData.HandMaterial = _skinnedMeshRenderer.material;
        }

        public void OnLoadScene()
        {
            _skinnedMeshRenderer.material = _savedData.HandMaterial;
        }
    }
}
