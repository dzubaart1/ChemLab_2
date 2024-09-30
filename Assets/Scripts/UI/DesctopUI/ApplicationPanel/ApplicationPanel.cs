using System;
using BioEngineerLab.Core;
using BioEngineerLab.UI.Components;
using UnityEngine;

namespace BioEngineerLab.UI
{
    [RequireComponent(typeof(ApplicationPanelSwitcher))]
    public class ApplicationPanel : BasePanel<DesctopPanelsType>
    {
        
        [Space]
        [Header("Buttons")]
        [SerializeField] private ButtonComponent _closeAppButton;
        
        private ApplicationPanelSwitcher _panelSwitcher;

        private TasksService _tasksService;

        [SerializeField] private FinalPanel _finalPanel;
        
        private void Awake()
        {
            _tasksService = Engine.GetService<TasksService>();
            
            _tasksService.EndTasksListEvent += OnEndTaskListEvent;
            _panelSwitcher = GetComponent<ApplicationPanelSwitcher>();
            _closeAppButton.OnClickButton += OnClickCloseAppBtn;
        }

        private void OnDestroy()
        {
            _tasksService.EndTasksListEvent += OnEndTaskListEvent;
        }

        private void OnClickCloseAppBtn()
        {
            SwitchPanel(DesctopPanelsType.DesctopPanel);
        }

        private void OnEndTaskListEvent()
        {
            _finalPanel.OnEndTask();
            SwitchPanel(DesctopPanelsType.FinalPanel);
        }
    }
}
