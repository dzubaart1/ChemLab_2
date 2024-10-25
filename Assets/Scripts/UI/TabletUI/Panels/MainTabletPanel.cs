using BioEngineerLab.Core;
using BioEngineerLab.Tasks;
using TMPro;
using UnityEngine;

namespace BioEngineerLab.UI
{
    public class MainTabletPanel : BasePanel<TabletPanelsType>
    {
        [SerializeField] private TextMeshProUGUI _taskTitleText, _taskDescriptionText;
        
        private TasksService _tasksService;
        
        /*private void Awake()
        {
            _tasksService = Engine.GetService<TasksService>();
            //_tasksService.TaskUpdatedEvent += OnTaskUpdate;

            //OnTaskUpdate(_tasksService.GetCurrentTask());
        }

        private void OnDestroy()
        {
            _tasksService.TaskUpdatedEvent -= OnTaskUpdate;
        }
        
        private void OnTaskUpdate(Tas task)
        {
            _taskTitleText.text = task.Title;
            _taskDescriptionText.text = task.Description;
        }*/
    }
}