using System;
using UI.Components;
using UnityEngine;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;
using Core;

namespace UI.TabletUI.Panels
{
    public class EndGamePanel : BasePanel<TabletPanelsType>
    {
        [SerializeField] private ButtonComponent _resultButton;
        
        [CanBeNull] private GameManager _gameManager;

        private void Awake()
        {
            _gameManager = GameManager.Instance;
        }
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
            _gameManager.LoadScene("FinishScene");
        }
    }
}
