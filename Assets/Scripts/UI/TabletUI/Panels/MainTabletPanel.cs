using System;
using BioEngineerLab.Tasks;
using Core;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace UI.TabletUI.Panels
{
    public class MainTabletPanel : BaseTabletPanel
    {
        [Header("UIs")]
        [SerializeField] private Button _hintButton;
        [SerializeField] private Button _infoButton;
        [SerializeField] private Button _safetyButton;
        [SerializeField] private TextMeshProUGUI _taskTitleText;
        [SerializeField] private TextMeshProUGUI _taskDescriptionText;
        
        [CanBeNull] private LabTask _showingTask;

        private void OnEnable()
        {
            _hintButton.onClick.AddListener(OnHintButtonClick);
            _infoButton.onClick.AddListener(OnInfoButtonClick);
            _safetyButton.onClick.AddListener(OnSafetyButtonClick);
        }

        private void OnDisable()
        {
            _hintButton.onClick.RemoveListener(OnHintButtonClick);
            _infoButton.onClick.RemoveListener(OnInfoButtonClick);
            _safetyButton.onClick.RemoveListener(OnSafetyButtonClick);
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

        private void Update()
        {
            GameManager gameManager = GameManager.Instance;
            if (gameManager == null)
            {
                return;
            }
            
            if (_showingTask == null)
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

        public override void SetTaskToShow(LabTask task)
        {
            _showingTask = task;
        }

        public override void SetLabToShow(ELab lab)
        {
        }
    }
}