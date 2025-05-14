using BioEngineerLab.Tasks.SideEffects;
using Containers;
using Core;
using Saveables;
using UnityEngine;
using TMPro;

namespace BioEngineerLab.Machines
{
    public class DozatorMachine : MonoBehaviour, ISideEffectActivator, ISaveableOther
    {
        private class SavedData
        {
            public float Volume;
        }
        
        [Header("UIs")]
        [SerializeField] private TextMeshProUGUI _text;
        
        [Space]
        [Header("Refs")]
        [SerializeField] private LabContainer _labContainer;
        
        private float volume = 0;
        private SavedData _savedData = new SavedData();
        
        private void Start()
        {
            GameManager gameManager = GameManager.Instance;
            if (gameManager == null)
            {
                return;
            }

            if (gameManager.CurrentBaseLocalManager == null)
            {
                return;
            }
            
            gameManager.CurrentBaseLocalManager.AddSideEffectActivator(this);
            gameManager.CurrentBaseLocalManager.AddSaveableOther(this);
        }

        public void OnActivateSideEffect(LabSideEffect sideEffect)
        {
            if (sideEffect is not SetVolumeLabSideEffect setVolumeLabSideEffect)
            {
                return;
            }

            if (setVolumeLabSideEffect.Container != EContainer.DozatorContainer)
            {
                return;
            }

            _text.text = setVolumeLabSideEffect.Volume.ToString("F4");
            volume = setVolumeLabSideEffect.Volume;
        }
        
        public void Save()
        {
            _savedData.Volume = volume;
        }

        public void Load()
        {
            volume = _savedData.Volume;
            _text.text = volume.ToString("F4");
        }
    }
}
