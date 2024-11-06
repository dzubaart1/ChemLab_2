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
            public bool IsAnimating;
        }
        
        private Anchor Anchor { get; set; }
        
        private SaveService _saveService;
        private TasksService _tasksService;
        
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
            
            Anchor = anchor;
            Anchor.TogglePhysics(false);
            Anchor.transform.parent = transform;
            Anchor.transform.localPosition = new Vector3(0, 0.01f, 0);
            Anchor.transform.rotation = Quaternion.identity;

            if (_isTaskSendable)
            {
                return;
            }
            
            _tasksService.TryCompleteTask(new AnchorLabActivity(_labContainer.ContainerType));
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