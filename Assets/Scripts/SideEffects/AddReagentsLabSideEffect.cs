using Core;
using Substances;

namespace SideEffects
{
    public class AddReagentsLabSideEffect : LabSideEffect
    {
        public LabSubstanceProperty LabSubstanceProperty;
        public float Weight;

        public AddReagentsLabSideEffect()
            : base(ESideEffect.AddReagentsSideEffect, ESideEffectTime.StartTask)
        {
            
        }

        public AddReagentsLabSideEffect(AddReagentsLabSideEffect sideEffect)
            : base(ESideEffect.AddReagentsSideEffect, sideEffect.SideEffectTimeType)
        {
            LabSubstanceProperty = new LabSubstanceProperty(sideEffect.LabSubstanceProperty);
            Weight = sideEffect.Weight;
        }

        public AddReagentsLabSideEffect(float weight, LabSubstanceProperty labSubstanceProperty, ESideEffectTime sideEffectTime)
            : base(ESideEffect.AddReagentsSideEffect, sideEffectTime)
        {
            LabSubstanceProperty = labSubstanceProperty;
            Weight = weight;
        }
    }
}