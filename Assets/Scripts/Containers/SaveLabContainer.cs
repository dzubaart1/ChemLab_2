using Core;
using Core.Services;
using Substances;
using UnityEngine;

namespace Containers
{
    [RequireComponent(typeof(LabContainer))]
    public class SaveLabContainer : MonoBehaviour, ISaveable
    {
        private struct SavedData
        {
            public LabSubstance[] Substances;
        }
        
        private LabContainer _labContainer;
        private SavedData _savedData = new SavedData();

        private SaveService _saveService;
        
        private void Awake()
        {
            _labContainer = GetComponent<LabContainer>();

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
            _savedData.Substances = new LabSubstance[_labContainer.Substances.Count];
            
            for(int i = 0; i < _labContainer.Substances.Count; i++)
            {
                if(_labContainer.GetSubstanceByLayer((ESubstanceLayer)i) is not null)
                    _savedData.Substances[i] = new LabSubstance(_labContainer.GetSubstanceByLayer((ESubstanceLayer)i));
            }
        }

        public void OnLoadScene()
        {
            _labContainer.UpdateSubstances(_savedData.Substances);
        }
    }
}