using Core;
using JetBrains.Annotations;
using UnityEditor;

namespace BioEngineerLab.Tasks.SideEffects
{
    public class SetHintImgSideEffectEditor : EditorSideEffect
    {
#if UNITY_EDITOR
        [CanBeNull] private SetHintImgSideEffect _sideEffect;
        
        public SetHintImgSideEffectEditor(LabSideEffect labSideEffect)
            : base(labSideEffect)
        {
            if (labSideEffect is SetHintImgSideEffect handler)
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

            _sideEffect.HintImageFullName = EditorGUILayout.TextField("Hint Image SideEffect", _sideEffect.HintImageFullName);
        }

        public override ESideEffect GetSideEffectType()
        {
            return ESideEffect.SetHintImgSideEffect;
        }
#endif
    }
}