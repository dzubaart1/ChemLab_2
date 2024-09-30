using System.Collections.Generic;
using BioEngineerLab.Core;
using BioEngineerLab.Gameplay;
using BioEngineerLab.Substances;
using BioEngineerLab.Tasks;
using UnityEngine;

namespace BioEngineerLab.Containers
{
    [RequireComponent(typeof(Container))]
    public class ReagentsContainer : MonoBehaviour, ISaveable
    {
        private struct SavedData
        {
            public Substance Substance;
        }

        [SerializeField] private SubstanceProperty _reagentsSubstanceProperty;
        
        private SavedData _savedData;

        private SaveService _saveService;
        private TasksService _tasksService;
        private Container _container;

        private int _prevTaskNumber;
        
        private void Awake()
        {
            _saveService = Engine.GetService<SaveService>();
            _saveService.SaveSceneStateEvent += OnSaveScene;
            _saveService.LoadSceneStateEvent += OnLoadScene;
            
            _tasksService = Engine.GetService<TasksService>();
            _tasksService.TaskUpdatedEvent += OnUpdateTask;
            
            _savedData = new SavedData();
            
            _container = GetComponent<Container>();
            _container.SubstancePrefabRenderer.enabled = true;
            _container.SubstancePrefabRenderer.material.color = _reagentsSubstanceProperty.Color;
        }

        private void Start()
        {
            OnSaveScene();
        }

        private void OnDestroy()
        {
            _saveService.SaveSceneStateEvent -= OnSaveScene;
            _saveService.LoadSceneStateEvent -= OnLoadScene;
        }

        public void OnSaveScene()
        {
            TryAddNewSubstanceFromTask(_tasksService.GetCurrentTask());
            _savedData.Substance = _container.PeekLastSubstance();
        }

        public void OnLoadScene()
        {
            if (_savedData.Substance == null)
            {
                return;
            }
            
            _container.UpdateSubstancesList(new List<Substance>{_savedData.Substance});
        }
        
        private void OnUpdateTask(TaskProperty taskProperty)
        {
            if (_tasksService.GetCurrentTask().SaveableTask)
            {
                return;
            }
            
            TryAddNewSubstanceFromTask(taskProperty);
        }

        private void TryAddNewSubstanceFromTask(TaskProperty taskProperty)
        {
            if (!taskProperty.IsSubstanceAdding || taskProperty.SubstanceName != _reagentsSubstanceProperty.SubstanceName)
            {
                return;
            }
            
            _container.AddSubstance(new Substance(_reagentsSubstanceProperty, taskProperty.SubstanceWeight));
        }
    }
}