using System;
using System.Collections.Generic;
using BioEngineerLab.Core;
using BioEngineerLab.Gameplay;
using BioEngineerLab.Substances;
using UnityEngine;

namespace BioEngineerLab.Containers
{
    public class ContainerSave : MonoBehaviour, ISaveable
    {
        private struct SavedData
        {
            public List<Substance> Substances;
        }
        
        private Container _container;
        private SavedData _savedData;

        private SaveService _saveService;
        
        private void Awake()
        {
            _container = GetComponent<Container>();

            _saveService = Engine.GetService<SaveService>();
            _saveService.SaveSceneStateEvent += OnSaveScene;
            _saveService.LoadSceneStateEvent += OnLoadScene;

            _savedData = new SavedData();
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
            _savedData.Substances = new List<Substance>(_container.SubstancesList.Count);
            
            foreach (var substance in _container.SubstancesList)
            {
                _savedData.Substances.Add(new Substance(substance));
            }
        }

        public void OnLoadScene()
        {
            _container.UpdateSubstancesList(_savedData.Substances);
        }
    }
}