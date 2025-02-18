using BioEngineerLab.Tasks.SideEffects;
using Core;

namespace BioEngineerLab.Tasks.SideEffects
{
    public class SetVolumeLabSideEffect : LabSideEffect
    {
        public float Volume;
        public EContainer Container;

        public SetVolumeLabSideEffect()
            : base(ESideEffect.SetVolumeSideEffect, ESideEffectTime.EndTask)
        {
            
        }

        public SetVolumeLabSideEffect(SetVolumeLabSideEffect sideEffect)
            : base(ESideEffect.SetVolumeSideEffect, sideEffect.SideEffectTimeType)
        {
            Volume = sideEffect.Volume;
            Container = sideEffect.Container;
        }

        public SetVolumeLabSideEffect(float volume, EContainer container)
            : base(ESideEffect.SetVolumeSideEffect, ESideEffectTime.EndTask)
        {
            Volume = volume;
            Container = container;
        }
    }
}