using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BioEngineerLab.Activities;
using BioEngineerLab.Gameplay;
using BioEngineerLab.JSON;
using BioEngineerLab.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace BioEngineerLab.Core
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
        
        public event Action<TaskProperty> TaskUpdatedEvent;
        public event Action EndTasksListEvent;
        public event Action TaskFailedEvent;

        public IReadOnlyCollection<TaskProperty> TasksList => _tasksList;
        private List<TaskProperty> _tasksList;
        
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
        
        public Task Initialize()
        {
            _tasksList = new List<TaskProperty>();
            LoadJSONTasks();

            return Task.CompletedTask;
        }

        public void Destroy()
        {
        }

        public TaskProperty GetCurrentTask()
        {
            return _tasksList[_currentTaskId];
        }

        public void MoveToNextTask()
        {
            if (_currentTaskId + 1 == _tasksList.Count-1)
            {
                EndTasksListEvent?.Invoke();
                return;
            }

            _currentTaskId++;
            if (_tasksList[_currentTaskId].SaveableTask)
            {
                _saveService.SaveSceneState();
            }
            TaskUpdatedEvent?.Invoke(_tasksList[_currentTaskId]);
        }
        
        public void MoveToPrevTask()
        {
            if (_currentTaskId - 1 == -1)
            {
                EndTasksListEvent?.Invoke();
                return;
            }
            
            _currentTaskId--;
            TaskUpdatedEvent?.Invoke(_tasksList[_currentTaskId]);
        }

        public void TryCompleteTask(Activity activity)
        {
            if (_currentTaskId == (_tasksList.Count - 1))
            {
                return;
            }
            
            Debug.Log($"Try complete #{_currentTaskId} {activity.ActivityType}; Current {_tasksList[_currentTaskId].TaskActivity}");
            
            bool isTaskCompleted = activity.EqualsActivity(_tasksList[_currentTaskId].TaskActivity);
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

        private void LoadJSONTasks()
        {
            BetterStreamingAssets.Initialize();
            var allfiles = BetterStreamingAssets.GetFiles($"tasks/");

            foreach (var file in allfiles)
            {
                if (file.Contains("meta"))
                {
                    continue;
                }
                
                var settings = new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All
                };

                var task = (TaskProperty)JsonConvert.DeserializeObject(BetterStreamingAssets.ReadAllText(file), settings);

                if (task.IsTaskChangeSprite)
                {
                    task.Sprite = Resources.Load<Sprite>($"TasksSprites/{task.SpriteName}");
                }
                
                if (task.HasHintSprite)
                {
                    task.HintSprite = Resources.Load<Sprite>($"HintSprites/{task.HintSpriteName}");
                }
                
                _tasksList.Add(task);
            }
            
            _tasksList.Sort(new TaskComparer());
        }

        public static void LoadAllTasksToScriptableObjects()
        {
            List<TasksPropertyScriptableObject> _scriptableObjects =
                Resources.LoadAll<TasksPropertyScriptableObject>("Tasks").ToList();
            
            var allfiles = Directory.GetFiles($"{Application.streamingAssetsPath}/tasks");

            foreach (var tasksPropertyScriptable in _scriptableObjects)
            {
                string fileName = allfiles.First(fileName => !fileName.Contains("meta") && GetTaskNumberFromFileName(fileName) == tasksPropertyScriptable.TaskProperty.Number);

                if (string.IsNullOrWhiteSpace(fileName))
                {
                    continue;
                }

                TaskProperty taskProperty = JSONSaver.Load<TaskProperty>(fileName);
                tasksPropertyScriptable.FillFields(taskProperty);
            }
        }
        
        public static void SaveAllTasksFromScriptableObjects()
        {
            List<TasksPropertyScriptableObject> _scriptableObjects =
                Resources.LoadAll<TasksPropertyScriptableObject>("Tasks").ToList();
            
            BetterStreamingAssets.Initialize();
            
            var allfiles = Directory.GetFiles($"{Application.streamingAssetsPath}/tasks");
            
            foreach (var file in allfiles)
            {
                File.Delete(file);
            }

            foreach (var tasksPropertyScriptable in _scriptableObjects)
            {
                tasksPropertyScriptable.Save();
            }
        }

        private static int GetTaskNumberFromFileName(string name)
        {
            return int.Parse(name.Split("_")[1].Split(".")[0]);
        }

        public List<Error> GetErrorsList()
        {
            List<Error> _errorsList = new List<Error>();

            foreach (var taskID in _errorsSet)
            {
                _errorsList.Add(new Error(_tasksList[taskID].Description, taskID));
            }
            
            return _errorsList;
        }

        public TimeSpan GetCurrentGameTime()
        {
            _endTime = DateTime.Now;
            return (_endTime - _gameStart);
        }

        public void OnSaveScene()
        {
            Debug.Log("SAVE TASK: " + _currentTaskId);
            _savedData.CurrentTaskID = _currentTaskId;
        }

        public void OnLoadScene()
        {
            _currentTaskId = _savedData.CurrentTaskID;
            TaskUpdatedEvent?.Invoke(_tasksList[_currentTaskId]);
        }
    }

    public class TaskComparer : IComparer<TaskProperty>
    {
        public int Compare(TaskProperty x, TaskProperty y)
        {
            if (x.Number == y.Number)
            {
                return 0;
            }

            return x.Number > y.Number ? 1 : -1;
        }
    }
}
