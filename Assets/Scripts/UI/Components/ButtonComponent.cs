using System;
using BioEngineerLab.Activities;
using BioEngineerLab.Core;
using UnityEngine;
using UnityEngine.UI;

namespace BioEngineerLab.UI.Components
{
    [RequireComponent(typeof(Button))]
    public class ButtonComponent : MonoBehaviour
    {
        public event Action OnClickButton;

        [Header("Configs")]
        [SerializeField] private EButton _buttonType;
        [SerializeField] private bool _isTaskSendable;
        
        [Header("UIs")]
        [SerializeField] private Image _btnImage;
        [SerializeField] private Sprite _onSprite;
        [SerializeField] private Sprite _offSprite;

        public bool IsOn { get; private set; }

        private Button _button;

        private TasksService _tasksService;
        
        private void Start()
        {
            _tasksService = Engine.GetService<TasksService>();
            _button = GetComponent<Button>();
        }

        public void SetIsOn(bool value)
        {
            IsOn = value;
            ChangeStatus();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClickBtn);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClickBtn);
        }

        protected virtual void OnClickBtn()
        {
            IsOn = !IsOn;
            ChangeStatus();
        }

        private void ChangeStatus()
        {
            _btnImage.sprite = IsOn ? _onSprite : _offSprite;
            
            OnClickButton?.Invoke();
            
            if (_isTaskSendable)
            {
                _tasksService.TryCompleteTask(new ButtonClickedActivity(_buttonType));
            }
        }
    }
}