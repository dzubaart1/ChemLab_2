using Core;

namespace SideEffects
{
    public class SpawnDocLabSideEffect : LabSideEffect
    {
        public EMachine MachineType;

        public SpawnDocLabSideEffect()
            : base(ESideEffect.SpawnDocSideEffect, ESideEffectTime.EndTask)
        {
        }

        public SpawnDocLabSideEffect(SpawnDocLabSideEffect sideEffect)
            : base(ESideEffect.SpawnDocSideEffect, sideEffect.SideEffectTimeType)
        {
            MachineType = sideEffect.MachineType;
        }

        public SpawnDocLabSideEffect(EMachine machine)
            : base(ESideEffect.SpawnDocSideEffect, ESideEffectTime.EndTask)
        {
            MachineType = machine;
        }
    }
}