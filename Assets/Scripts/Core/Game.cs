using System;
using System.Collections.Generic;
using BioEngineerLab.Activities;
using BioEngineerLab.Tasks;
using BioEngineerLab.Tasks.SideEffects;
using Core.Services;
using Crafting;
using Database;
using JetBrains.Annotations;
using UnityEngine;

namespace Core
{
    public class Game : MonoBehaviour
    {
        private struct SavedData
        {
            public int CurrentID;
        }

        public const string FINISH_SCENE_NAME = "FinishScene";
        public const string START_SCENE_NAME = "StartScene";
        public const string LAB_1_SCENE_NAME = "Lab1";
        public const string LAB_2_SCENE_NAME = "Lab2";
        public const string LAB_3_SCENE_NAME = "Lab2";
        
        public event Action<LabSideEffect> SideEffectActivatedEvent;
        public event Action<LabTask> TaskUpdatedEvent;
        public event Action TaskFailedEvent;
        public event Action FinishGameEvent;
        public event Action SaveGameEvent;
        public event Action LoadGameEvent;
        
        public DateTime GameStartTime => _gameStartTime;
        public DateTime GameFinishTime => _gameFinishTime; 
        public ELab CurrentLab => _currentLab;
        public IReadOnlyCollection<ErrorTask> Errors => _errors;
        public IReadOnlyCollection<SOLabCraft> SOCrafts => _soCrafts;

        [CanBeNull]
        public LabTask CurrentTask
        {
            get
            {
                if (_currentTaskID >= 0 & _currentTaskID < _tasksList.Count)
                {
                    return _tasksList[_currentTaskID];
                }

                return null;
            }
        }
        
        private int _currentTaskID = 0;
        private bool _isGameStarted = false;
        private DateTime _gameStartTime = DateTime.Now;
        private DateTime _gameFinishTime = DateTime.Now;
        private ELab _currentLab = ELab.Lab1;
        private SavedData _savedData = new SavedData();
        
        private HashSet<ErrorTask> _errors = new HashSet<ErrorTask>();
        private List<LabTask> _tasksList = new List<LabTask>();
        private List<SOLabCraft> _soCrafts = new List<SOLabCraft>();

        private void Update()
        {
            if (_isGameStarted & _currentTaskID == _tasksList.Count)
            {
                FinishGame();
            }
        }
        
        public void StartGame(ELab lab)
        {
            _soCrafts = ResourcesDatabase.ReadAllCraft();
            _tasksList = LabTasksDatabase.ReadAll(lab);

            _isGameStarted = true;
         
            ResetGame(lab);
            MoveToNextTask();
        }
        
        public void FinishGame()
        {
            _isGameStarted = false;
            
            _gameFinishTime = DateTime.Now;
            
            FinishGameEvent?.Invoke();
        }

        public void SaveGame()
        {
            _savedData.CurrentID = _currentTaskID;
            
            SaveGameEvent?.Invoke();
        }

        public void LoadGame()
        {
            _currentTaskID = _savedData.CurrentID;
            LoadGameEvent?.Invoke();
            
            if (IsCorrectTaskID(_currentTaskID))
            {
                ActivateSideEffects(_tasksList[_currentTaskID], ESideEffectTime.StartTask);
                TaskUpdatedEvent?.Invoke(_tasksList[_currentTaskID]);
            }
        }

        public void CompleteTask(LabActivity activity)
        {
            if (!IsCorrectTaskID(_currentTaskID))
            {
                return;
            }

            if (_tasksList[_currentTaskID].LabActivity.Equals(activity))
            {
                MoveToNextTask();
                return;
            }
            
            _errors.Add(new ErrorTask(_tasksList[_currentTaskID].Title, _tasksList[_currentTaskID].Number));
            TaskFailedEvent?.Invoke();
        }

        private void MoveToNextTask()
        {
            if ((_currentLab == ELab.Lab2 || _currentLab == ELab.Lab3) && _currentTaskID == 8)
            {
                _currentTaskID = 13;
            }
            if (_currentLab == ELab.Lab2 && _currentTaskID == 90)
            {
                _currentTaskID = 91;
            }
            if (_currentLab == ELab.Lab2 && _currentTaskID == 93)
            {
                _currentTaskID = 94;
            }
            if (_currentLab == ELab.Lab2 && _currentTaskID == 96)
            {
                _currentTaskID = 97;
            }
            if (_currentLab == ELab.Lab2 && _currentTaskID == 99)
            {
                _currentTaskID = 100;
            }
            if (_currentLab == ELab.Lab2 && _currentTaskID == 101)
            {
                _currentTaskID = 103;
            }
            if (_currentLab == ELab.Lab2 && _currentTaskID == 136)
            {
                _currentTaskID = 137;
            }
            if (_currentLab == ELab.Lab2 && _currentTaskID == 150)
            {
                _currentTaskID = 151;
            }
            
            if (_currentLab == ELab.Lab3 && _currentTaskID == 36)
            {
                _currentTaskID = 37;
            }
            if (_currentLab == ELab.Lab3 && _currentTaskID == 38)
            {
                _currentTaskID = 40;
            }
            
            if (IsCorrectTaskID(_currentTaskID))
            {
                ActivateSideEffects(_tasksList[_currentTaskID], ESideEffectTime.EndTask);   
            }
            
            _currentTaskID++;

            if (IsCorrectTaskID(_currentTaskID))
            {
                if (_tasksList[_currentTaskID].SaveableTask)
                {
                    SaveGame();
                }

                ActivateSideEffects(_tasksList[_currentTaskID], ESideEffectTime.StartTask);
                TaskUpdatedEvent?.Invoke(_tasksList[_currentTaskID]);
            }
        }

        private void ResetGame(ELab lab)
        {
            _isGameStarted = false;
            
            _currentTaskID = -1;
            
            _gameStartTime = DateTime.Now;
            _gameFinishTime = DateTime.Now;

            _currentLab = lab;
            
            _errors.Clear();
        }
        
        private void ActivateSideEffects(LabTask labTask, ESideEffectTime sideEffectTime)
        {
            foreach (var sideEffect in labTask.LabSideEffects)
            {
                if (sideEffect.SideEffectTimeType == sideEffectTime)
                {
                    SideEffectActivatedEvent?.Invoke(sideEffect);
                }
            }
        }

        private bool IsCorrectTaskID(int id)
        {
            return id >= 0 && id < _tasksList.Count;
        }
    }
}