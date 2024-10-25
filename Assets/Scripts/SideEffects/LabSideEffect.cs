using System;

namespace BioEngineerLab.Tasks.SideEffects
{
    [Serializable]
    public abstract class LabSideEffect
    {
        public ESideEffectTime SideEffectTimeType { get; set; }
        
        public ESideEffect SideEffectType { get; private set; }

        protected LabSideEffect(ESideEffect sideEffectType, ESideEffectTime sideEffectTimeType)
        {
            SideEffectType = sideEffectType;
            SideEffectTimeType = sideEffectTimeType;
        }
    }
}