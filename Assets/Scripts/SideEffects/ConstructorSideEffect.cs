using BioEngineerLab.Tasks.SideEffects;
using Core;

namespace BioEngineerLab.Tasks.SideEffects
{
    public class ConstructorSideEffect : LabSideEffect
    {
        public ESocket SocketType;
        public bool IsLock;

        public ConstructorSideEffect()
            : base(ESideEffect.ConstructorSideEffect, ESideEffectTime.EndTask)
        {
            
        }

        public ConstructorSideEffect(ConstructorSideEffect sideEffect)
            : base(ESideEffect.ConstructorSideEffect, sideEffect.SideEffectTimeType)
        {
            SocketType = sideEffect.SocketType;
            IsLock = sideEffect.IsLock;
        }

        public ConstructorSideEffect(ESocket socketType, bool isLock)
            : base(ESideEffect.ConstructorSideEffect, ESideEffectTime.EndTask)
        {
            SocketType = socketType;
            IsLock = isLock;
        }
    }
}