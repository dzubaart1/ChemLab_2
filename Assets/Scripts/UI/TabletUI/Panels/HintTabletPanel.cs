using System;
using System.IO;
using BioEngineerLab.Tasks;
using Core;
using BioEngineerLab.Tasks.SideEffects;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TabletUI.Panels
{
    public class HintTabletPanel : BaseTabletPanel, ISideEffectActivator
    {
        [SerializeField] private Image _taskHintImage;
        [SerializeField] private Button _returnButton;
        
        private void Start()
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
            
            gameManager.CurrentBaseLocalManager.AddSideEffectActivator(this);
            _taskHintImage.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _returnButton.onClick.AddListener(OnReturnButtonClicked);
        }

        private void OnDisable()
        {
            _returnButton.onClick.RemoveListener(OnReturnButtonClicked);
        }

        private void Update()
        {
            
        }

        private void OnReturnButtonClicked()
        {
            TabletUI.SwitchToMainPanel();
        }

        public override void SetTaskToShow(LabTask task)
        {
            _taskHintImage.gameObject.SetActive(false);
        }

        public override void SetLabToShow(ELab lab)
        {
        }
        
        public void OnActivateSideEffect(LabSideEffect sideEffect)
        {
            if (sideEffect is not SetHintImgSideEffect setHintImgSideEffect)
            {
                return;
            }

            _taskHintImage.gameObject.SetActive(true);
        }
    }
}