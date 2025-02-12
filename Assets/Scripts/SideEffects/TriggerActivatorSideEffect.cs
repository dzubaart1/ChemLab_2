using BioEngineerLab.Tasks.SideEffects;
using Core;

namespace BioEngineerLab.Tasks.SideEffects
{
    public class TriggerActivatorSideEffect : LabSideEffect
    {
        public bool IsActive;
        public ETriggerType TriggerType;

        public TriggerActivatorSideEffect()
            : base(ESideEffect.TriggerActivatorSideEffect, ESideEffectTime.EndTask)
        {
            
        }

        public TriggerActivatorSideEffect(TriggerActivatorSideEffect sideEffect)
            : base(ESideEffect.TriggerActivatorSideEffect, sideEffect.SideEffectTimeType)
        {
            IsActive = sideEffect.IsActive;
            TriggerType = sideEffect.TriggerType;
        }

        public TriggerActivatorSideEffect(bool isActive, ETriggerType triggerType)
            : base(ESideEffect.TriggerActivatorSideEffect, ESideEffectTime.EndTask)
        {
            IsActive = isActive;
            TriggerType = triggerType;
        }
    }
}