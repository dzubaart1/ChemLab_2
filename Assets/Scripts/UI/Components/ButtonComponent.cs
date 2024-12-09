using System;
using BioEngineerLab.Activities;
using Core;
using JetBrains.Annotations;
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

        [CanBeNull] private GameManager _gameManager;

        private void Awake()
        {
            _gameManager = GameManager.Instance;
        }

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

            if (!isLoad)
            {
                ChangeStatus();   
            }
        }

        private void OnClickBtn()
        {
            IsOn = !IsOn;
            ChangeStatus();
        }

        private void ChangeStatus()
        {
            if (_gameManager == null)
            {
                return;
            }
            
            _btnImage.sprite = IsOn ? _onSprite : _offSprite;
            
            ClickBtnEvent?.Invoke();
            
            if (_isTaskSendable)
            {
                _gameManager.CompleteTask(new ButtonClickedActivity(_buttonType));
            }
        }
    }
}