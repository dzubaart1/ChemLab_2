using System;
using BioEngineerLab.Tasks;
using Core;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TabletUI.Panels
{
    public class InfoTabletPanel : BaseTabletPanel
    {
        [SerializeField] private TextMeshProUGUI _taskInfoText;
        [SerializeField] private Button _returnButton;
        
        [CanBeNull] private LabTask _showingTask;

        private void OnEnable()
        {
            _returnButton.onClick.AddListener(OnReturnButtonClicked);
        }

        private void OnDisable()
        {
            _returnButton.onClick.RemoveListener(OnReturnButtonClicked);
        }

        private void Update()
        {
            if (_showingTask == null)
            {
                return;
            }

            _taskInfoText.text = _showingTask.Warning;
        }

        private void OnReturnButtonClicked()
        {
            TabletUI.SwitchToMainPanel();
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