using System;
using Core;

namespace BioEngineerLab.Tasks.SideEffects
{
    [Serializable]
    public class LabSideEffect
    {
        public ESideEffectTime SideEffectTimeType { get; set; }
        
        public ESideEffect SideEffectType { get; private set; }

        public LabSideEffect(ESideEffect sideEffectType, ESideEffectTime sideEffectTimeType)
        {
            SideEffectType = sideEffectType;
            SideEffectTimeType = sideEffectTimeType;
        }
    }
}