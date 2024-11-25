using Core;
using Core.Services;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TabletUI.Panels
{
    public class HintTabletPanel : BasePanel<TabletPanelsType>
    {
        [SerializeField] private Image _hintImage;

        [SerializeField] private Button _mainPanelBtn;

        private TasksService _tasksService;

        private void OnEnable()
        {
            //if (_tasksService.GetCurrentTask().HasHintSprite)
            //{
            //    _hintImage.sprite = _tasksService.GetCurrentTask().HintSprite;
            //}
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
            //TaskProperty currentTask = _tasksService.GetCurrentTask();
            //switch (currentTask.ActivityConfig.ActivityType)
            //{
            //    case ActivityType.SliderValueChangedActivity:
            //        SwitchPanel(TabletPanelsType.SliderTaskPanel);
            //        break;
            //    case ActivityType.DragLineActivity:
            //        SwitchPanel(TabletPanelsType.DragLinePanel);
            //        break;
            //    default:
            //        SwitchPanel(TabletPanelsType.MainPanel);
            //        break;
            //}
        }
    }
}
