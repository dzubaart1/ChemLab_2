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
        
        public void Initialize()
        {
            _tasksList = new List<TaskProperty>();
            LoadJSONTasks();
        }

        public void Destroy()
        {
        }
        
        public void TryCompleteTask(Activity activity)
        {
            bool isTaskCompleted = _tasksList[_currentTaskId].ActivityConfig.Activity.CompleteActivity(activity);
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

        public TaskProperty GetCurrentTask()
        {
            return _tasksList[_currentTaskId];
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

        public TimeSpan GetCurrentGameTime()
        {
            _endTime = DateTime.Now;
            return (_endTime - _gameStart);
        }

        private void MoveToNextTask()
        {
            if (_currentTaskId + 1 == _tasksList.Count-1)
            {
                EndTasksListEvent?.Invoke();
                return;
            }

            Debug.Log("hi from move");

            ActivateSideEffects(ESideEffectTime.EndTask);
            
            _currentTaskId++;
            
            ActivateSideEffects(ESideEffectTime.StartTask);
            
            if (_tasksList[_currentTaskId].SaveableTask)
            {
                _saveService.SaveSceneState();
            }
            
            TaskUpdatedEvent?.Invoke(_tasksList[_currentTaskId]);
        }
        
        private void MoveToPrevTask()
        {
            if (_currentTaskId - 1 == -1)
            {
                EndTasksListEvent?.Invoke();
                return;
            }
            
            _currentTaskId--;
            TaskUpdatedEvent?.Invoke(_tasksList[_currentTaskId]);
        }

        private void LoadJSONTasks()
        {
            BetterStreamingAssets.Initialize();
            string[] allFiles = BetterStreamingAssets.GetFiles($"tasks/");

            foreach (var file in allFiles)
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
                
                _tasksList.Add(task);
            }
            
            _tasksList.Sort(new TaskComparer());
        }

        public static void LoadAllTasksToScriptableObjects()
        {
            List<TasksPropertyScriptableObject> scriptableObjects =
                Resources.LoadAll<TasksPropertyScriptableObject>("Tasks").ToList();
            
            string[] allFiles = Directory.GetFiles($"{Application.streamingAssetsPath}/tasks");

            foreach (var tasksPropertyScriptable in scriptableObjects)
            {
                string fileName = allFiles.First(fileName => !fileName.Contains("meta") && GetTaskNumberFromFileName(fileName) == tasksPropertyScriptable.TaskProperty.Number);

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
            List<TasksPropertyScriptableObject> scriptableObjects =
                Resources.LoadAll<TasksPropertyScriptableObject>("Tasks").ToList();
            
            BetterStreamingAssets.Initialize();
            
            string[] allFiles = Directory.GetFiles($"{Application.streamingAssetsPath}/tasks");
            
            foreach (var file in allFiles)
            {
                File.Delete(file);
            }

            foreach (var tasksPropertyScriptable in scriptableObjects)
            {
                tasksPropertyScriptable.Save();
            }
        }

        private static int GetTaskNumberFromFileName(string name)
        {
            return int.Parse(name.Split("_")[1].Split(".")[0]);
        }

        public void OnSaveScene()
        {
            Debug.Log("hi from on save scene");
            _savedData.CurrentTaskID = _currentTaskId;
        }

        public void OnLoadScene()
        {
            Debug.Log("hi from on load scene");
            _currentTaskId = _savedData.CurrentTaskID;
            TaskUpdatedEvent?.Invoke(_tasksList[_currentTaskId]);
        }

        private void ActivateSideEffects(ESideEffectTime sideEffectTime)
        {
            Debug.Log("activate side effect");
            foreach (var config in _tasksList[_currentTaskId].SideEffectConfigs)
            {
                Debug.Log("activate side effect#");
                if (config.SideEffect.SideEffectTimeType == sideEffectTime)
                {
                    Debug.Log("activate side effect!!!!");
                    config.SideEffect.OnActivated();
                }
            }
        }
    }

    public class TaskComparer : IComparer<TaskProperty>
    {
        public int Compare(TaskProperty x, TaskProperty y)
        {
            if (x == null | y == null)
            {
                return 0;
            }
            
            if (x.Number == y.Number)
            {
                return 0;
            }

            return x.Number > y.Number ? 1 : -1;
        }
    }
}
