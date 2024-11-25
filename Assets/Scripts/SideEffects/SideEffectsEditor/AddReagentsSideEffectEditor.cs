using System.Linq;
using Core;
using Database;
using JetBrains.Annotations;
using Substances;
using UnityEditor;

namespace SideEffects.SideEffectsEditor
{
    public class AddReagentsSideEffectEditor : EditorSideEffect
    {
        [CanBeNull] private AddReagentsLabSideEffect _addReagentsLab;
        [CanBeNull] private SOLabSubstanceProperty _newSoLabSubstanceProperty;
        
        public AddReagentsSideEffectEditor(LabSideEffect labSideEffect)
            : base(labSideEffect)
        {
            if (labSideEffect is AddReagentsLabSideEffect handler)
            {
                _addReagentsLab = handler;
            }

            if (_addReagentsLab != null)
            {
                _newSoLabSubstanceProperty = ResourcesDatabase.ReadWhereSubstanceProperties(
                        substanceProperty => substanceProperty.LabSubstanceProperty.Equals(_addReagentsLab.LabSubstanceProperty)).FirstOrDefault();
            }
        }
        
        public override void ShowInEditor()
        {
            if (_addReagentsLab == null)
            {
                return;
            }
            
            _addReagentsLab.SideEffectTimeType = (ESideEffectTime)EditorGUILayout.EnumPopup("Side Effect Time", _addReagentsLab.SideEffectTimeType);
            _newSoLabSubstanceProperty = EditorGUILayout.ObjectField("Adding Substance", _newSoLabSubstanceProperty, typeof(SOLabSubstanceProperty), true) as SOLabSubstanceProperty;
            if (_newSoLabSubstanceProperty != null)
            {
                _addReagentsLab.LabSubstanceProperty = _newSoLabSubstanceProperty.LabSubstanceProperty;
            }
            _addReagentsLab.Weight = EditorGUILayout.FloatField("Weight Substance", _addReagentsLab.Weight);
        }

        public override ESideEffect GetSideEffectType()
        {
            return ESideEffect.AddReagentsSideEffect;
        }
    }
}