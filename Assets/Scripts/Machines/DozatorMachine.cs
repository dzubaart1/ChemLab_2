using BioEngineerLab.Tasks.SideEffects;
using Containers;
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

        public void OnActivateSideEffect(LabSideEffect sideEffect)
        {
            if (sideEffect is not SetDozatorVolumeLabSideEffect setDozatorVolumeLabSideEffect)
            {
                return;
            }

            if (setDozatorVolumeLabSideEffect.DozatorVolume >= 1)
            {
                _text.text = setDozatorVolumeLabSideEffect.DozatorVolume + ".00";
            }
            else
            {
                _text.text = setDozatorVolumeLabSideEffect.DozatorVolume + "";
            }
            
            _labContainer.ChangeMaxVolume(setDozatorVolumeLabSideEffect.DozatorVolume);
        }
    }
}
