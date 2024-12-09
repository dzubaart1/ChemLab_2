using BioEngineerLab.Tasks;
using Core;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

namespace UI.TabletUI.Panels
{
    public class MainTabletPanel : BasePanel<TabletPanelsType>
    {
        [SerializeField] private TextMeshProUGUI _taskTitleText;
        [SerializeField] private TextMeshProUGUI _taskDescriptionText;
        
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
            
            _gameManager.Game.TaskUpdatedEvent += OnTaskUpdate;
        }

        private void OnDisable()
        {
            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.Game.TaskUpdatedEvent -= OnTaskUpdate;
        }

        private void Start()
        {
            if (_gameManager == null)
            {
                return;
            }
            
            if (_gameManager.Game.CurrentTask != null)
            {
                OnTaskUpdate(_gameManager.Game.CurrentTask);
            }
        }
        
        private void OnTaskUpdate(LabTask task)
        {
            _taskTitleText.text = task.Title;
            _taskDescriptionText.text = task.Description;
        }
    }
}