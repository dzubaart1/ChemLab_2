using BioEngineerLab.Tasks.SideEffects;
using Containers;
using Core;
using UnityEngine;
using TMPro;

namespace BioEngineerLab.Machines
{
    public class DozatorMachine : MonoBehaviour, ISideEffectActivator
    {
        [Header("UIs")]
        [SerializeField] private TextMeshProUGUI _text;
        
        [Space]
        [Header("Refs")]
        [SerializeField] private LabContainer _labContainer;
        
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
        }
    }
}
