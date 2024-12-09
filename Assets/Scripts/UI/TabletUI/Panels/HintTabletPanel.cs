using Core;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TabletUI.Panels
{
    public class HintTabletPanel : BasePanel<TabletPanelsType>
    {
        [SerializeField] private Image _hintImage;
        [SerializeField] private Button _mainPanelBtn;

        [CanBeNull] private GameManager _gameManager;
        
        private void Awake()
        {
            _gameManager = GameManager.Instance;
        }
        
        private void OnEnable()
        {
            if (_gameManager == null)
            {
                return;
            }
            
            /*if (_tasksService.CurrentTask.HasHintSprite)
            {
                _hintImage.sprite = _tasksService.GetCurrentTask().HintSprite;
            }*/
            
            _mainPanelBtn.onClick.AddListener(OnClickMainPanelBtn);
        }

        private void OnDisable()
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
