using System;
using BioEngineerLab.Activities;
using BioEngineerLab.Core;
using UnityEngine;
using UnityEngine.UI;

namespace BioEngineerLab.UI.Components
{
    public class ButtonComponent : MonoBehaviour
    {
        public event Action ClickBtnEvent;

        [Header("Configs")]
        [SerializeField] private EButton _buttonType;
        [SerializeField] private bool _isTaskSendable;
        [SerializeField] private bool _isStartOn;
        
        [Header("UIs")]
        [SerializeField] private Image _btnImage;
        [SerializeField] private Sprite _onSprite;
        [SerializeField] private Sprite _offSprite;
        [SerializeField] private Button _button;
        
        public bool IsOn { get; private set; }

        private TasksService _tasksService;
        
        private void Start()
        {
            _tasksService = Engine.GetService<TasksService>();
            IsOn = _isStartOn;
        }
        
        private void OnEnable()
        {
            _button.onClick.AddListener(OnClickBtn);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClickBtn);
        }

        public void SetIsOn(bool value, bool isLoad = true)
        {
            IsOn = value;

            if (!isLoad)
            {
                ChangeStatus();   
            }
        }

        private void OnClickBtn()
        {
            IsOn = !IsOn;
            ChangeStatus();
        }

        private void ChangeStatus()
        {
            _btnImage.sprite = IsOn ? _onSprite : _offSprite;
            
            ClickBtnEvent?.Invoke();
            
            if (_isTaskSendable)
            {
                _tasksService.TryCompleteTask(new ButtonClickedActivity(_buttonType));
            }
        }
    }
}