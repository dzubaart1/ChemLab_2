using BioEngineerLab.Tasks.SideEffects;
using Core;

namespace BioEngineerLab.Tasks.SideEffects
{
    public class TriggerActivatorSideEffect : LabSideEffect
    {
        public bool IsActive;

        public TriggerActivatorSideEffect()
            : base(ESideEffect.TriggerActivatorSideEffect, ESideEffectTime.EndTask)
        {
            
        }

        public TriggerActivatorSideEffect(TriggerActivatorSideEffect sideEffect)
            : base(ESideEffect.TriggerActivatorSideEffect, sideEffect.SideEffectTimeType)
        {
            IsActive = sideEffect.IsActive;
        }

        public TriggerActivatorSideEffect(bool isActive)
            : base(ESideEffect.TriggerActivatorSideEffect, ESideEffectTime.EndTask)
        {
            IsActive = isActive;
        }
    }
}