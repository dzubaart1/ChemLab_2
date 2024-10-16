using System;
using System.Globalization;
using BioEngineerLab.Activities;
using BioEngineerLab.Core;
using BioEngineerLab.Tasks;
using BioEngineerLab.UI.Components;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BioEngineerLab.UI
{
    public class DragLinePanel : BasePanel<TabletPanelsType>
    {
        [SerializeField] private TextMeshProUGUI _taskTitleText, _taskDescriptionText, _valueText;
        [SerializeField] private Button _confirmValueBtn;

        private TasksService _tasksService;
        private UIComponentsService _uiComponentsService;

        private DragLine _currentDragLine;

        private void Awake()
        {
            _tasksService = Engine.GetService<TasksService>();
            _uiComponentsService = Engine.GetService<UIComponentsService>();

            _tasksService.TaskUpdatedEvent += OnUpdateTask;
            
            _confirmValueBtn.onClick.AddListener(OnConfirmValueBtnClick);
        }

        private void OnDestroy()
        {
            _confirmValueBtn.onClick.RemoveListener(OnConfirmValueBtnClick);
        }

        private void OnEnable()
        {
            OnUpdateTask(_tasksService.GetCurrentTask());
        }

        private void OnUpdateTask(TaskProperty taskProperty)
        {
            //if (taskProperty.ActivityConfig.ActivityType != EActivity.DragLineActivity)
            //{
            //    return;
            //}
            
            Debug.Log("Try Update");
            if (_currentDragLine != null)
            {
                _currentDragLine.MovedLineEvent -= OnCurrentDragLineMoved;
                _currentDragLine = null;
            }
            
            //DragLineActivity dragLineActivity = taskProperty.TaskActivity as DragLineActivity;
            //if (dragLineActivity == null)
            //{
            //    Debug.Log("Return 2");
            //    return;
            //}
            
            _taskTitleText.text = taskProperty.Title;
            _taskDescriptionText.text = taskProperty.Description;
            
            //_currentDragLine = _uiComponentsService.GetDragLineByType(dragLineActivity.DragLineType);
            _currentDragLine.gameObject.SetActive(true);
            _currentDragLine.MovedLineEvent += OnCurrentDragLineMoved;
            _valueText.text = _currentDragLine.GetYOffset().ToString(CultureInfo.InvariantCulture);
        }

        private void OnCurrentDragLineMoved(double value)
        {
            _valueText.text = value.ToString(CultureInfo.InvariantCulture);
        }
        
        private void OnConfirmValueBtnClick()
        {
            if (_currentDragLine == null || _valueText == null)
            {
                return;
            }
            
            //_tasksService.TryCompleteTask(new DragLineActivity(_currentDragLine.DragLineType, float.Parse(_valueText.text, CultureInfo.InvariantCulture)));
        }
    }
}