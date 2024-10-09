using System.Collections.Generic;
using BioEngineerLab.Core;
using BioEngineerLab.Substances;
using UnityEditor;
using Container = BioEngineerLab.Containers.Container;

namespace BioEngineerLab.Tasks.SideEffects
{
    public class AddReagentsSideEffect : SideEffect
    {
        public SubstanceProperty ReagentsSubstance { get; private set; }
        public float Weight { get; private set; }

        public AddReagentsSideEffect(float weight = 0f, SubstanceProperty reagentsSubstance = null)
            : base(ESideEffect.AddReagentsSideEffect, ESideEffectTime.StartTask)
        {
            ReagentsSubstance = reagentsSubstance;
            Weight = weight;
        }
        
        public override void ShowInEditor()
        {
            SideEffectTimeType = (ESideEffectTime)EditorGUILayout.EnumPopup("Side Effect Time", SideEffectTimeType);
            ReagentsSubstance = EditorGUILayout.ObjectField("Reagents Substance", ReagentsSubstance, typeof(SubstanceProperty), true) as SubstanceProperty;
            Weight = EditorGUILayout.FloatField("Weight Substance", Weight);
        }

        public override void OnActivated()
        {
            ContainerService containerService = Engine.GetService<ContainerService>();
            List<Container> reagentsContainer = containerService.GetReagentsContainer();
            
            foreach (var container in reagentsContainer)
            {
                container.PutSubstance(new Substance(ReagentsSubstance, Weight));
            }
        }
    }
}