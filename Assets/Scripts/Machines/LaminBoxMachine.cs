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
        
        [Space]
        [Header("UIs")]
        [SerializeField] private ButtonComponent _lightButton;
        [SerializeField] private ButtonComponent _UVButton;
        [SerializeField] private ButtonComponent _upButton;
        [SerializeField] private ButtonComponent _openButton;
        [SerializeField] private TextMeshProUGUI _text;

        [Space]
        [Header("Configs")]
        [SerializeField] private string _openAnimatorState = "Open";
        [SerializeField] private string _closeAnimatorState = "Close";
        
        private SavedData _savedData = new SavedData();
        
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
        
        private void OnEnable()
        {
            _lightButton.ClickBtnEvent += OnLightButtonClicked;
            _openButton.ClickBtnEvent += OnOpenButtonClicked;
            _UVButton.ClickBtnEvent += OnUVButtonClicked;
            _upButton.ClickBtnEvent += OnUpButtonClicked;
        }

        private void OnDisable()
        {
            _lightButton.ClickBtnEvent -= OnLightButtonClicked;
            _openButton.ClickBtnEvent -= OnOpenButtonClicked;
            _UVButton.ClickBtnEvent -= OnUVButtonClicked;
            _upButton.ClickBtnEvent -= OnUpButtonClicked;
        }

        private void OnLightButtonClicked()
        {
            _mainLight.SetActive(_lightButton.IsOn);
        }
        private void OnUVButtonClicked()
        {
            _UVLight.SetActive(_UVButton.IsOn);
            _text.SetText("");
        }

        private void OnUpButtonClicked()
        {
            _text.SetText("20\'");
        }

        private void OnOpenButtonClicked()
        {
            _animator.Play(_openButton.IsOn ? _openAnimatorState : _closeAnimatorState);
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