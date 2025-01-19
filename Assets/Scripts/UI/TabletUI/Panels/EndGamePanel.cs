using BioEngineerLab.Tasks;
using UI.Components;
using UnityEngine;
using Core;

namespace UI.TabletUI.Panels
{
    public class EndGamePanel : BaseTabletPanel
    {
        [SerializeField] private ButtonComponent _resultButton;

        private void OnEnable()
        {
            _resultButton.ClickBtnEvent += OnClickResultButton;
        }

        private void OnDisable()
        {
            _resultButton.ClickBtnEvent -= OnClickResultButton;
        }

        private void OnClickResultButton()
        {
            GameManager gameManager = GameManager.Instance;
            if (gameManager == null)
            {
                return;
            }
            
            gameManager.LoadScene(GameManager.LOBBY_SCENE_NAME);
        }

        public override void SetTaskToShow(LabTask task)
        {
        }

        public override void SetLabToShow(ELab lab)
        {
        }
    }
}
