using Core;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TabletUI.Panels
{
    public class TaskFailedTabletPanel : BasePanel<TabletPanelsType>
    {
        [SerializeField] private Button _loadPrevSceneStateBtn;

        [CanBeNull] private GameManager _gameManager;
        
        private void Awake()
        {
            _gameManager = GameManager.Instance;
        }

        private void OnEnable()
        {
            _loadPrevSceneStateBtn.onClick.AddListener(OnLoadPrevStateBtn);
        }

        private void OnDisable()
        {
            _loadPrevSceneStateBtn.onClick.RemoveListener(OnLoadPrevStateBtn);
        }

        private void OnLoadPrevStateBtn()
        {
            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.LoadGame();
        }
    }
}