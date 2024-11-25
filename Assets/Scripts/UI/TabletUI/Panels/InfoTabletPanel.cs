using Core;
using Core.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TabletUI.Panels
{
    public class InfoTabletPanel : BasePanel<TabletPanelsType>
    {
        [SerializeField] private TextMeshProUGUI _infoText;

        [SerializeField] private Button _mainPanelBtn;

        private TasksService _tasksService;

        private void OnEnable()
        {
            if (_tasksService.CurrentTask == null)
            {
                return;
            }
        
            _infoText.text = string.IsNullOrWhiteSpace(_tasksService.CurrentTask.Warning) ? " " :_tasksService.CurrentTask.Warning;
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
            if (_tasksService.CurrentTask == null)
            {
                return;
            }
        
            switch (_tasksService.CurrentTask.LabActivity.ActivityType)
            {
                default:
                    SwitchPanel(TabletPanelsType.MainPanel);
                    break;
            }
        }
    }
}
