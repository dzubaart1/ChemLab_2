using System;
using BioEngineerLab.Tasks;
using Core;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TabletUI.Panels
{
    public class MainTabletPanel : BaseTabletPanel
    {
        [Header("UIs")]
        [SerializeField] private Button _homeButton;
        [SerializeField] private Button _hintButton;
        [SerializeField] private Button _infoButton;
        [SerializeField] private Button _safetyButton;
        [SerializeField] private Button _musicButton;
        [SerializeField] private Button _soundButton;
        [SerializeField] private TextMeshProUGUI _taskTitleText;
        [SerializeField] private TextMeshProUGUI _taskDescriptionText;

        [Header("Sprites")]
        [SerializeField] private Sprite _musicOn;
        [SerializeField] private Sprite _musicOff;
        [SerializeField] private Sprite _soundOn;
        [SerializeField] private Sprite _soundOff;
        
        [Header("AudioSourse")]
        [SerializeField] private AudioSource _music;
        
        [CanBeNull] private LabTask _showingTask;

        private void OnEnable()
        {
            _homeButton.onClick.AddListener(OnHomeButtonClick);
            _hintButton.onClick.AddListener(OnHintButtonClick);
            _infoButton.onClick.AddListener(OnInfoButtonClick);
            _safetyButton.onClick.AddListener(OnSafetyButtonClick);
            _musicButton.onClick.AddListener(OnMusicButtonClick);
            _soundButton.onClick.AddListener(OnSoundButtonClick);
        }

        private void OnDisable()
        {
            _showingTask = null;
            
            _homeButton.onClick.RemoveListener(OnHomeButtonClick);
            _hintButton.onClick.RemoveListener(OnHintButtonClick);
            _infoButton.onClick.RemoveListener(OnInfoButtonClick);
            _safetyButton.onClick.RemoveListener(OnSafetyButtonClick);
            _musicButton.onClick.RemoveListener(OnMusicButtonClick);
            _soundButton.onClick.RemoveListener(OnSoundButtonClick);
        }

        private void OnHomeButtonClick()
        {
            TabletUI.SwitchToLoadLobbyPanel();
        }
        
        private void OnHintButtonClick()
        {
            TabletUI.SwitchToHintPanel();
        }

        private void OnInfoButtonClick()
        {
            TabletUI.SwitchToInfoPanel();
        }
        
        private void OnSafetyButtonClick()
        {
            TabletUI.SwitchToSafetyPanel();
        }

        private void OnMusicButtonClick()
        {
            GameManager gameManager = GameManager.Instance;
            if (gameManager == null)
            {
                return;
            }
            
            gameManager.IsMusicOn = !gameManager.IsMusicOn;
            Image musicImage = _musicButton.targetGraphic as Image;
            musicImage.sprite = gameManager.IsMusicOn ? _musicOn : _musicOff;
            if (gameManager.IsMusicOn)
            {
                _music.Play();
            }
            else
            {
                _music.Pause();
            }
        }

        private void OnSoundButtonClick()
        {
            GameManager gameManager = GameManager.Instance;
            if (gameManager == null)
            {
                return;
            }
            
            gameManager.IsSoundsOn = !gameManager.IsSoundsOn;
            Image soundImage = _soundButton.targetGraphic as Image;
            soundImage.sprite = gameManager.IsSoundsOn ? _soundOn : _soundOff;
        }

        public override void SetTaskToShow(LabTask task)
        {
            _showingTask = task;
            
            GameManager gameManager = GameManager.Instance;
            if (gameManager == null)
            {
                return;
            }

            switch (gameManager.CurrentLanguage)
            {
                case ELabLanguage.English:
                    _taskTitleText.text = _showingTask.TitleEnglish;
                    break;
                case ELabLanguage.Russia:
                    _taskTitleText.text = _showingTask.Title;
                    break;
                default:
                    Debug.LogError("Can't find language!");
                    break;
            }
            
            switch (gameManager.CurrentLanguage)
            {
                case ELabLanguage.English:
                    _taskDescriptionText.text = _showingTask.DescriptionEnglish;
                    break;
                case ELabLanguage.Russia:
                    _taskDescriptionText.text = _showingTask.Description;
                    break;
                default:
                    Debug.LogError("Can't find language!");
                    break;
            }
            
            if (String.IsNullOrEmpty(_showingTask.HintImagePath))
            {
                _hintButton.interactable = false;
            }
            else
            {
                _hintButton.interactable = true;
            }

            if (String.IsNullOrEmpty(_showingTask.Warning))
            {
                _infoButton.interactable = false;
            }
            else
            {
                _infoButton.interactable = true;
            }

            if (String.IsNullOrEmpty(_showingTask.SafetyPrecautions))
            {
                _safetyButton.interactable = false;
            }
            else
            {
                _safetyButton.interactable = true;
            }
        }

        public override void SetLabToShow(ELab lab)
        {
        }
    }
}