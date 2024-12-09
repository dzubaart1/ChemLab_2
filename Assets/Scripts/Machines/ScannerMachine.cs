using Core;
using Core.Services;
using JetBrains.Annotations;
using Mechanics;
using UI.Components;
using UnityEngine;

namespace Machines
{
    [RequireComponent(typeof(Collider))]
    public class ScannerMachine : MonoBehaviour, ISaveable
    {
        private class SavedData
        {
            public bool IsStarted;
        }

        [SerializeField] private ButtonComponent _button;
        [SerializeField] private Animator _animator;
        [SerializeField] private GameObject _canvas;
        [SerializeField] private VRSocketInteractor _socketInteractor;

        [CanBeNull] private GameManager _gameManager;
        
        private SavedData _savedData = new SavedData();

        private void Awake()
        {
            _gameManager = GameManager.Instance;
        }

        private void OnEnable()
        {
            _button.ClickBtnEvent += OnScannerBtnClicked;
            
            _gameManager = GameManager.Instance;

            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.Game.LoadGameEvent += OnLoadScene;
            _gameManager.Game.SaveGameEvent += OnSaveScene;
        }

        private void OnDisable()
        {
            _button.ClickBtnEvent -= OnScannerBtnClicked;
            
            _gameManager = GameManager.Instance;

            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.Game.LoadGameEvent -= OnLoadScene;
            _gameManager.Game.SaveGameEvent -= OnSaveScene;
        }

        private void Start()
        {
            OnSaveScene();
        }

        private void OnScannerBtnClicked()
        {
            if (_socketInteractor.SelectedObject is null)
            {
                return;
            }
            
            _animator.Play(_button.IsOn ? "ButtonOn" : "ButtonOff");
            _canvas.SetActive(_button.IsOn);
        }
        public void OnSaveScene()
        {
            _savedData.IsStarted = _button.IsOn;
        }

        public void OnLoadScene()
        {
            if (!_savedData.IsStarted & _button.IsOn)
            {
                _button.SetIsOn(_savedData.IsStarted);
                _animator.Play(_button.IsOn ? "ButtonOn" : "ButtonOff");
            }
            
            if (_savedData.IsStarted & !_button.IsOn)
            {
                _button.SetIsOn(_savedData.IsStarted);
                _animator.Play(_button.IsOn ? "ButtonOn" : "ButtonOff");
            }
        }
    }
}