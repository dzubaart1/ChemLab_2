using BioEngineerLab.Tasks;
using Core;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TabletUI.Panels
{
    public class TaskFailedTabletPanel : BaseTabletPanel
    {
        [SerializeField] private Button _loadPrevSceneStateBtn;
        
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
            GameManager gameManager = GameManager.Instance;
            
            if (gameManager == null)
            {
                return;
            }

            if (gameManager.CurrentBaseLocalManager == null)
            {
                return;
            }
            
            gameManager.CurrentBaseLocalManager.LoadGame();
        }

        public override void SetTaskToShow(LabTask task)
        {
        }

        public override void SetLabToShow(ELab lab)
        {
        }
    }
}