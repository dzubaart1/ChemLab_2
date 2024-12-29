using BioEngineerLab.Tasks;
using Core;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace UI.TabletUI.Panels
{
    public class MainTabletPanel : BaseTabletPanel
    {
        [SerializeField] private TextMeshProUGUI _taskTitleText;
        [SerializeField] private TextMeshProUGUI _taskDescriptionText;
        
        [CanBeNull] private LabTask _showingTask;

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