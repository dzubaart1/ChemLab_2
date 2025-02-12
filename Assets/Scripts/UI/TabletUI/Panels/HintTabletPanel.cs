using System;
using System.IO;
using BioEngineerLab.Tasks;
using Core;
using BioEngineerLab.Tasks.SideEffects;
using Database;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TabletUI.Panels
{
    public class HintTabletPanel : BaseTabletPanel, ISideEffectActivator
    {
        [SerializeField] private Image _taskHintImage;
        [SerializeField] private Button _returnButton;
        
        public void Init()
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
        }

        private void OnEnable()
        {
            _returnButton.onClick.AddListener(OnReturnButtonClicked);
        }

        private void OnDisable()
        {
            _returnButton.onClick.RemoveListener(OnReturnButtonClicked);
        }

        private void OnReturnButtonClicked()
        {
            TabletUI.SwitchToMainPanel();
        }

        public void NewTask()
        {
            _taskHintImage.sprite = null;
        }
        
        public void OnActivateSideEffect(LabSideEffect sideEffect)
        {
            if (sideEffect is not SetHintImgSideEffect setHintImgSideEffect)
            {
                return;
            }
            _taskHintImage.sprite = ResourcesDatabase.ReadHintImage(setHintImgSideEffect.HintImageFullName);
        }

        public override void SetTaskToShow(LabTask task)
        {
        }

        public override void SetLabToShow(ELab lab)
        {
        }
    }
}