using System;

namespace BioEngineerLab.Tasks.SideEffects
{
    [Serializable]
    public abstract class SideEffect
    {
        public ESideEffect SideEffectType { get; private set; }
        public ESideEffectTime SideEffectTimeType { get; protected set; }

        protected SideEffect(ESideEffect sideEffectType, ESideEffectTime sideEffectTimeType)
        {
            SideEffectType = sideEffectType;
            SideEffectTimeType = sideEffectTimeType;
        }

        public abstract void ShowInEditor();
        public abstract void OnActivated();
    }
}