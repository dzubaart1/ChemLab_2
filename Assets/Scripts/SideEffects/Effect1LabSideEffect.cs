namespace BioEngineerLab.Tasks.SideEffects
{
    public class Effect1LabSideEffect : LabSideEffect
    {
        public int SomeConfig;

        public Effect1LabSideEffect()
            : base(ESideEffect.Effect1, ESideEffectTime.StartTask)
        {
        }

        public Effect1LabSideEffect(Effect1LabSideEffect effect1LabSideEffect)
            : base(ESideEffect.Effect1, effect1LabSideEffect.SideEffectTimeType)
        {
            SomeConfig = effect1LabSideEffect.SomeConfig;
        }
        
        public Effect1LabSideEffect(int someConfig, ESideEffectTime sideEffectTime)
            : base(ESideEffect.Effect1, sideEffectTime)
        {
            SomeConfig = someConfig;
        }
    }
}