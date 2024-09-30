using System;
using System.Threading.Tasks;
using BioEngineerLab.Configurations;
using BioEngineerLab.Gameplay;
using BioEngineerLab.Substances;

namespace BioEngineerLab.Core
{
    public class DropAnimationService : IService, ISaveable
    {
        private struct SavedData
        {
            public DropAnimationConfiguration.DropAnimationSubstance CurrentDropAnimationSubstance;
            public int SpritesIndex;
        }
        
        public event Action<DropAnimationConfiguration.DropAnimationSubstance> StartAnimationEvent;
        public event Action<int> ShowCurrentSpriteAnimationEvent;

        public DropAnimationConfiguration Configuration { get; private set; }
        public DropAnimationConfiguration.DropAnimationSubstance CurrentDropAnimationSubstance { get; private set; }

        private int _spritesIndex;
        private SavedData _savedData;

        private SaveService _saveService;
        
        public DropAnimationService(DropAnimationConfiguration configuration, SaveService saveService)
        {
            Configuration = configuration;
            _saveService = saveService;

            _saveService.SaveSceneStateEvent += OnSaveScene;
            _saveService.LoadSceneStateEvent += OnLoadScene;

            _savedData = new SavedData();
        }

        public Task Initialize()
        {
            OnSaveScene();
            return Task.CompletedTask;
        }

        public void Destroy()
        {
        }
        
        public void PlayAnimation(SubstanceName substanceName)
        {
            _spritesIndex = 0;
            CurrentDropAnimationSubstance = FindDropAnimation(substanceName);
            StartAnimationEvent?.Invoke(CurrentDropAnimationSubstance);
        }

        public void ShowFirstFrame()
        {
            ShowCurrentSpriteAnimationEvent?.Invoke(_spritesIndex);
        }

        public void BackFrame()
        {
            if (_spritesIndex - 1 < 0)
            {
                return;
            }
            
            ShowCurrentSpriteAnimationEvent?.Invoke(--_spritesIndex);
        }

        public void ForwardFrame()
        {
            if (_spritesIndex + 1 >= CurrentDropAnimationSubstance.SubstanceSprites.Count)
            {
                return;
            }
            
            ShowCurrentSpriteAnimationEvent?.Invoke(++_spritesIndex);
        }

        private DropAnimationConfiguration.DropAnimationSubstance FindDropAnimation(SubstanceName substanceName)
        {
            foreach (var animationSubstance in Configuration.DropAnimationSubstances)
            {
                if (animationSubstance.SubstanceName == substanceName)
                {
                    return animationSubstance;
                }
            }

            throw new ArgumentException("There is not such DropAnimationSubstance!");
        }

        public void OnSaveScene()
        {
            _savedData.CurrentDropAnimationSubstance = CurrentDropAnimationSubstance;
            _savedData.SpritesIndex = _spritesIndex;
        }

        public void OnLoadScene()
        {
            _spritesIndex = _savedData.SpritesIndex;
            CurrentDropAnimationSubstance = _savedData.CurrentDropAnimationSubstance;
        }
    }
}