using System;
using System.Collections.Generic;
using Core;
using Core.Services;
using JetBrains.Annotations;
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

        [CanBeNull] private GameManager _gameManager;

        private SavedData _savedData = new SavedData();

        private void Awake()
        {
            _gameManager = GameManager.Instance;
        }

        private void OnEnable()
        {
            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.Game.SaveGameEvent += OnSaveScene;
            _gameManager.Game.LoadGameEvent += OnLoadScene;
        }

        private void OnDisable()
        {
            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.Game.SaveGameEvent -= OnSaveScene;
            _gameManager.Game.LoadGameEvent -= OnLoadScene;
        }

        private void Start()
        {
            CurrentPanel = _firstShowPanel;
            
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