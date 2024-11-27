using BioEngineerLab.Activities;
using Configurations;
using Core;
using Core.Services;
using Mechanics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace Gameplay
{
    [RequireComponent(typeof(VRGrabInteractable))]
    public class HandModelChanger : MonoBehaviour, ISaveable
    {
        private struct SavedData
        {
            public bool IsVisible;
        }

        [SerializeField] private MeshRenderer _meshRenderer;
        [SerializeField] private PlayerHand.PlayerHandType _handType;
        [SerializeField] private HandModelConfiguration.HandModelType _handModelType;

        private SaveService _saveService;
        private TasksService _tasksService;
        
        private VRGrabInteractable _vrGrabInteractable;
        private SavedData _savedData = new SavedData();
        
        private void Awake()
        {
            _tasksService = Engine.GetService<TasksService>();
            
            _saveService = Engine.GetService<SaveService>();
            _saveService.SaveSceneStateEvent += OnSaveScene;
            _saveService.LoadSceneStateEvent += OnLoadScene;

            _vrGrabInteractable = GetComponent<VRGrabInteractable>();
            _vrGrabInteractable.selectEntered.AddListener(OnSelectEntered);
        }

        private void Start()
        {
            OnSaveScene();
        }

        private void OnDestroy()
        {
            _saveService.SaveSceneStateEvent -= OnSaveScene;
            _saveService.LoadSceneStateEvent -= OnLoadScene;
            _vrGrabInteractable.selectEntered.RemoveListener(OnSelectEntered);
        }

        private void OnSelectEntered(SelectEnterEventArgs args)
        {
            PlayerHand playerHand = args.interactorObject.transform.GetComponent<PlayerHand>();

            if (playerHand.HandType == _handType)
            {
                playerHand.ToggleHandModel(_handModelType);
                _meshRenderer.enabled = false;
                _vrGrabInteractable.interactionManager.SelectExit(args.interactorObject, _vrGrabInteractable);
                _tasksService.TryCompleteTask(new MachineLabActivity(EMachineActivity.OnStart, EMachine.HandModelChangerMachine));
            }
        }

        public void OnSaveScene()
        {
            _savedData.IsVisible = _meshRenderer.enabled;
        }

        public void OnLoadScene()
        {
            _meshRenderer.enabled = _savedData.IsVisible;
        }
    }
}