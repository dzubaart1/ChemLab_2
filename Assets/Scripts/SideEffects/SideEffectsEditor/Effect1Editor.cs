using JetBrains.Annotations;
using UnityEditor;

namespace BioEngineerLab.Tasks.SideEffects
{
    public class Effect1Editor : EditorSideEffect
    {
        [CanBeNull] private Effect1LabSideEffect _effect1LabSideEffect;
        
        public Effect1Editor(LabSideEffect labSideEffect)
            : base(labSideEffect)
        {
            if (labSideEffect is Effect1LabSideEffect handler)
            {
                _effect1LabSideEffect = handler;
            }
        }
        
        public override void ShowInEditor()
        {
            if (_effect1LabSideEffect == null)
            {
                return;
            }
            
            _effect1LabSideEffect.SideEffectTimeType = (ESideEffectTime)EditorGUILayout.EnumPopup("Side Effect Time", _effect1LabSideEffect.SideEffectTimeType);
            _effect1LabSideEffect.SomeConfig = (int)EditorGUILayout.IntField("Some Config", _effect1LabSideEffect.SomeConfig);
        }

        public override ESideEffect GetSideEffectType()
        {
            return ESideEffect.Effect1;
        }
    }
}