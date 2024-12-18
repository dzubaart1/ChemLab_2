using System;
using BioEngineerLab.Tasks;
using Core;
using JetBrains.Annotations;
using UnityEngine;

namespace Containers
{
    public class SaveLabContainer : MonoBehaviour, ISaveable
    {
        private struct SavedData
        {
            public LabSubstance[] Substances;
            public EContainer ContainerType;
        }
        
        [SerializeField] private LabContainer _labContainer;

        [CanBeNull] private GameManager _gameManager;
        
        private SavedData _savedData = new SavedData();
        
        private void Awake()
        {
            _gameManager = GameManager.Instance;
        }

        private void OnEnable()
        {
            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.Game.SaveGameEvent += OnSaveScene;
            _gameManager.Game.LoadGameEvent += OnLoadScene;
        }

        private void OnDisable()
        {
            if (_gameManager == null)
            {
                return;
            }
            
            _gameManager.Game.SaveGameEvent -= OnSaveScene;
            _gameManager.Game.LoadGameEvent -= OnLoadScene;
        }

        private void Start()
        {
            OnSaveScene();
        }
        
        public void OnSaveScene()
        {
            _savedData.Substances = new LabSubstance[_labContainer.Substances.Count];
            
            for(int i = 0; i < _labContainer.Substances.Count; i++)
            {
                if(_labContainer.GetSubstanceByLayer((ESubstanceLayer)i) is not null)
                    _savedData.Substances[i] = new LabSubstance(_labContainer.GetSubstanceByLayer((ESubstanceLayer)i));
            }
            
            _savedData.ContainerType = _labContainer.ContainerType;
        }

        public void OnLoadScene()
        {
            _labContainer.UpdateSubstances(_savedData.Substances);
            _labContainer.ChangeContainerType(_savedData.ContainerType);
        }
    }
}