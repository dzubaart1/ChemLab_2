using System;
using System.Collections;
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
using UnityEngine;

namespace LocalManagers
{
    public class LabLocalManager : BaseLocalManager
    {
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

        private List<ISaveableDoor> _saveableDoors = new List<ISaveableDoor>();
        private List<ISaveableUI> _saveableUis = new List<ISaveableUI>();
        private List<ISaveableOther> _saveableOthers = new List<ISaveableOther>();
        private List<ISideEffectActivator> _sideEffectActivators = new List<ISideEffectActivator>();
        private List<ISaveableContainer> _containers = new List<ISaveableContainer>();
        private List<ISaveableSocket> _sockets = new List<ISaveableSocket>();
        private List<ISaveableGrabInteractable> _grabInteractables = new List<ISaveableGrabInteractable>();

        [CanBeNull] private TabletUI _tabletUI;

        private HashSet<LabTask> _errorsTask = new HashSet<LabTask>();
        private List<LabTask> _tasksList = new List<LabTask>();
        private List<SOLabCraft> _soCrafts = new List<SOLabCraft>();

        private DateTime _gameStartTime;
        private bool _isGameStarted = false;
        private int _currentTaskID = 0;

        private int _savedTaskID;
        private bool _isError = false;

        public override IEnumerator InitLab(ELab lab)
        {
            _soCrafts = ResourcesDatabase.ReadAllCraft();
            _tasksList = LabTasksDatabase.ReadAll(lab);
            
            yield return new WaitForSeconds(1.5f);

            _isGameStarted = true;

            _currentTaskID = 0;
            _savedTaskID = 0;

            _gameStartTime = DateTime.Now;

            SaveGame();
            ActivateSideEffects(_tasksList[_currentTaskID], ESideEffectTime.StartTask);

            TabletUI tabletUI = FindObjectOfType<TabletUI>();
            if (tabletUI != null)
            {
                _tabletUI = tabletUI;
                _tabletUI.OnTaskUpdated(CurrentTask);
            }
        }

        public override void FinishGame()
        {
            GameManager gameManager = GameManager.Instance;
            if (gameManager == null)
            {
                return;
            }

            gameManager.OnFinishGame((DateTime.Now - _gameStartTime).Minutes, _errorsTask.Count);
            if (_tabletUI != null)
            {
                _tabletUI.OnFinishGame();
            }
        }

        public override void SaveGame()
        {
            _savedTaskID = _currentTaskID;
            
            foreach (var saveableDoor in _saveableDoors)
            {
                saveableDoor.SaveDoorState();
            }

            foreach (var socket in _sockets)
            {
                socket.Save();
            }

            foreach (var grabInteractable in _grabInteractables)
            {
                grabInteractable.Save();
            }

            foreach (var container in _containers)
            {
                container.Save();
            }

            foreach (var saveableUi in _saveableUis)
            {
                saveableUi.SaveUIState();
            }

            foreach (var saveableOther in _saveableOthers)
            {
                saveableOther.Save();
            }
        }

        public override void LoadGame()
        {
            GameManager gameManager = GameManager.Instance;
            if (gameManager == null)
            {
                return;
            }
            
            _isError = false;
            
            gameManager.PlayerSpawner.Player.ReleaseAllGrabbables();
            
            foreach (var socket in _sockets)
            {
                socket.ReleaseAllLoad();
            }

            foreach (var container in _containers)
            {
                container.ReleaseAnchor();
            }

            foreach (var socket in _sockets)
            {
                socket.ReleaseLocks();
            }

            foreach (var grabInteractable in _grabInteractables)
            {
                grabInteractable.LoadSavedTransform();
            }

            foreach (var socket in _sockets)
            {
                socket.PutSavedLocks();
            }

            foreach (var container in _containers)
            {
                container.PutSavedSubstances();
                container.PutSavedContainerType();
                container.PutSavedAnchor();
            }

            foreach (var saveableUi in _saveableUis)
            {
                saveableUi.LoadUIState();
            }

            foreach (var socket in _sockets)
            {
                socket.PutSavedInteractable();
            }
            
            foreach (var saveableOther in _saveableOthers)
            {
                saveableOther.Load();
            }
            
            foreach (var saveableDoor in _saveableDoors)
            {
                saveableDoor.LoadDoorState();
            }

            _currentTaskID = _savedTaskID;

            ActivateSideEffects(_tasksList[_currentTaskID], ESideEffectTime.StartTask);

            if (_tabletUI != null)
            {
                _tabletUI.OnTaskUpdated(CurrentTask);
            }
        }

        public override void OnActivityComplete(LabActivity activity)
        {
            Debug.Log($" TRY COMPLETE {activity.ActivityType}! {activity} {_tasksList[_currentTaskID].LabActivity}");

            if (!IsCorrectTaskID(_currentTaskID))
            {
                return;
            }

            if (_tasksList[_currentTaskID].LabActivity.Equals(activity))
            {
                MoveToNextTask();
                return;
            }

            if (_tabletUI != null)
            {
                _tabletUI.OnTaskFailed();
                _isError = true;
            }

            _errorsTask.Add(_tasksList[_currentTaskID]);
        }

        public override IReadOnlyList<SOLabCraft> GetSOCrafts()
        {
            return _soCrafts;
        }

        public override void AddSaveableUI(ISaveableUI saveableUI)
        {
            _saveableUis.Add(saveableUI);
        }

        public override void AddSaveableOther(ISaveableOther saveableOther)
        {
            _saveableOthers.Add(saveableOther);
        }

        public override void AddSaveableDoor(ISaveableDoor saveableDoor)
        {
            _saveableDoors.Add(saveableDoor);
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
            if (_isError)
            {
                return;
            }
            
            if (IsCorrectTaskID(_currentTaskID))
            {
                ActivateSideEffects(_tasksList[_currentTaskID], ESideEffectTime.EndTask);
            }

            _currentTaskID++;

            if (IsCorrectTaskID(_currentTaskID))
            {
                ActivateSideEffects(_tasksList[_currentTaskID], ESideEffectTime.StartTask);

                if (_tabletUI != null)
                {
                    _tabletUI.OnTaskUpdated(_tasksList[_currentTaskID]);
                }

                if (_tasksList[_currentTaskID].SaveableTask)
                {
                    SaveGame();
                }
            }

            if (_currentTaskID == _tasksList.Count)
            {
                FinishGame();
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