using System;
using BioEngineerLab.Activities;
using BioEngineerLab.Configurations;
using BioEngineerLab.Core;
using BioEngineerLab.Gameplay;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BioEngineerLab.UI.Components
{
    public class DragLine : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, ISaveable
    {
        private struct SavedData
        {
            public float YPos;
        }
        
        public event Action<double> MovedLineEvent;
        
        //public DragLineType DragLineType;
        
        [Header("Constraints")]
        [SerializeField] private bool _isVerticalLock;
        [SerializeField] private float _minValue;
        [SerializeField] private float _maxValue;

        private float newX, newY, newZ;

        private UIComponentsService _uiComponentsService;
        private TasksService _tasksService;
        private SaveService _saveService;
        //private DropAnimationService _dropAnimationService;

        private SavedData _savedData;
        
        private void Awake()
        {
            _uiComponentsService = Engine.GetService<UIComponentsService>();
            _uiComponentsService.RegisterDragLine(this);
            
            _tasksService = Engine.GetService<TasksService>();
            _saveService = Engine.GetService<SaveService>();
            _saveService.SaveSceneStateEvent += OnSaveScene;
            _saveService.LoadSceneStateEvent += OnLoadScene;

            //_dropAnimationService = Engine.GetService<DropAnimationService>();
            //_dropAnimationService.StartAnimationEvent += OnAnimationStart;

            _savedData = new SavedData();
        }
        private void OnDestroy()
        {
            //_dropAnimationService.StartAnimationEvent -= OnAnimationStart;
        }

        private void Start()
        {
            OnSaveScene();
            Debug.Log("drug line show");
            gameObject.SetActive(true);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
        }

        public void OnDrag(PointerEventData eventData)
        {
            var currentRaycastPosition = eventData.pointerCurrentRaycast.worldPosition;

            newX = transform.position.x;
            newY = transform.position.y;
            newZ = transform.position.z;
            
            if (_isVerticalLock)
            {
                newY = currentRaycastPosition.y;
            }
            else
            {
                newX = currentRaycastPosition.x;
                newZ = currentRaycastPosition.z;
            }
            
            transform.position = new Vector3(newX, newY, newZ);

            if (MovedLineEvent == null)
            {
                //_tasksService.TryCompleteTask(new DragLineActivity(DragLineType, GetYOffset()));
            }
            else
            {
                MovedLineEvent.Invoke(Math.Round(transform.position.y * 100, 1));
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
        }

        public double GetYOffset()
        {
            return Math.Round(transform.position.y * 100, 1);
        }

        public void OnSaveScene()
        {
            _savedData.YPos = transform.position.y;
        }

        public void OnLoadScene()
        {
            transform.position = new Vector3(transform.position.x, _savedData.YPos, transform.position.z);
        }

        //public void OnAnimationStart(DropAnimationConfiguration.DropAnimationSubstance dropAnimationSubstance)
        //{
        //    Debug.Log("drug line hide");
        //    gameObject.SetActive(false);
        //}
    }
}