using System;
using BioEngineerLab.Tasks.SideEffects;
using Core;

namespace BioEngineerLab.Tasks.SideEffects
{
    public class WarningTextLabSideEffect : LabSideEffect
    {
        public String WarningText;
        public bool IsActive;

        public WarningTextLabSideEffect()
            : base(ESideEffect.WarningTextSideEffect, ESideEffectTime.StartTask)
        {
            
        }

        public WarningTextLabSideEffect(WarningTextLabSideEffect sideEffect)
            : base(ESideEffect.WarningTextSideEffect, sideEffect.SideEffectTimeType)
        {
            WarningText = sideEffect.WarningText;
            IsActive = sideEffect.IsActive;
        }

        public WarningTextLabSideEffect(String warningText, bool isActive)
            : base(ESideEffect.WarningTextSideEffect, ESideEffectTime.StartTask)
        {
            WarningText = warningText;
            IsActive = isActive;
        }
    }
}