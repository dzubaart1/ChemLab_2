using System;
using System.Collections.Generic;
using BioEngineerLab.Activities;
using BioEngineerLab.Tasks;
using BioEngineerLab.Tasks.SideEffects;
using Database;
using JetBrains.Annotations;

namespace Core.Services
{
    public class TasksService : IService, ISaveable
    {
        private struct SavedData
        {
            public int CurrentTaskID;
        }
        
        public struct Error
        {
            public string TaskText;
            public int TaskNumber;

            public Error(string taskText, int taskNumber)
            {
                TaskText = taskText;
                TaskNumber = taskNumber;
            }
        }

        public event Action<LabSideEffect> SideEffectActivatedEvent;
        public event Action<LabTask> TaskUpdatedEvent;
        public event Action EndTasksListEvent;
        public event Action TaskFailedEvent;

        public int LabNumber => _labNumber;
        
        [CanBeNull]
        public LabTask CurrentTask
        {
            get
            {
                if (_currentTaskId < 0 | _currentTaskId >= _tasksList.Count)
                {
                    return null;
                }
                
                return _tasksList[_currentTaskId];
            }
        }

        public TimeSpan CurrentGameTime => (DateTime.Now - _gameStart);

        public IReadOnlyCollection<LabTask> TasksList => _tasksList;
        private List<LabTask> _tasksList = new List<LabTask>();

        private int _labNumber = 0;
        private int _currentTaskId = 0;
        private SavedData _savedData;

        private HashSet<int> _errorsSet;
        private static DateTime _gameStart;
        private static DateTime _endTime;

        private SaveService _saveService;

        public TasksService(SaveService saveService)
        {
            _saveService = saveService;
            _saveService.SaveSceneStateEvent += OnSaveScene;
            _saveService.LoadSceneStateEvent += OnLoadScene;

            _savedData = new SavedData();
            _gameStart = DateTime.Now;

            _errorsSet = new HashSet<int>();
            
            OnSaveScene();
        }
        
        public void Initialize()
        {
            LoadTasks(ELab.Lab1);
            Engine.Behaviour.BehaviourStartEvent += ActivateCurrentTask;
        }

        public void Destroy()
        {
            Engine.Behaviour.BehaviourStartEvent -= ActivateCurrentTask;
        }

        public void LoadTasks(ELab lab)
        {
            _tasksList = LabTasksDatabase.GetInstance().ReadAll(lab);
        }
        
        public void TryCompleteTask(LabActivity labActivity)
        {
            bool isTaskCompleted = _tasksList[_currentTaskId].LabActivity.Equals(labActivity);
            if (isTaskCompleted)
            {
                MoveToNextTask();
            }
            else
            {
                _errorsSet.Add(_currentTaskId);
                TaskFailedEvent?.Invoke();
            }
        }
        
        public List<Error> GetErrorsList()
        {
            List<Error> errorsList = new List<Error>();

            foreach (var taskID in _errorsSet)
            {
                errorsList.Add(new Error(_tasksList[taskID].Description, taskID));
            }
            
            return errorsList;
        }

        public void MoveToNextTask()
        {
            if (_currentTaskId + 1 == _tasksList.Count)
            {
                EndTasksListEvent?.Invoke();
                return;
            }

            ActivateSideEffects(ESideEffectTime.EndTask);
            
            _currentTaskId++;
            
            ActivateCurrentTask();
        }

        private void ActivateCurrentTask()
        {
            ActivateSideEffects(ESideEffectTime.StartTask);
            
            if (_tasksList[_currentTaskId].SaveableTask)
            {
                _saveService.SaveSceneState();
            }
            
            TaskUpdatedEvent?.Invoke(_tasksList[_currentTaskId]);
        }
        
        public void MoveToPrevTask()
        {
            if (_currentTaskId == 0)
            {
                EndTasksListEvent?.Invoke();
                return;
            }
            
            _currentTaskId--;
            TaskUpdatedEvent?.Invoke(_tasksList[_currentTaskId]);
        }

        public void OnSaveScene()
        {
            _savedData.CurrentTaskID = _currentTaskId;
        }

        public void OnLoadScene()
        {
            _currentTaskId = _savedData.CurrentTaskID;
            TaskUpdatedEvent?.Invoke(_tasksList[_currentTaskId]);
        }

        private void ActivateSideEffects(ESideEffectTime sideEffectTime)
        {
            foreach (var sideEffect in _tasksList[_currentTaskId].LabSideEffects)
            {
                if (sideEffect.SideEffectTimeType == sideEffectTime)
                {
                    SideEffectActivatedEvent?.Invoke(sideEffect);
                }
            }
        }
    }
}
