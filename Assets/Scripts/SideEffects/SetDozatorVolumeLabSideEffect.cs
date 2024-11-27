using BioEngineerLab.Tasks.SideEffects;
using Core;

namespace BioEngineerLab.Tasks.SideEffects
{
    public class SetDozatorVolumeLabSideEffect : LabSideEffect
    {
        public float DozatorVolume;

        public SetDozatorVolumeLabSideEffect()
            : base(ESideEffect.SetDozatorVolumeSideEffect, ESideEffectTime.EndTask)
        {
            
        }

        public SetDozatorVolumeLabSideEffect(SetDozatorVolumeLabSideEffect sideEffect)
            : base(ESideEffect.SetDozatorVolumeSideEffect, sideEffect.SideEffectTimeType)
        {
            DozatorVolume = sideEffect.DozatorVolume;
        }

        public SetDozatorVolumeLabSideEffect(float dozatorVolume)
            : base(ESideEffect.SetDozatorVolumeSideEffect, ESideEffectTime.EndTask)
        {
            DozatorVolume = dozatorVolume;
        }
    }
}