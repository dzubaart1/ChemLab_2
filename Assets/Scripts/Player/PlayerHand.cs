using JetBrains.Annotations;
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

        [CanBeNull] private GameManager _gameManager;

        private SavedData _savedData = new SavedData();
    
        private void Awake()
        {
            /*ServicesManager servicesManager = ServicesManager.Instance;
            if (servicesManager == null)
            {
                return;
            }
            
            ConfigurationsManager configurationsManager = ConfigurationsManager.Instance;
            
            if (configurationsManager == null)
            {
                return;
            }
            
            _handModelConfiguration = configurationsManager.HandModelConfiguration;
            _saveService = servicesManager.SaveService;

            HandType = _handType;*/
        }

        private void OnEnable()
        {
            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.Game.LoadGameEvent += OnLoadScene;
            _gameManager.Game.SaveGameEvent += OnSaveScene;
        }

        private void OnDisable()
        {
            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.Game.LoadGameEvent -= OnLoadScene;
            _gameManager.Game.SaveGameEvent -= OnSaveScene;
        }

        private void Start()
        {
            OnSaveScene();
        }

        /*public void ToggleHandModel(HandModelConfiguration.HandModelType handModelType)
        {
            Material targetMaterial =
                _handModelConfiguration.HandModelSettingsList.Find(model => model.HandModelType == handModelType).HandModelMaterial;

            if (targetMaterial == null)
            {
                return;
            }

            _skinnedMeshRenderer.material = targetMaterial;
        }*/

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
