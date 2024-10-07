using BioEngineerLab.Core;
using BioEngineerLab.Gameplay;
using BioEngineerLab.Substances;
using UnityEngine;

namespace BioEngineerLab.Containers
{
    [RequireComponent(typeof(Container))]
    public class ContainerSave : MonoBehaviour, ISaveable
    {
        private struct SavedData
        {
            public Substance[] Substances;
        }
        
        private Container _container;
        private SavedData _savedData = new SavedData();

        private SaveService _saveService;
        
        private void Awake()
        {
            _container = GetComponent<Container>();

            _saveService = Engine.GetService<SaveService>();
            _saveService.SaveSceneStateEvent += OnSaveScene;
            _saveService.LoadSceneStateEvent += OnLoadScene;
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
            _savedData.Substances = new Substance[_container.Substances.Count];
            
            for(int i = 0; i < _container.Substances.Count; i++)
            {
                _savedData.Substances[i] = new Substance(_container.GetSubstanceByLayer((ESubstanceLayer)i));
            }
        }

        public void OnLoadScene()
        {
            _container.UpdateSubstances(_savedData.Substances);
        }
    }
}