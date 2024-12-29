using System;
using BioEngineerLab.Tasks;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.TabletUI.Panels
{
    public class LoadLabPanel : BaseTabletPanel
    {
        [Header("UIs")]
        [SerializeField] private Button _cancelButton;
        [SerializeField] private Button _submitButton;
        [SerializeField] private TMP_Text _panelText;

        private ELab _currentLab;
        
        private void OnEnable()
        {
            _cancelButton.onClick.AddListener(OnCancelButtonClicked);
            _submitButton.onClick.AddListener(OnSubmitButtonClicked);
        }

        private void OnDisable()
        {
            _cancelButton.onClick.RemoveListener(OnCancelButtonClicked); 
            _submitButton.onClick.RemoveListener(OnSubmitButtonClicked);
        }

        public override void SetTaskToShow(LabTask task)
        {
        }

        public override void SetLabToShow(ELab lab)
        {
            _currentLab = lab;
            _panelText.text = $"Вы уверены, что хотите загрузить лабораторную работу {lab.ToString()}?";
        }

        private void OnCancelButtonClicked()
        {
            TabletUI.SwitchToMainPanel();
        }

        private void OnSubmitButtonClicked()
        {
            GameManager gameManager = GameManager.Instance;
            if (gameManager == null)
            {
                return;
            }
            
            gameManager.SetLab(_currentLab);
        }
    }
}