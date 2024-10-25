namespace BioEngineerLab.Tasks.SideEffects
{
    public class Effect2LabSideEffect : LabSideEffect
    {
        public int SomeConfig2;

        public Effect2LabSideEffect()
            : base(ESideEffect.Effect2, ESideEffectTime.StartTask)
        {
        }

        public Effect2LabSideEffect(Effect2LabSideEffect effect2LabSideEffect)
            : base(ESideEffect.Effect2, effect2LabSideEffect.SideEffectTimeType)
        {
            SomeConfig2 = effect2LabSideEffect.SomeConfig2;
        }

        public Effect2LabSideEffect(int someConfig2, ESideEffectTime sideEffectTime)
            : base(ESideEffect.Effect2, sideEffectTime)
        {
            SomeConfig2 = someConfig2;
        }
    }
}