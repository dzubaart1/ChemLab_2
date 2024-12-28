using System;
using System.Collections.Generic;
using BioEngineerLab.Tasks;
using Core;
using UI.TabletUI.Panels;
using UnityEngine;

namespace UI.TabletUI
{
    public class TabletUI : MonoBehaviour
    {
        public enum ETabletUIPanel
        {
            EndGamePanel,
            MainPanel,
            TaskFailedPanel
        }
        
        [SerializeField] private List<BaseTabletPanel> _panels;

        private BaseTabletPanel _currentPanel;

        private void Awake()
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
            
            gameManager.CurrentBaseLocalManager.AddTabletUI(this);
        }

        private void Start()
        {
            foreach (var panel in _panels)
            {
                panel.gameObject.SetActive(false);
            }
        }

        public void OnTaskFailed()
        {
            if (!TryGetPanel(ETabletUIPanel.TaskFailedPanel, out BaseTabletPanel panel))
            {
                return;
            }
            
            _currentPanel.gameObject.SetActive(false);
            panel.gameObject.SetActive(true);
        }

        public void OnFinishGame()
        {
            if (!TryGetPanel(ETabletUIPanel.EndGamePanel, out BaseTabletPanel panel))
            {
                return;
            }
            
            _currentPanel.gameObject.SetActive(false);
            panel.gameObject.SetActive(true);
        }

        public void OnTaskUpdated(LabTask task)
        {
            if (!TryGetPanel(ETabletUIPanel.MainPanel, out BaseTabletPanel panel))
            {
                return;
            }
            
            _currentPanel.gameObject.SetActive(false);
            panel.gameObject.SetActive(true);
            panel.SetTaskToShow(task);
        }

        private bool TryGetPanel(ETabletUIPanel tabletUIPanelType, out BaseTabletPanel tabletPanel)
        {
            tabletPanel = null;
            
            foreach (var panel in _panels)
            {
                if (panel.TabletPanelType == tabletUIPanelType)
                {
                    tabletPanel = panel;
                    return true;
                }
            }

            return false;
        }
    }
}