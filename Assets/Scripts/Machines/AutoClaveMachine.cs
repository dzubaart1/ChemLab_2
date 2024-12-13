using System.Collections;
using BioEngineerLab.Activities;
using Core;
using JetBrains.Annotations;
using Mechanics;
using UI.Components;
using UnityEngine;

namespace Machines
{
    public class AutoClaveMachine : MonoBehaviour, ISaveable
    {
        private class SavedData
        {
            public bool IsOn;
            public bool IsOpen;
        }

        [SerializeField] private ButtonComponent _powerButton;
        [SerializeField] private ButtonComponent _pullButton;
        [SerializeField] private VRSocketInteractor[] _socketInteractors;
        [SerializeField] private Transform _door;
        
        [CanBeNull] private GameManager _gameManager;
        
        private bool _isOpen = false;
        private bool _isPulled = false;
        private SavedData _savedData = new SavedData();
        private Animator _animator;

        private void Awake()
        {
            _gameManager = GameManager.Instance;
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.Game.LoadGameEvent += OnLoadScene;
            _gameManager.Game.SaveGameEvent += OnSaveScene;
            
            _pullButton.ClickBtnEvent += OnPullButtonClick;
        }

        private void OnDisable()
        {
            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.Game.LoadGameEvent -= OnLoadScene;
            _gameManager.Game.SaveGameEvent -= OnSaveScene;
            
            _pullButton.ClickBtnEvent -= OnPullButtonClick;
        }

        private void Start()
        {
            OnSaveScene();
        }

        private void Update()
        {
            if (_gameManager == null)
            {
                return;
            }
            
            if (_door.transform.rotation.z > -0.49f && !_isOpen)
            {
                _isOpen = true;
            }
            
            else if (_door.transform.rotation.z < -0.49f && _isOpen)
            {
                _isOpen = false;
                _gameManager.Game.CompleteTask(new DoorLabActivity(EDoor.AutoClaveDoor, EDoorActivity.Closed));
            }
        }

        private void OnPullButtonClick()
        {
            if (_isPulled)
            {
                _isPulled = false;
                _animator.Play("Close");
            }
            else
            {
                _isPulled = true;
                _animator.Play("Open");
            }
        }
        public void OnSaveScene()
        {
            _savedData.IsOpen = _isOpen;
        }

        public void OnLoadScene()
        {
            if (_savedData.IsOpen)
            {
                _isOpen = true;
                _door.transform.rotation = new Quaternion(-0.7f, 0, 0, 0.7f);
            }
            else
            {
                _isOpen = false;
                if (_isPulled)
                {
                    StartCoroutine(PlayCloseAnimation());
                }
                else
                {
                    _door.transform.rotation = new Quaternion(-0.5f, -0.5f, -0.5f, 0.5f);
                }
            }
        }

        private IEnumerator PlayCloseAnimation()
        {
            _door.transform.rotation = new Quaternion(-0.7f, 0, 0, 0.7f);
            _animator.Play("Close");
            yield return new WaitForSeconds(1.0f);
            _isOpen = false;
            _door.transform.rotation = new Quaternion(-0.5f, -0.5f, -0.5f, 0.5f);
        }
    }
}