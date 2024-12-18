using BioEngineerLab.Activities;
using Core;
using Gameplay;
using JetBrains.Annotations;
using UnityEngine;

namespace Containers
{
    public class AnchorLabContainer : MonoBehaviour, ISaveable
    {
        private struct SavedData
        {
            public Anchor Anchor;
            public bool IsAnimating;
        }
        
        [SerializeField] private LabContainer _labContainer;
        
        private Anchor Anchor { get; set; }
        
        [CanBeNull] private GameManager _gameManager;
        
        private SavedData _savedData = new SavedData();
        private bool _isTaskSendable;

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
        }

        private void OnDisable()
        {
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

        public void OnSaveScene()
        {
            _savedData.Anchor = Anchor;
        }

        public void OnLoadScene()
        {
            _isTaskSendable = true;

            if (_savedData.Anchor == null & Anchor == null)
            {
                AnimateAnchor(_savedData.IsAnimating);
            }

            if (_savedData.Anchor == null & Anchor != null)
            {
                ReleaseAnchor();
            }

            if (_savedData.Anchor != null & Anchor == null)
            {
                PutAnchor(_savedData.Anchor);
                AnimateAnchor(_savedData.IsAnimating);
            }

            if (_savedData.Anchor != null & Anchor != null)
            {
                PutAnchor(_savedData.Anchor);
                AnimateAnchor(_savedData.IsAnimating);
            }
            
            _isTaskSendable = false;
        }

        public void PutAnchor(Anchor anchor)
        {
            if (Anchor != null)
            {
                return;
            }

            if (_gameManager == null)
            {
                return;
            }
            
            Anchor = anchor;
            Anchor.TogglePhysics(false);
            Anchor.transform.parent = transform;
            Anchor.transform.localPosition = new Vector3(0, 0.01f, 0);
            Anchor.transform.rotation = Quaternion.identity;

            if (_isTaskSendable)
            {
                return;
            }
            
            _gameManager.Game.CompleteTask(new AnchorLabActivity(_labContainer.ContainerType));
        }

        public void AnimateAnchor(bool value)
        {
            if (Anchor == null)
            {
                return;
            }
            
            Anchor.ToggleAnimate(value);
            _savedData.IsAnimating = value;
        }

        private void ReleaseAnchor()
        {
            if (Anchor == null)
            {
                return;
            }
            
            Anchor.transform.parent = null;
            Anchor.TogglePhysics(true);
            
            Anchor = null;
        }
    }
}