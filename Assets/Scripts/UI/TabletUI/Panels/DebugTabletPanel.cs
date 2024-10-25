using BioEngineerLab.Core;
using UnityEngine;
using UnityEngine.UI;

namespace BioEngineerLab.UI
{
    public class DebugTabletPanel : BasePanel<TabletPanelsType>
    {
        [SerializeField] private Button _prevBtn, _nextBtn;

        private TasksService _tasksService;
        
        private void Awake()
        {
            _prevBtn.onClick.AddListener(OnClickPrevBtn);
            _nextBtn.onClick.AddListener(OnClickNextBtn);

            _tasksService = Engine.GetService<TasksService>();
        }

        private void OnDestroy()
        {
            _prevBtn.onClick.RemoveListener(OnClickPrevBtn);
            _nextBtn.onClick.RemoveListener(OnClickNextBtn);
        }
        
        private void OnClickPrevBtn()
        {
            _tasksService.MoveToPrevTask();
        }

        private void OnClickNextBtn()
        {
            _tasksService.MoveToNextTask();
        }
    }
}