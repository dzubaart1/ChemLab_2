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

        public ButtonType ButtonType;
        public bool IsTaskSendable = true;
        public Button Button { get; private set; }

        private TasksService _tasksService;

        private Button _button;

        [SerializeField] private Sprite _pressedSprite;
        [SerializeField] private Image _imageButton;

        private void Start()
        {
            _tasksService = Engine.GetService<TasksService>();

            _button = GetComponent<Button>();
            Button = _button;

            _button.onClick.AddListener(OnClickBtn);
        }

        protected virtual void OnClickBtn()
        {
            if (_pressedSprite != null && _imageButton!= null)
            {
                _imageButton.sprite = _pressedSprite;
            }
            OnClickButton?.Invoke();
            if (IsTaskSendable)
            {
                _tasksService.TryCompleteTask(new ButtonClickedActivity(ButtonType));
            }
        }
    }
}