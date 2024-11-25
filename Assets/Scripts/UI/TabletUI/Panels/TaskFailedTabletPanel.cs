using Core;
using Core.Services;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TabletUI.Panels
{
    public class TaskFailedTabletPanel : BasePanel<TabletPanelsType>
    {
        [SerializeField] private Button _loadPrevSceneStateBtn;

        private SaveService _saveService;
        
        private void Awake()
        {
            _saveService = Engine.GetService<SaveService>();
            
            _loadPrevSceneStateBtn.onClick.AddListener(OnLoadPrevStateBtn);
        }

        private void OnDestroy()
        {
            _loadPrevSceneStateBtn.onClick.RemoveListener(OnLoadPrevStateBtn);
        }

        private void OnLoadPrevStateBtn()
        {
            _saveService.LoadSceneState();
        }
    }
}