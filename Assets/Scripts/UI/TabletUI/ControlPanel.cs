using System;
using System.Collections.Generic;
using BioEngineerLab.Tasks;
using Core;
using JetBrains.Annotations;
using UI.Components;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TabletUI
{
    public class ControlPanel : MonoBehaviour
    {
        [SerializeField] private Button _hintButton;
        [SerializeField] private Button _infoButton;
        [SerializeField] private Button _closeButton;

        [SerializeField] private TabletUI _tabletUI;

        private void Start()
        {
            _closeButton.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _hintButton.onClick.AddListener(OnHintButtonClick);
            _infoButton.onClick.AddListener(OnInfoButtonClick);
            _closeButton.onClick.AddListener(OnCloseButtonClick);
        }

        private void OnDisable()
        {
            _hintButton.onClick.RemoveListener(OnHintButtonClick);
            _infoButton.onClick.RemoveListener(OnInfoButtonClick);
            _closeButton.onClick.RemoveListener(OnCloseButtonClick);
        }

        private void OnHintButtonClick()
        {
            _closeButton.gameObject.SetActive(true);
            _tabletUI.SwitchToHintPanel();
        }

        private void OnInfoButtonClick()
        {
            _closeButton.gameObject.SetActive(true);
            _tabletUI.SwitchToInfoPanel();
        }

        private void OnCloseButtonClick()
        {
            _closeButton.gameObject.SetActive(false);
            _tabletUI.SwitchToMainPanel();
        }
    }
}