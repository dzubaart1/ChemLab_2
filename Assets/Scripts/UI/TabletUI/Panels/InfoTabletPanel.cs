using System;
using BioEngineerLab.Activities;
using BioEngineerLab.Core;
using BioEngineerLab.Tasks;
using BioEngineerLab.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoTabletPanel : BasePanel<TabletPanelsType>
{
    [SerializeField] private TextMeshProUGUI _infoText;

    [SerializeField] private Button _mainPanelBtn;

    private TasksService _tasksService;

    private void OnEnable()
    {
        if (string.IsNullOrWhiteSpace(_tasksService.GetCurrentTask().Warning))
        {
            _infoText.text = "";
            return;
        }
        _infoText.text = _tasksService.GetCurrentTask().Warning;
    }

    private void Awake()
    {
        _tasksService = Engine.GetService<TasksService>();

        _mainPanelBtn.onClick.AddListener(OnClickMainPanelBtn);
    }

    private void OnDestroy()
    {
        _mainPanelBtn.onClick.RemoveListener(OnClickMainPanelBtn);
    }

    private void OnClickMainPanelBtn()
    {
        TaskProperty currentTask = _tasksService.GetCurrentTask();
        switch (currentTask.ActivityConfig.ActivityType)
        {
            //case ActivityType.SliderValueChangedActivity:
            //    SwitchPanel(TabletPanelsType.SliderTaskPanel);
            //    break;
            //case ActivityType.DragLineActivity:
            //    SwitchPanel(TabletPanelsType.DragLinePanel);
            //    break;
            //default:
            //    SwitchPanel(TabletPanelsType.MainPanel);
            //    break;
        }
    }
}
