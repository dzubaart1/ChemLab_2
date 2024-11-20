using BioEngineerLab.Core;
using BioEngineerLab.Gameplay;
using UnityEngine;
using BioEngineerLab.UI.Components;

namespace BioEngineerLab.Machines
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

        private SaveService _saveService;
        
        private SavedData _savedData = new SavedData();

        private void Awake()
        {
            _saveService = Engine.GetService<SaveService>();
        }

        private void OnEnable()
        {
            _saveService.LoadSceneStateEvent += OnLoadScene;
            _saveService.SaveSceneStateEvent += OnSaveScene;

            _button.ClickBtnEvent += OnScannerBtnClicked;
        }

        private void OnDisable()
        {
            _saveService.LoadSceneStateEvent -= OnLoadScene;
            _saveService.SaveSceneStateEvent -= OnSaveScene;
            
            _button.ClickBtnEvent -= OnScannerBtnClicked;
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