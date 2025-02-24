using System;
using System.IO;
using BioEngineerLab.Tasks;
using Core;
using BioEngineerLab.Tasks.SideEffects;
using Database;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TabletUI.Panels
{
    public class HintTabletPanel : BaseTabletPanel
    {
        [SerializeField] private Image _taskHintImage;
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

        private void OnReturnButtonClicked()
        {
            TabletUI.SwitchToMainPanel();
        }

        public void NewTask()
        {
            _taskHintImage.sprite = null;
        }

        public override void SetTaskToShow(LabTask task)
        {
            if (task == null)
            {
                return;
            }

            _showingTask = task;
            _taskHintImage.sprite = ResourcesDatabase.ReadHintImage(_showingTask.HintImagePath);
        }

        public override void SetLabToShow(ELab lab)
        {
        }
    }
}