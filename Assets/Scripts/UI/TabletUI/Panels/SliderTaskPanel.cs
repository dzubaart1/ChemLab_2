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
    public class SliderTaskPanel : BasePanel<TabletPanelsType>
    {
        [SerializeField] private TextMeshProUGUI _taskTitleText, _taskDescriptionText, _valueText;
        [SerializeField] private Button _confirmValueBtn;

        private TasksService _tasksService;
        private UIComponentsService _uiComponentsService;

        //private SliderComponent _currentSlider;

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
            //if (taskProperty.ActivityType != ActivityType.SliderValueChangedActivity)
            //{
            //    return;
            //}
            
            //if (_currentSlider != null)
            //{
            //    _currentSlider.ValueChangedEvent -= OnCurrentSliderValueChanged;
            //    _currentSlider.IsListenedBySliderPanel = false;
            //    _currentSlider = null;
            //}
            
            //SliderValueChangedActivity sliderActivity = taskProperty.TaskActivity as SliderValueChangedActivity;
            //if (sliderActivity == null)
            //{
            //    return;
            //}
            
            //_taskTitleText.text = taskProperty.Title;
            //_taskDescriptionText.text = taskProperty.Description;
            
            //_currentSlider = _uiComponentsService.GetSliderByType(sliderActivity.SliderType);
            //_currentSlider.IsListenedBySliderPanel = true;
            //_currentSlider.ValueChangedEvent += OnCurrentSliderValueChanged;
            //_valueText.text = _currentSlider.Value.ToString(CultureInfo.InvariantCulture);
        }

        private void OnCurrentSliderValueChanged(float value)
        {
            _valueText.text = value.ToString(CultureInfo.InvariantCulture);
        }
        
        private void OnConfirmValueBtnClick()
        {
            //if (_currentSlider == null || _valueText == null)
            //{
            //    return;
            //}
            
            //_tasksService.TryCompleteTask(new SliderValueChangedActivity(_currentSlider.SliderType, float.Parse(_valueText.text, CultureInfo.InvariantCulture)));
        }
    }
}