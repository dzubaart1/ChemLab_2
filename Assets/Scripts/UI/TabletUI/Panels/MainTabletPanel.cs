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
        [SerializeField] private Button _hintButton;
        [SerializeField] private Button _infoButton;
        [SerializeField] private TextMeshProUGUI _taskTitleText;
        [SerializeField] private TextMeshProUGUI _taskDescriptionText;
        
        [CanBeNull] private LabTask _showingTask;

        private void OnEnable()
        {
            _hintButton.onClick.AddListener(OnHintButtonClick);
            _infoButton.onClick.AddListener(OnInfoButtonClick);
        }

        private void OnDisable()
        {
            _hintButton.onClick.RemoveListener(OnHintButtonClick);
            _infoButton.onClick.RemoveListener(OnInfoButtonClick);
        }

        private void OnHintButtonClick()
        {
            TabletUI.SwitchToHintPanel();
        }

        private void OnInfoButtonClick()
        {
            TabletUI.SwitchToInfoPanel();
        }

        private void Update()
        {
            if (_showingTask == null)
            {
                return;
            }

            _taskTitleText.text = _showingTask.Title;
            _taskDescriptionText.text = _showingTask.Description;
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