using Activities;
using Core;
using Core.Services;
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

        private SaveService _saveService;
        private TasksService _tasksService;
        private Animator _animator;
        
        private SavedData _savedData = new SavedData();

        private void Awake()
        {
            _tasksService = Engine.GetService<TasksService>();
            _saveService = Engine.GetService<SaveService>();
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _saveService.LoadSceneStateEvent += OnLoadScene;
            _saveService.SaveSceneStateEvent += OnSaveScene;

            _button.ClickBtnEvent += PlayAnimation;
        }

        private void OnDisable()
        {
            _saveService.LoadSceneStateEvent -= OnLoadScene;
            _saveService.SaveSceneStateEvent -= OnSaveScene;
            
            _button.ClickBtnEvent -= PlayAnimation;
        }

        private void Start()
        {
            OnSaveScene();
        }

        private void PlayAnimation()
        {
            if (!_button.IsOn)
            {
                _animator.Play("Open");
                
                _tasksService.TryCompleteTask(new MachineLabActivity(EMachineActivity.OnStart, EMachine.CentrifugaContainerMachine));
            }
            else
            {
                _animator.Play("Close");

                _tasksService.TryCompleteTask(new MachineLabActivity(EMachineActivity.OnFinish, EMachine.CentrifugaContainerMachine));
            }
        }
        public void OnSaveScene()
        {
            _savedData.IsOpen = !_button.IsOn;
        }

        public void OnLoadScene()
        {
            if (_savedData.IsOpen & !_button.IsOn)
            {
                _button.SetIsOn(_savedData.IsOpen);
                _animator.Play("Open");
            }
            if(!_savedData.IsOpen & _button.IsOn)
            {
                _button.SetIsOn(_savedData.IsOpen);
                _animator.Play("Close");
            }
        }
    }
}