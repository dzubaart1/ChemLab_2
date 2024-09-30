using System;
using System.Collections;
using BioEngineerLab.Configurations;
using BioEngineerLab.Core;
using BioEngineerLab.Gameplay;
using BioEngineerLab.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace BioEngineerLab.UI
{
    public class LiveImage : MonoBehaviour, ISaveable
    {
        public Image Image { get; private set; }
        
        private struct SavedData
        {
            public Sprite CurrentSprite;
            public float ColorA;
        }
        
        [SerializeField] private Image _image;
        
        private TasksService _tasksService;
        private DropAnimationService _dropAnimationService;
        private SaveService _saveService;
        
        private bool _isAnimating;

        private SavedData _savedData;

        private void Awake()
        {
            _saveService = Engine.GetService<SaveService>();
            _saveService.LoadSceneStateEvent += OnLoadScene;
            _saveService.SaveSceneStateEvent += OnSaveScene;
            
            _dropAnimationService = Engine.GetService<DropAnimationService>();
            _dropAnimationService.ShowCurrentSpriteAnimationEvent += OnShowCurrentSpriteAnimation;
            _dropAnimationService.StartAnimationEvent += OnAnimationStart;
            
            _tasksService = Engine.GetService<TasksService>();
            _tasksService.TaskUpdatedEvent += OnTaskUpdate;

            _savedData = new SavedData();

            Image = _image;
        }

        private void Start()
        {
            OnSaveScene();
        }

        private void OnDestroy()
        {
            _saveService.LoadSceneStateEvent -= OnLoadScene;
            _saveService.SaveSceneStateEvent -= OnSaveScene;
            _dropAnimationService.ShowCurrentSpriteAnimationEvent -= OnShowCurrentSpriteAnimation;
            _dropAnimationService.StartAnimationEvent -= OnAnimationStart;
            _tasksService.TaskUpdatedEvent -= OnTaskUpdate;
        }

        public void ChangeSprite(Sprite sprite)
        {
            _image.sprite = sprite;
        }

        public void ChangeContrast(float contrast)
        {
            var tempColor = new Color(contrast, contrast, contrast, 1);
            _image.color = tempColor;
        }
        
        private void OnTaskUpdate(TaskProperty taskProperty)
        {
            if (taskProperty.IsTaskChangeSprite)
            {
                _image.sprite = taskProperty.Sprite;
            }
        }
        
        private void OnShowCurrentSpriteAnimation(int spriteIndex)
        {
            _image.sprite = _dropAnimationService.CurrentDropAnimationSubstance.SubstanceSprites[spriteIndex];
        }
        
        private void OnAnimationStart(DropAnimationConfiguration.DropAnimationSubstance dropAnimationSubstance)
        {
            StartCoroutine(PlaySpriteAnimation(dropAnimationSubstance));
        }

        private IEnumerator PlaySpriteAnimation(DropAnimationConfiguration.DropAnimationSubstance dropAnimationSubstance)
        {
            _isAnimating = true;
            foreach (var sprite in dropAnimationSubstance.SubstanceSprites)
            {
                _image.sprite = sprite;
                yield return new WaitForSeconds(dropAnimationSubstance.AnimationDuration);
                
                if (!_isAnimating)
                {
                    _image.sprite = _savedData.CurrentSprite;
                    yield break;
                }
            }

            _isAnimating = false;
        }

        public void OnSaveScene()
        {
            _savedData.CurrentSprite = _image.sprite;
            _savedData.ColorA = _image.color.r;
        }

        public void OnLoadScene()
        {
            if (_isAnimating)
            {
                _isAnimating = false;
            }
            else
            {
                _image.sprite = _savedData.CurrentSprite;
            }

            var tempColor = new Color(_savedData.ColorA, _savedData.ColorA, _savedData.ColorA, 1);
            _image.color = tempColor;
        }
    }
}