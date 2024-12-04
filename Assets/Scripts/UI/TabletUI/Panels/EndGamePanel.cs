using System;
using UI.Components;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.TabletUI.Panels
{
    public class EndGamePanel : BasePanel<TabletPanelsType>
    {
        [SerializeField] private ButtonComponent _resultButton;
        
        private void OnEnable()
        {
            _resultButton.ClickBtnEvent += OnClickButton;
        }
        
        private void OnDisable()
        {
            _resultButton.ClickBtnEvent -= OnClickButton;
        }

        private void OnClickButton()
        {
            SceneManager.LoadScene("FinishScene");
        }
    }
}
