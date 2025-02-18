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
        
        /*public void Init()
        {
            GameManager gameManager = GameManager.Instance;
            if (gameManager == null)
            {
                return;
            }

            if (gameManager.CurrentBaseLocalManager == null)
            {
                return;
            }
            
            gameManager.CurrentBaseLocalManager.AddSideEffectActivator(this);
        }*/

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

        private void Update()
        {
            if (_showingTask == null)
            {
                return;
            }

            _taskHintImage.sprite = ResourcesDatabase.ReadHintImage(_showingTask.HintImagePath);
        }

        public void NewTask()
        {
            _taskHintImage.sprite = null;
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