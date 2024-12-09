using Core;
using JetBrains.Annotations;
using UI.Components;
using UnityEngine;

namespace Machines
{
    [RequireComponent(typeof(Collider))]
    public class CentrifugaContainerMachine : MonoBehaviour, ISaveable
    {
        private class SavedData
        {
            public bool IsOpen;
        }
        
        [SerializeField] private ButtonComponent _button;
        
        [CanBeNull] private Animator _animator;
        public Animator Animator
        {
            get
            {
                return _animator ??= GetComponent<Animator>();
            }
        }

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
            
            _gameManager.Game.LoadGameEvent += OnLoadScene;
            _gameManager.Game.SaveGameEvent += OnSaveScene;

            _button.ClickBtnEvent += OnClickButton;
        }

        private void OnDisable()
        {
            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.Game.LoadGameEvent -= OnLoadScene;
            _gameManager.Game.SaveGameEvent -= OnSaveScene;
            
            _button.ClickBtnEvent -= OnClickButton;
        }

        private void Start()
        {
            OnSaveScene();
        }

        private void OnClickButton()
        {
            if (_button.IsOn)
            {
                Animator.Play("Open");
            }
            else
            {
                Animator.Play("Close");
            }
        }
        public void OnSaveScene()
        {
            _savedData.IsOpen = _button.IsOn;
        }

        public void OnLoadScene()
        {
            if (_savedData.IsOpen & _button.IsOn)
            {
                _button.SetIsOn(_savedData.IsOpen);
                Animator.Play("Open");
            }
            if(!_savedData.IsOpen & _button.IsOn)
            {
                _button.SetIsOn(_savedData.IsOpen);
                Animator.Play("Close");
            }
        }
    }
}