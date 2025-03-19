using System;
using BioEngineerLab.Tasks;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TabletUI.Panels
{
    public class LoadLobbyPanel : BaseTabletPanel
    {
        [Header("UIs")]
        [SerializeField] private TMP_Text _loadLobbyText;
        [SerializeField] private Button _cancelButton;
        [SerializeField] private Button _confirmButton;

        private void OnEnable()
        {
            GameManager gameManager = GameManager.Instance;
            if (gameManager == null)
            {
                return;
            }
            
            switch (gameManager.CurrentLanguage)
            {
                case ELabLanguage.English:
                    _loadLobbyText.text = $"Are you sure you want to download the lobby?";
                    break;
                case ELabLanguage.Russia:
                    _loadLobbyText.text = $"Вы уверены, что хотите загрузить лобби?";
                    break;
                default:
                    Debug.LogError("Can't find language!");
                    break;
            }
            
            _confirmButton.onClick.AddListener(OnConfirmBtnClick);
            _cancelButton.onClick.AddListener(OnCancelBtnClick);
        }

        private void OnDisable()
        {
            _confirmButton.onClick.RemoveListener(OnConfirmBtnClick);
            _cancelButton.onClick.RemoveListener(OnCancelBtnClick);
        }

        private void OnCancelBtnClick()
        {
            TabletUI.SwitchToMainPanel();
        }

        private void OnConfirmBtnClick()
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
        
            gameManager.IsGameFinished = true;
            gameManager.LoadScene(GameManager.LOBBY_SCENE_NAME);
            TabletUI.SwitchToPreviewPanel();
        }

        public override void SetTaskToShow(LabTask task)
        {
        }

        public override void SetLabToShow(ELab lab)
        {
        }
    }
}