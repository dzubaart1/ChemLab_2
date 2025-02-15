﻿using System;
using BioEngineerLab.Activities;
using Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Components
{
    public class ButtonComponent : MonoBehaviour
    {
        public event Action ClickBtnEvent;

        [Header("Configs")]
        [SerializeField] private EButton _buttonType;
        [SerializeField] private bool _isTaskSendable;
        [SerializeField] private bool _isStartOn;
        
        [Header("UIs")]
        [SerializeField] private Image _btnImage;
        [SerializeField] private Sprite _onSprite;
        [SerializeField] private Sprite _offSprite;
        [SerializeField] private Button _button;

        public bool IsOn { get; private set; }

        private void Start()
        {
            IsOn = _isStartOn;
        }
        
        private void OnEnable()
        {
            _button.onClick.AddListener(OnClickBtn);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClickBtn);
        }

        public void SetIsOn(bool value, bool isLoad = true)
        {
            IsOn = value;
            _btnImage.sprite = IsOn ? _onSprite : _offSprite;
            
            if (!isLoad)
            {
                ActivitySend();   
            }
        }

        private void OnClickBtn()
        {
            IsOn = !IsOn;
            _btnImage.sprite = IsOn ? _onSprite : _offSprite;
            ActivitySend();
        }

        private void ActivitySend()
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
            
            ClickBtnEvent?.Invoke();
            
            if (_isTaskSendable)
            {
                gameManager.CurrentBaseLocalManager.OnActivityComplete(new ButtonClickedActivity(_buttonType));
            }
        }
    }
}