namespace BioEngineerLab.Tasks.SideEffects
{
    public class SpawnDocLabSideEffect : LabSideEffect
    {
        public EMachine MachineType;

        public SpawnDocLabSideEffect()
            : base(ESideEffect.SetDozatorVolumeSideEffect, ESideEffectTime.EndTask)
        {
        }

        public SpawnDocLabSideEffect(SpawnDocLabSideEffect sideEffect)
            : base(ESideEffect.SetDozatorVolumeSideEffect, sideEffect.SideEffectTimeType)
        {
            MachineType = sideEffect.MachineType;
        }

        public SpawnDocLabSideEffect(EMachine machine)
            : base(ESideEffect.SetDozatorVolumeSideEffect, ESideEffectTime.EndTask)
        {
            MachineType = machine;
        }
    }
}