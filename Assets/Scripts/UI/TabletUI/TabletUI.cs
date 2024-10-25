using BioEngineerLab.Core;
using BioEngineerLab.Tasks;
using UnityEngine;

namespace BioEngineerLab.UI
{
    [RequireComponent(typeof(TabletPanelSwitcher))]
    public class TabletUI : MonoBehaviour
    {
        private TabletPanelSwitcher _panelSwitcher;
        private TasksService _tasksService;

        private void Awake()
        {
            _panelSwitcher = GetComponent<TabletPanelSwitcher>();
            
            _tasksService = Engine.GetService<TasksService>();
            _tasksService.TaskFailedEvent += OnTaskFailed;
            _tasksService.TaskUpdatedEvent += OnTaskUpdated;
            _tasksService.EndTasksListEvent += OnEndTasksList;
        }

        private void OnDestroy()
        {
            _tasksService.TaskFailedEvent -= OnTaskFailed;
            _tasksService.TaskUpdatedEvent -= OnTaskUpdated;
            _tasksService.EndTasksListEvent -= OnEndTasksList;
        }

        private void OnTaskFailed()
        {
            _panelSwitcher.SwitchPanel(TabletPanelsType.TaskFailedPanel);
        }

        private void OnEndTasksList()
        {
            _panelSwitcher.SwitchPanel(TabletPanelsType.EndGamePanel);
        }

        private void OnTaskUpdated(LabTask task)
        {

        }
    }
}