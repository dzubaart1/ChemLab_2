using Core;
using Core.Services;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TabletUI.Panels
{
    public class InfoTabletPanel : BasePanel<TabletPanelsType>
    {
        [SerializeField] private TextMeshProUGUI _infoText;
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
            
            if (_gameManager.Game.CurrentTask != null)
            {
                _infoText.text = string.IsNullOrWhiteSpace(_gameManager.Game.CurrentTask.Warning) ? " " :_gameManager.Game.CurrentTask.Warning;
            }
            
            _mainPanelBtn.onClick.AddListener(OnClickMainPanelBtn);
        }

        private void OnDisable()
        {
            _mainPanelBtn.onClick.RemoveListener(OnClickMainPanelBtn);
        }

        private void OnClickMainPanelBtn()
        {
            if (_gameManager == null)
            {
                return;
            }
            
            if (_gameManager.Game.CurrentTask == null)
            {
                return;
            }
        
            switch (_gameManager.Game.CurrentTask.LabActivity.ActivityType)
            {
                default:
                    SwitchPanel(TabletPanelsType.MainPanel);
                    break;
            }
        }
    }
}
