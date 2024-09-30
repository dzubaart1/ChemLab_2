using System;
using BioEngineerLab.Activities;
using BioEngineerLab.Core;
using BioEngineerLab.Gameplay;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace BioEngineerLab.UI.Components
{
    [RequireComponent(typeof(Slider))]
    public class SliderComponent : MonoBehaviour, ISaveable
    {
        private struct SavedData
        {
            public float Value;
        }

        public bool IsListenedBySliderPanel;
        
        public SliderType SliderType;
        public event Action<float> ValueChangedEvent;

        public float Value { get; private set; }
 
        [SerializeField] private TextMeshProUGUI _valueText;
        [SerializeField] private int _scaler;

        private UIComponentsService _uiComponentsService;
        private TasksService _tasksService;
        private SaveService _saveService;

        private Slider _slider;
        private SavedData _savedData;
        private bool _isLoadSceneValue;

        private void Awake()
        {
            _uiComponentsService = Engine.GetService<UIComponentsService>();
            _uiComponentsService.RegisterSlider(this);
            
            _saveService = Engine.GetService<SaveService>();
            _saveService.SaveSceneStateEvent += OnSaveScene;
            _saveService.LoadSceneStateEvent += OnLoadScene;

            _tasksService = Engine.GetService<TasksService>();

            _slider = GetComponent<Slider>();
            _slider.wholeNumbers = true;
            _slider.onValueChanged.AddListener(OnSliderValueChanged);

            _savedData = new SavedData();
        }

        private void Start()
        {
            UpdateValue();
            OnSaveScene();
        }

        private void OnSliderValueChanged(float value)
        {
            UpdateValue();
            
            ValueChangedEvent?.Invoke(value * _scaler);

            if (_isLoadSceneValue)
            {
                _isLoadSceneValue = false;
                return;
            }

            if (!IsListenedBySliderPanel)
            {
                _tasksService.TryCompleteTask(new SliderValueChangedActivity(SliderType, Value));
            }
        }

        private void UpdateValue()
        {
            Value = _scaler * _slider.value;
            _valueText.text = _slider.value * _scaler + "";
        }

        public void OnSaveScene()
        {
            _savedData.Value = _slider.value;
        }

        public void OnLoadScene()
        {
            _isLoadSceneValue = true;
            _slider.value = _savedData.Value;
            UpdateValue();
        }
    }
}