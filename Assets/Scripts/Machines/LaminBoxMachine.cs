using Core;
using Saveables;
using TMPro;
using UI.Components;
using UnityEngine;

namespace Machines
{
    public class LaminBoxMachine : MonoBehaviour, ISaveableUI
    {
        private class SavedData
        {
            public bool IsLight;
            public bool IsUVLight;
            public bool IsOpen;
        }
        
        [Header("Refs")]
        [SerializeField] private GameObject _mainLight;
        [SerializeField] private GameObject _UVLight;
        [SerializeField] private Animator _animator;
        [SerializeField] private KeyChecker _keyChecker;
        
        [Space]
        [Header("UIs")]
        [SerializeField] private ButtonComponent _lightButton;
        [SerializeField] private ButtonComponent _FButton;
        [SerializeField] private ButtonComponent _UVButton;
        [SerializeField] private ButtonComponent _upButton;
        [SerializeField] private ButtonComponent _openButton;
        [SerializeField] private Transform _states;
        [SerializeField] private TextMeshProUGUI _keyboardUnlockText;

        [SerializeField] private TextMeshProUGUI _LText;
        [SerializeField] private TextMeshProUGUI _FText;
        [SerializeField] private TextMeshProUGUI _UVText;

        [Space]
        [Header("Configs")]
        [SerializeField] private string _openAnimatorState = "Open";
        [SerializeField] private string _closeAnimatorState = "Close";
        
        private SavedData _savedData = new SavedData();
        private float _delayTimer = 1f;
        private float _timer = 0;
        private bool _isTimerActive;
        
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
            
            gameManager.CurrentBaseLocalManager.AddSaveableUI(this);
        }

        private void Update()
        {
            if (_isTimerActive)
            {
                _timer += Time.deltaTime;

                if (_timer >= _delayTimer)
                {
                    _keyboardUnlockText.transform.gameObject.SetActive(false);
                    _states.gameObject.SetActive(true);
                    _timer = 0;
                    _isTimerActive = false;
                }
            }
        }
        
        private void OnEnable()
        {
            _lightButton.ClickBtnEvent += OnLightButtonClicked;
            _FButton.ClickBtnEvent += OnFButtonClicked;
            _openButton.ClickBtnEvent += OnOpenButtonClicked;
            _UVButton.ClickBtnEvent += OnUVButtonClicked;
            _upButton.ClickBtnEvent += OnUpButtonClicked;

            _keyChecker.KeyboardUnlockedEvent += OnKeyboardUnlock;
        }

        private void OnDisable()
        {
            _lightButton.ClickBtnEvent -= OnLightButtonClicked;
            _FButton.ClickBtnEvent -= OnFButtonClicked;
            _openButton.ClickBtnEvent -= OnOpenButtonClicked;
            _UVButton.ClickBtnEvent -= OnUVButtonClicked;
            _upButton.ClickBtnEvent -= OnUpButtonClicked;
            
            _keyChecker.KeyboardUnlockedEvent -= OnKeyboardUnlock;
        }

        private void OnLightButtonClicked()
        {
            _mainLight.SetActive(_lightButton.IsOn);
            
            _LText.text = _lightButton.IsOn ? "L\nВкл." : "L\nВыкл.";
        }

        private void OnFButtonClicked()
        {
            _FText.text = _FButton.IsOn ? "F\nВкл." : "F\nВыкл.";
        }
        private void OnUVButtonClicked()
        {
            _UVLight.SetActive(_UVButton.IsOn);
            
            _UVText.text = _UVButton.IsOn ? "UV\nВкл." : "UV\nВыкл.";
        }

        private void OnUpButtonClicked()
        {
            _UVText.text = "UV\n0:20";
        }

        private void OnOpenButtonClicked()
        {
            _animator.Play(_openButton.IsOn ? _openAnimatorState : _closeAnimatorState);
        }

        private void OnKeyboardUnlock()
        {
            _keyboardUnlockText.transform.gameObject.SetActive(true);
            _states.gameObject.SetActive(false);
            
            _isTimerActive = true;
        }
        
        public void SaveUIState()
        {
            _savedData.IsLight = _lightButton.IsOn;
            _savedData.IsUVLight = _UVButton.IsOn;
            _savedData.IsOpen = _openButton.IsOn;
        }

        public void LoadUIState()
        {
            _lightButton.SetIsOn(_savedData.IsLight);
            _openButton.SetIsOn(_savedData.IsOpen);
            _UVButton.SetIsOn(_savedData.IsUVLight);
            
            _mainLight.SetActive(_lightButton.IsOn);
            _UVLight.SetActive(_UVButton.IsOn);
            _animator.Play(_openButton.IsOn ? _openAnimatorState : _closeAnimatorState);
        }
    }
}