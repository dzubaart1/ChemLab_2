using Core;
using JetBrains.Annotations;
using UnityEditor;

namespace SideEffects.SideEffectsEditor
{
    public class SpawnDocSideEffectEditor : EditorSideEffect
    {
        [CanBeNull] private SpawnDocLabSideEffect _sideEffect;
        
        public SpawnDocSideEffectEditor(LabSideEffect labSideEffect)
            : base(labSideEffect)
        {
            if (labSideEffect is SpawnDocLabSideEffect handler)
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
            
            _sideEffect.MachineType = (EMachine)EditorGUILayout.EnumPopup("Machine Type", _sideEffect.MachineType);
        }

        public override ESideEffect GetSideEffectType()
        {
            return ESideEffect.SpawnDocSideEffect;
        }
    }
}