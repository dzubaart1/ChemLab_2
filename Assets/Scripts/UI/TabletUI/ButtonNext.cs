using Core;
using Core.Services;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TabletUI
{
    public class ButtonNext : MonoBehaviour
    {
        [SerializeField] private Button _nextButton;

        private TasksService _tasksService;

        private void Awake()
        {
            _tasksService = Engine.GetService<TasksService>();

            _nextButton.onClick.AddListener(OnClickNextBtn);
        }

        private void OnClickNextBtn()
        {
            //_tasksService.TryCompleteTask(new ButtonClickedActivity(ButtonType.NextButton));
        }
    }
}