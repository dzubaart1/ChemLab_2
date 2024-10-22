using System.Collections.Generic;
using BioEngineerLab.Containers;
using BioEngineerLab.Core;
using BioEngineerLab.Substances;
using UnityEditor;

namespace BioEngineerLab.Tasks.SideEffects
{
    public class AddReagentsSideEffect : SideEffect
    {
        public ESubstanceName SubstanceName;
        public ESubstanceMode SubstanceMode;
        public float Weight;

        public AddReagentsSideEffect(float weight = 0f, ESubstanceName substanceName = ESubstanceName.Bad, ESubstanceMode substanceMode = ESubstanceMode.Dry)
            : base(ESideEffect.AddReagentsSideEffect, ESideEffectTime.StartTask)
        {
            SubstanceName = substanceName;
            SubstanceMode = substanceMode;
            Weight = weight;
        }
        
        public override void ShowInEditor()
        {
            SubstanceName = (ESubstanceName)EditorGUILayout.EnumPopup("Adding Substance Name", SubstanceName);
            SubstanceMode = (ESubstanceMode)EditorGUILayout.EnumPopup("Adding Substance Mode", SubstanceMode);
            SideEffectTimeType = (ESideEffectTime)EditorGUILayout.EnumPopup("Side Effect Time", SideEffectTimeType);
            Weight = EditorGUILayout.FloatField("Weight Substance", Weight);
        }

        public override void OnActivated()
        {
            CraftService craftService = Engine.GetService<CraftService>();
            ContainerService containerService = Engine.GetService<ContainerService>();
            List<Container> reagentsContainer = containerService.GetReagentsContainer();

            SubstanceProperty substanceProperty = craftService.GetSubstanceProperty(SubstanceName, SubstanceMode);
            
            foreach (var container in reagentsContainer)
            {
                if (container.ReagentsProperty == substanceProperty)
                {
                    container.PutSubstance(new Substance(substanceProperty, Weight));   
                }
            }
        }
    }
}