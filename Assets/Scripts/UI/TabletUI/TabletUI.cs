using System;
using System.Collections.Generic;
using BioEngineerLab.Tasks;
using Core;
using JetBrains.Annotations;
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
            TaskFailedPanel,
            LoadLabPanel
        }
        
        [SerializeField] private List<BaseTabletPanel> _panels;

        [CanBeNull] private BaseTabletPanel _currentPanel;
        
        private void Start()
        {
            foreach (var panel in _panels)
            {
                panel.gameObject.SetActive(false);
                panel.TabletUI = this;
            }
        }

        public void OnTaskFailed()
        {
            if (!TryGetPanel(ETabletUIPanel.TaskFailedPanel, out BaseTabletPanel panel))
            {
                return;
            }

            SwitchPanel(panel);
        }

        public void OnSelectLab(ELab lab)
        {
            if(!TryGetPanel(ETabletUIPanel.LoadLabPanel, out BaseTabletPanel panel))
            {
                return;
            }
            
            SwitchPanel(panel);
            panel.SetLabToShow(lab);
        }

        public void OnFinishGame()
        {
            if (!TryGetPanel(ETabletUIPanel.EndGamePanel, out BaseTabletPanel panel))
            {
                return;
            }
            
            SwitchPanel(panel);
        }

        public void OnTaskUpdated(LabTask task)
        {
            if (!TryGetPanel(ETabletUIPanel.MainPanel, out BaseTabletPanel panel))
            {
                return;
            }
            
            SwitchPanel(panel);
            panel.SetTaskToShow(task);
        }

        public void SwitchToMainPanel()
        {
            if (!TryGetPanel(ETabletUIPanel.MainPanel, out BaseTabletPanel mainPanel))
            {
                return;
            }
            
            SwitchPanel(mainPanel);
        }

        private void SwitchPanel(BaseTabletPanel targetPanel)
        {
            if (_currentPanel != null)
            {
                _currentPanel.gameObject.SetActive(false);
            }

            _currentPanel = targetPanel;
            _currentPanel.gameObject.SetActive(true);
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