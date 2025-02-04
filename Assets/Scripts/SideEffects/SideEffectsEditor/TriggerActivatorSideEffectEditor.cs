using Core;
using JetBrains.Annotations;
using UnityEditor;

namespace BioEngineerLab.Tasks.SideEffects
{
    public class TriggerActivatorSideEffectEditor : EditorSideEffect
    {
#if UNITY_EDITOR
        [CanBeNull] private TriggerActivatorSideEffect _sideEffect;
        [CanBeNull] private SOLabSubstanceProperty _newSoLabSubstanceProperty;
        
        public TriggerActivatorSideEffectEditor(LabSideEffect labSideEffect)
            : base(labSideEffect)
        {
            if (labSideEffect is TriggerActivatorSideEffect handler)
            {
                _sideEffect = handler;
            }
        }
        
        public override void ShowInEditor()
        {
            if (_sideEffect == null)
            {
                return;
            }
            
            _sideEffect.SideEffectTimeType = (ESideEffectTime)EditorGUILayout.EnumPopup("Side Effect Time", _sideEffect.SideEffectTimeType);
            _sideEffect.IsActive = EditorGUILayout.Toggle("Is Active", _sideEffect.IsActive);
        }

        public override ESideEffect GetSideEffectType()
        {
            return ESideEffect.TriggerActivatorSideEffect;
        }
#endif
    }
}