using BioEngineerLab.Tasks;
using Core;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TabletUI.Panels
{
    public class HintTabletPanel : BaseTabletPanel
    {
        [SerializeField] private Image _taskHintImage;
        
        [CanBeNull] private LabTask _showingTask;

        private void Update()
        {
            if (_showingTask == null)
            {
                return;
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