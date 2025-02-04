using BioEngineerLab.Tasks;
using BioEngineerLab.Tasks.SideEffects;
using Core;
using JetBrains.Annotations;
using UnityEditor;

namespace BioEngineerLab.Tasks.SideEffects
{
    public class WarningTextLabSideEffectEditor : EditorSideEffect
    {
#if UNITY_EDITOR
        [CanBeNull] private WarningTextLabSideEffect _sideEffect;
        
        public WarningTextLabSideEffectEditor(LabSideEffect labSideEffect)
            : base(labSideEffect)
        {
            if (labSideEffect is WarningTextLabSideEffect handler)
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
            
            _sideEffect.WarningText = EditorGUILayout.TextField("Warning Text", _sideEffect.WarningText);
            _sideEffect.IsActive = EditorGUILayout.Toggle("Is Active", _sideEffect.IsActive);
        }

        public override ESideEffect GetSideEffectType()
        {
            return ESideEffect.WarningTextSideEffect;
        }
#endif
    }
}