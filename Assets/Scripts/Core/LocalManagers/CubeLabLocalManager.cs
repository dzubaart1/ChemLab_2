using System;
using System.Collections.Generic;
using BioEngineerLab.Activities;
using BioEngineerLab.Tasks;
using BioEngineerLab.Tasks.SideEffects;
using Core;
using Crafting;
using Database;
using JetBrains.Annotations;
using Saveables;
using UI.TabletUI;

namespace LocalManagers
{
    public class CubeLabBaseLocalManager : BaseLocalManager
    {
        public event Action<LabTask> TaskUpdatedEvent;
        public event Action TaskFailedEvent;
        
        [CanBeNull]
        public LabTask CurrentTask
        {
            get
            {
                if (IsCorrectTaskID(_currentTaskID))
                {
                    return _tasksList[_currentTaskID];
                }

                return null;
            }
        }

        private List<ISideEffectActivator> _sideEffectActivators = new List<ISideEffectActivator>();
        private List<ISaveableContainer> _containers = new List<ISaveableContainer>();
        private List<ISaveableSocket> _sockets = new List<ISaveableSocket>();
        private List<ISaveableGrabInteractable> _grabInteractables = new List<ISaveableGrabInteractable>();
        
        private HashSet<LabTask> _errorsTask = new HashSet<LabTask>();
        private List<LabTask> _tasksList = new List<LabTask>();
        private List<SOLabCraft> _soCrafts = new List<SOLabCraft>();

        private DateTime _gameStartTime;
        private DateTime _gameFinishTime;
        private bool _isGameStarted = false;
        private int _currentTaskID = 0;
        
        public override void InitLab(ELab lab)
        {
            _soCrafts = ResourcesDatabase.ReadAllCraft();
            _tasksList = LabTasksDatabase.ReadAll(lab);
            
            _isGameStarted = true;
            
            _currentTaskID = 0;
            
            _gameStartTime = DateTime.Now;
            _gameFinishTime = DateTime.Now;
            
            MoveToNextTask();
        }

        public override void SaveGame()
        {
            foreach (var container in _containers)
            {
                container.Save();
            }

            foreach (var socket in _sockets)
            {
                socket.Save();
            }

            foreach (var grabInteractable in _grabInteractables)
            {
                grabInteractable.Save();
            }
        }

        public override void LoadGame()
        {
            foreach (var socket in _sockets)
            {
                socket.ReleaseAllLoad();
            }

            foreach (var grabInteractable in _grabInteractables)
            {
                grabInteractable.LoadSavedTransform();
            }

            foreach (var container in _containers)
            {
                container.PutSavedSubstances();
            }

            foreach (var socket in _sockets)
            {
                socket.PutSavedInteractable();
            }
        }

        public override void OnActivityComplete(LabActivity activity)
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
            
            _errorsTask.Add(_tasksList[_currentTaskID]);
            TaskFailedEvent?.Invoke();
        }

        public override float GetGameTime()
        {
            throw new NotImplementedException();
        }

        public override List<LabTask> GetErrorTasks()
        {
            throw new NotImplementedException();
        }

        public override List<SOLabCraft> GetSOCrafts()
        {
            return _soCrafts;
        }

        public override void AddTabletUI(TabletUI tabletUI)
        {
            throw new NotImplementedException();
        }

        public override void AddSideEffectActivator(ISideEffectActivator sideEffectActivator)
        {
            _sideEffectActivators.Add(sideEffectActivator);
        }
        
        public override void AddSaveableContainer(ISaveableContainer saveableContainer)
        {
            _containers.Add(saveableContainer);
        }

        public override void AddSaveableSocket(ISaveableSocket saveableSocket)
        {
            _sockets.Add(saveableSocket);
        }

        public override void AddGrabInteractables(ISaveableGrabInteractable saveableGrabInteractable)
        {
            _grabInteractables.Add(saveableGrabInteractable);
        }

        private void MoveToNextTask()
        {
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
        
        private void ActivateSideEffects(LabTask labTask, ESideEffectTime sideEffectTime)
        {
            foreach (var sideEffect in labTask.LabSideEffects)
            {
                if (sideEffect.SideEffectTimeType == sideEffectTime)
                {
                    foreach (var sideEffectActivator in _sideEffectActivators)
                    {
                        sideEffectActivator.OnActivateSideEffect(sideEffect);
                    }
                }
            }
        }
        
        private bool IsCorrectTaskID(int id)
        {
            return id >= 0 && id < _tasksList.Count;
        }
    }
}