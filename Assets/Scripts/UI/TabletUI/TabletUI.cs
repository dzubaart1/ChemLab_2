using System.Collections.Generic;
using BioEngineerLab.Tasks;
using Core;
using JetBrains.Annotations;
using Machines;
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
            LoadLabPanel,
            HintPanel,
            InfoPanel,
        }

        [Header("Refs")]
        [SerializeField] private WarningTextActivator _warningTextActivator;
        
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

        public void Init()
        {
            _warningTextActivator.Init();
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
            if (!TryGetPanel(ETabletUIPanel.MainPanel, out BaseTabletPanel mainPanel))
            {
                return;
            }
            
            if (!TryGetPanel(ETabletUIPanel.HintPanel, out BaseTabletPanel hintPanel))
            {
                return;
            }
            
            if (!TryGetPanel(ETabletUIPanel.InfoPanel, out BaseTabletPanel infoPanel))
            {
                return;
            }

            SwitchPanel(mainPanel);
            mainPanel.SetTaskToShow(task);
            hintPanel.SetTaskToShow(task);
            infoPanel.SetTaskToShow(task);
        }
        
        public void SwitchToHintPanel()
        {
            if (!TryGetPanel(ETabletUIPanel.HintPanel, out BaseTabletPanel hintPanel))
            {
                return;
            }
            
            SwitchPanel(hintPanel);
        }

        public void SwitchToInfoPanel()
        {
            if (!TryGetPanel(ETabletUIPanel.InfoPanel, out BaseTabletPanel infoPanel))
            {
                return;
            }
            
            SwitchPanel(infoPanel);
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

            targetPanel.gameObject.SetActive(true);
            _currentPanel = targetPanel;
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