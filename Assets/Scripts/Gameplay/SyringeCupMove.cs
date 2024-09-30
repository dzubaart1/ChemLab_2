using System.Collections;
using System.Collections.Generic;
using BioEngineerLab.Activities;
using BioEngineerLab.Core;
using UnityEngine.XR.Content.Interaction;
using UnityEngine;

namespace BioEngineerLab.Gameplay
{
    public class SyringeCupMove : MonoBehaviour, ISaveable
    {
        public enum SyringePos
        {
            TOP,
            MIDDLE,
            BOTTOM
        }
        
        private struct SavedData
        {
            public SyringePos SyringePos;
        }

        [Header("Configuration")]
        [SerializeField] private float _speedYMoving = 0.01f;

        [Space]
        [Header("References")]
        [SerializeField] private Transform _cup;

        private SaveService _saveService;
        private TasksService _tasksService;
        
        private SavedData _savedData;

        private SyringePos _currentPos;

        private Dictionary<SyringePos, float> _poses;

        private void Awake()
        {
            _tasksService = Engine.GetService<TasksService>();
            
            _saveService = Engine.GetService<SaveService>();
            _saveService.LoadSceneStateEvent += OnLoadScene;
            _saveService.SaveSceneStateEvent += OnSaveScene;
            
            _savedData = new SavedData();

            _poses = new Dictionary<SyringePos, float>();
            _poses.Add(SyringePos.TOP, 0.0581f);
            _poses.Add(SyringePos.MIDDLE, 0.0365f);
            _poses.Add(SyringePos.BOTTOM, 0.011f);
            _currentPos = SyringePos.TOP;
        }
        
        private void Start()
        {
            OnSaveScene();
        }

        public void OnClickSyringeButton()
        {
            if (_tasksService.GetCurrentTask().IsTaskChangeSyringePos)
            {
                StartCoroutine(CupMoveTo(_tasksService.GetCurrentTask().SyringePos));
            }
            
            _tasksService.TryCompleteTask(new MachineActivity(MachineActivityType.OnStart, MachineType.SyringeCupMoveMachine));
        }
        
        IEnumerator CupMoveTo(SyringePos syringePos)
        {
            Vector3 finalPosition = new Vector3(_cup.transform.localPosition.x,_poses[syringePos], _cup.transform.localPosition.z);
            _currentPos = syringePos;
            var startPosition = _cup.transform.localPosition;
            
            float step = _speedYMoving / (startPosition - finalPosition).magnitude * Time.fixedDeltaTime;
            float t = 0;
            while (t <= 1.0f)
            {
                t += step;
                _cup.transform.localPosition = Vector3.Lerp(startPosition, finalPosition, t);
                _cup.transform.Rotate(0, Time.deltaTime * 80, 0);
                yield return new WaitForFixedUpdate();
            }
            
            _cup.transform.localPosition = finalPosition;
        }
        
        public void OnSaveScene()
        {
            _savedData.SyringePos = _currentPos;
        }

        public void OnLoadScene()
        {
            if (_currentPos == _savedData.SyringePos)
            {
                return;
            }
            
            StartCoroutine(CupMoveTo(_savedData.SyringePos));
        }
    }
}