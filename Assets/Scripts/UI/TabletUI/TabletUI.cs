using BioEngineerLab.Tasks;
using Core;
using JetBrains.Annotations;
using UnityEngine;

namespace UI.TabletUI
{
    [RequireComponent(typeof(TabletPanelSwitcher))]
    public class TabletUI : MonoBehaviour
    {
        [SerializeField] private TabletPanelSwitcher _panelSwitcher;
        
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
            
            _gameManager.Game.TaskFailedEvent += OnTaskFailed;
            _gameManager.Game.TaskUpdatedEvent += OnTaskUpdated;
            _gameManager.Game.FinishGameEvent += OnFinishGame;
        }

        private void OnDisable()
        {
            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.Game.TaskFailedEvent -= OnTaskFailed;
            _gameManager.Game.TaskUpdatedEvent -= OnTaskUpdated;
            _gameManager.Game.FinishGameEvent -= OnFinishGame;
        }

        private void OnTaskFailed()
        {
            _panelSwitcher.SwitchPanel(TabletPanelsType.TaskFailedPanel);
        }

        private void OnFinishGame()
        {
            _panelSwitcher.SwitchPanel(TabletPanelsType.EndGamePanel);
        }

        private void OnTaskUpdated(LabTask task)
        {
            _panelSwitcher.SwitchPanel(TabletPanelsType.MainPanel);
        }
    }
}