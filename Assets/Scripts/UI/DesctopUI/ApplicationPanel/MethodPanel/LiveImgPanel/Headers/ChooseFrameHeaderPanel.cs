using System;
using BioEngineerLab.Core;
using BioEngineerLab.Gameplay;
using BioEngineerLab.UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace BioEngineerLab.UI.Headers
{
    public class ChooseFrameHeaderPanel : BasePanel<LiveImgHeaderPanelsType>, ISaveable
    {
        private struct SavedData
        {
            public Sprite Sprite;
            public bool WarningPanelActive;
        }
        
        [SerializeField] private GameObject _warningPanel;
        [SerializeField] private Button _backFrameButton;
        [SerializeField] private Button _forwardFrameButton;
        [SerializeField] private ButtonComponent _finishMeasurementButton;
        
        [SerializeField] private Image _headerBackground;
        [SerializeField] private Sprite _defaultBackground;
        private DropAnimationService _dropAnimationService;
        private SaveService _saveService;

        private SavedData _savedData;

        private void Awake()
        {
            _savedData = new SavedData();
            
            _saveService = Engine.GetService<SaveService>();
            _saveService.SaveSceneStateEvent += OnSaveScene;
            _saveService.LoadSceneStateEvent += OnLoadScene;
            
            _dropAnimationService = Engine.GetService<DropAnimationService>();
            _dropAnimationService.ShowCurrentSpriteAnimationEvent += OnShowCurrentSpriteAnimation;
            
            _finishMeasurementButton.OnClickButton += OnClickFinishMeasurementBtn;
            _backFrameButton.onClick.AddListener(OnClickBackFrameBtn);
            _forwardFrameButton.onClick.AddListener(OnClickForwardFrameBtn);
        }

        private void Start()
        {
            OnSaveScene();
        }

        private void OnDestroy()
        {
            _dropAnimationService.ShowCurrentSpriteAnimationEvent -= OnShowCurrentSpriteAnimation;
            _finishMeasurementButton.OnClickButton -= OnClickFinishMeasurementBtn;
            _backFrameButton.onClick.RemoveListener(OnClickBackFrameBtn);
            _forwardFrameButton.onClick.RemoveListener(OnClickForwardFrameBtn);
        }

        public void OnClickBackFrameBtn()
        {
            _dropAnimationService.BackFrame();
        }

        public void OnClickForwardFrameBtn()
        {
            _dropAnimationService.ForwardFrame();
        }
        
        private void OnClickFinishMeasurementBtn()
        {
            _warningPanel.SetActive(true);
            SwitchPanel(LiveImgHeaderPanelsType.MainHeaderPanel);
            _headerBackground.sprite = _defaultBackground;
        }
        
        private void OnShowCurrentSpriteAnimation(int spriteIndex)
        {
            if (spriteIndex >= 0 &&
                spriteIndex < _dropAnimationService.CurrentDropAnimationSubstance.HeaderSprites.Count)
            {
                _headerBackground.sprite = _dropAnimationService.CurrentDropAnimationSubstance.HeaderSprites[spriteIndex];
            }
        }

        public void OnSaveScene()
        {
            _savedData.Sprite = _headerBackground.sprite;
            _savedData.WarningPanelActive = _warningPanel.activeSelf;
        }

        public void OnLoadScene()
        {
            _headerBackground.sprite = _savedData.Sprite;
            _warningPanel.gameObject.SetActive(_savedData.WarningPanelActive);
        }
    }
}