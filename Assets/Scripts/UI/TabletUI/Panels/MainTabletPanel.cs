using BioEngineerLab.Tasks;
using Core;
using Core.Services;
using TMPro;
using UnityEngine;

namespace UI.TabletUI.Panels
{
    public class MainTabletPanel : BasePanel<TabletPanelsType>
    {
        [SerializeField] private TextMeshProUGUI _taskTitleText, _taskDescriptionText;
        
        private TasksService _tasksService;
        
        private void Awake()
        {
            _tasksService = Engine.GetService<TasksService>();
            _tasksService.TaskUpdatedEvent += OnTaskUpdate;
        }

        private void Start()
        {
            if (_tasksService.CurrentTask != null)
            {
                OnTaskUpdate(_tasksService.CurrentTask);
            }
        }

        private void OnDestroy()
        {
            _tasksService.TaskUpdatedEvent -= OnTaskUpdate;
        }
        
        private void OnTaskUpdate(LabTask task)
        {
            _taskTitleText.text = task.Title;
            _taskDescriptionText.text = task.Description;
        }
    }
}