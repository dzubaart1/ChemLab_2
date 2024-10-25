using JetBrains.Annotations;
using UnityEditor;

namespace BioEngineerLab.Tasks.SideEffects
{
    public class Effect2Editor : EditorSideEffect
    {
        [CanBeNull] private Effect2LabSideEffect _effect2LabSideEffect;
        
        public Effect2Editor(LabSideEffect labSideEffect)
            : base(labSideEffect)
        {
            if (labSideEffect is Effect2LabSideEffect handler)
            {
                _effect2LabSideEffect = handler;
            }
        }
        
        public override void ShowInEditor()
        {
            if (_effect2LabSideEffect == null)
            {
                return;
            }
            
            _effect2LabSideEffect.SideEffectTimeType = (ESideEffectTime)EditorGUILayout.EnumPopup("Side Effect Time", _effect2LabSideEffect.SideEffectTimeType);
            _effect2LabSideEffect.SomeConfig2 = (int)EditorGUILayout.IntField("Some Config 2", _effect2LabSideEffect.SomeConfig2);
        }

        public override ESideEffect GetSideEffectType()
        {
            return ESideEffect.Effect2;
        }
    }
}