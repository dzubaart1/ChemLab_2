using BioEngineerLab.Activities;
using BioEngineerLab.Core;
using BioEngineerLab.Gameplay;
using UnityEngine;

namespace BioEngineerLab.Containers
{
    [RequireComponent(typeof(LabContainer))]
    public class AnchorLabContainer : MonoBehaviour, ISaveable
    {
        private struct SavedData
        {
            public Anchor Anchor;
        }
        
        private SaveService _saveService;
        private TasksService _tasksService;

        private Anchor _anchor;
        private SavedData _savedData = new SavedData();
        private LabContainer _labContainer;
        private bool _isTaskSendable;

        private void Awake()
        {
            _labContainer = GetComponent<LabContainer>();
            
            _saveService = Engine.GetService<SaveService>();
            _saveService.LoadSceneStateEvent += OnLoadScene;
            _saveService.SaveSceneStateEvent += OnSaveScene;

            _tasksService = Engine.GetService<TasksService>();
        }

        private void Start()
        {
            OnSaveScene();
        }

        public Anchor GetAnchor()
        {
            return _anchor;
        }

        public void SetAnchor(Anchor anchor)
        {
            _anchor = anchor;
        }

        public void OnSaveScene()
        {
            _savedData.Anchor = _anchor;
        }

        public void OnLoadScene()
        {
            _isTaskSendable = true;

            if (_anchor != null)
            {
                ReleaseAnchor();
            }

            if (_savedData.Anchor != null)
            {
                _anchor = _savedData.Anchor;
                PutAnchor(_anchor);
            }

            _isTaskSendable = false;
        }

        public void PutAnchor(Anchor anchor)
        {
            if (_anchor != null)
            {
                return;
            }
            
            _anchor = anchor;
            _anchor.TogglePhysics(false);
            _anchor.transform.parent = transform;
            _anchor.transform.localPosition = new Vector3(0, 0.01f, 0);
            _anchor.transform.rotation = Quaternion.identity;

            if (_isTaskSendable)
            {
                return;
            }
            
            _tasksService.TryCompleteTask(new AnchorLabActivity(_labContainer.ContainerType));
        }

        private void ReleaseAnchor()
        {
            if (_anchor == null)
            {
                return;
            }
            
            _anchor.transform.parent = null;
            _anchor.TogglePhysics(true);
            
            _anchor = null;
        }
    }
}