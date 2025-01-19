using Core;
using Saveables;
using UI.Components;
using UnityEngine;

namespace Machines
{
    public class LaminBoxMachine : MonoBehaviour, ISaveableUI
    {
        private class SavedData
        {
            public bool IsLight;
            public bool IsOpen;
        }
        
        [Header("Refs")]
        [SerializeField] private GameObject _light;
        [SerializeField] private Animator _animator;
        
        [Space]
        [Header("UIs")]
        [SerializeField] private ButtonComponent _lightButton;
        [SerializeField] private ButtonComponent _openButton;

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
        }

        private void OnDisable()
        {
            _lightButton.ClickBtnEvent -= OnLightButtonClicked;
            _openButton.ClickBtnEvent -= OnOpenButtonClicked;
        }

        private void OnLightButtonClicked()
        {
            _light.SetActive(_lightButton.IsOn);
        }

        private void OnOpenButtonClicked()
        {
            _animator.Play(_openButton.IsOn ? _openAnimatorState : _closeAnimatorState);
        }
        
        public void SaveUIState()
        {
            _savedData.IsLight = _lightButton.IsOn;
            _savedData.IsOpen = _openButton.IsOn;
        }

        public void LoadUIState()
        {
            _lightButton.SetIsOn(_savedData.IsLight);
            _openButton.SetIsOn(_savedData.IsOpen);
            
            _light.SetActive(_lightButton.IsOn);
            _animator.Play(_openButton.IsOn ? _openAnimatorState : _closeAnimatorState);
        }
    }
}