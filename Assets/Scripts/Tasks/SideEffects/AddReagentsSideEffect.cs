using System.Collections.Generic;
using BioEngineerLab.Core;
using BioEngineerLab.Substances;
using UnityEditor;
using Container = BioEngineerLab.Containers.Container;

namespace BioEngineerLab.Tasks.SideEffects
{
    public class AddReagentsSideEffect : SideEffect
    {
        private SubstanceProperty _reagentsSubstance;
        private float _weight;

        public AddReagentsSideEffect(float weight = 0f, SubstanceProperty reagentsSubstance = null)
            : base(ESideEffect.AddReagentsSideEffect, ESideEffectTime.StartTask)
        {
            _reagentsSubstance = reagentsSubstance;
            _weight = weight;
        }
        
        public override void ShowInEditor()
        {
            SideEffectTimeType = (ESideEffectTime)EditorGUILayout.EnumPopup("Side Effect Time", SideEffectTimeType);
            _reagentsSubstance = EditorGUILayout.ObjectField("Reagents Substance", _reagentsSubstance, typeof(SubstanceProperty), true) as SubstanceProperty;
            _weight = EditorGUILayout.FloatField("Weight Substance", _weight);
        }

        public override void OnActivated()
        {
            ContainerService containerService = Engine.GetService<ContainerService>();
            List<Container> reagentsContainer = containerService.GetReagentsContainer();
            
            foreach (var container in reagentsContainer)
            {
                container.PutSubstance(new Substance(_reagentsSubstance, _weight));
            }
        }
    }
}