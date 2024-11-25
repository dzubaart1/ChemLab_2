using System;
using System.Collections.Generic;
using Core;
using Core.Services;
using UnityEngine;

namespace UI
{
    public class PanelSwitcher<T> : MonoBehaviour, ISaveable where T : Enum
    {
        private struct SavedData
        {
            public BasePanel<T> CurrentPanel;
        }
        
        public BasePanel<T> CurrentPanel { get; private set; }

        public bool SwitchPanelOnLoad = true;
        
        [SerializeField] private BasePanel<T> _firstShowPanel;
        [SerializeField] private List<BasePanel<T>> _panelsList;

        private SaveService _saveService;

        private SavedData _savedData;

        private void Awake()
        {
            _saveService = Engine.GetService<SaveService>();
            _saveService.SaveSceneStateEvent += OnSaveScene;
            _saveService.LoadSceneStateEvent += OnLoadScene;

            _savedData = new SavedData();
            
            CurrentPanel = _firstShowPanel;
        }

        private void Start()
        {
            foreach (var panel in _panelsList)
            {
                panel.HidePanel();
                panel.SwitchPanelRequest += SwitchPanel;
            }
            
            CurrentPanel.ShowPanel();
            OnSaveScene();
        }

        private void OnDestroy()
        {
            foreach (var panel in _panelsList)
            {
                panel.SwitchPanelRequest -= SwitchPanel;
            }
        }

        public void SwitchPanel(T panelType)
        {
            foreach (var panel in _panelsList)
            {
                if (panel.PanelType.Equals(panelType))
                {
                    CurrentPanel.HidePanel();;
                    CurrentPanel = panel;
                    CurrentPanel.ShowPanel();
                }
            }
        }

        public void OnSaveScene()
        {
            _savedData.CurrentPanel = CurrentPanel;
        }

        public void OnLoadScene()
        {
            if (SwitchPanelOnLoad)
            {
                SwitchPanel(_savedData.CurrentPanel.PanelType);
            }
        }
    }
}