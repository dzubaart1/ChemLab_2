using Core;
using JetBrains.Annotations;
using UnityEditor;

namespace BioEngineerLab.Tasks.SideEffects
{
    public class ConstructorLabSideEffectEditor : EditorSideEffect
    {
#if UNITY_EDITOR
        [CanBeNull] private ConstructorSideEffect _sideEffect;
        [CanBeNull] private SOLabSubstanceProperty _newSoLabSubstanceProperty;
        
        public ConstructorLabSideEffectEditor(LabSideEffect labSideEffect)
            : base(labSideEffect)
        {
            if (labSideEffect is ConstructorSideEffect handler)
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
            _sideEffect.SocketType = (ESocket)EditorGUILayout.EnumPopup("Socket Type", _sideEffect.SocketType);
            _sideEffect.IsLock = EditorGUILayout.Toggle("Is Lock", _sideEffect.IsLock);
        }

        public override ESideEffect GetSideEffectType()
        {
            return ESideEffect.ConstructorSideEffect;
        }
#endif
    }
}