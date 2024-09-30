using BioEngineerLab.Activities;
using BioEngineerLab.Core;
using BioEngineerLab.Gameplay;
using BioEngineerLab.Substances;
using UnityEngine;

namespace BioEngineerLab.Containers
{
    public class AnchorContainer : MonoBehaviour, ISaveable
    {
        [SerializeField] private Container _container;
        
        private struct SavedData
        {
            public Anchor Anchor;
        }
        
        public Anchor Anchor { get; private set; }

        private SavedData _savedData;
        
        private SaveService _saveService;
        private TasksService _tasksService;

        private bool _loadPut;

        private void Awake()
        {
            _saveService = Engine.GetService<SaveService>();
            _saveService.LoadSceneStateEvent += OnLoadScene;
            _saveService.SaveSceneStateEvent += OnSaveScene;

            _tasksService = Engine.GetService<TasksService>();

            _savedData = new SavedData();
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
            _loadPut = true;

            if (Anchor != null)
            {
                ReleaseAnchor();
            }

            if (_savedData.Anchor != null)
            {
                Anchor = _savedData.Anchor;
                PutAnchor(Anchor);
            }
            
            _loadPut = false;
        }

        public void PutAnchor(Anchor anchor)
        {
            if (Anchor != null)
            {
                return;
            }
            
            Anchor = anchor;
            Anchor.Rigidbody.isKinematic = true;
            Anchor.Collider.enabled = false;
            Anchor.transform.parent = transform;
            Anchor.transform.localPosition = new Vector3(0, 0.01f, 0);
            Anchor.transform.rotation = Quaternion.identity;

            if (_loadPut)
            {
                return;
            }

            SubstanceName substanceName = _container.PeekLastSubstance() == null
                ? SubstanceName.Empty
                : _container.PeekLastSubstance().SubstanceProperty.SubstanceName;
            
            _tasksService.TryCompleteTask(new AnchorActivity(substanceName));
        }

        private void ReleaseAnchor()
        {
            if (Anchor == null)
            {
                return;
            }
            
            Anchor.transform.parent = null;
            Anchor.Rigidbody.isKinematic = false;
            Anchor.Collider.enabled = true;
            
            Anchor = null;
        }
    }
}