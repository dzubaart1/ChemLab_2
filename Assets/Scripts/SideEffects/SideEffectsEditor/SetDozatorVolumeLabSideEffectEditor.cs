using BioEngineerLab.Tasks;
using BioEngineerLab.Tasks.SideEffects;
using Core;
using JetBrains.Annotations;
using UnityEditor;

namespace BioEngineerLab.Tasks.SideEffects
{
    public class SetVolumeLabSideEffectEditor : EditorSideEffect
    {
#if UNITY_EDITOR
        [CanBeNull] private SetVolumeLabSideEffect _sideEffect;
        [CanBeNull] private SOLabSubstanceProperty _newSoLabSubstanceProperty;
        
        public SetVolumeLabSideEffectEditor(LabSideEffect labSideEffect)
            : base(labSideEffect)
        {
            if (labSideEffect is SetVolumeLabSideEffect handler)
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
            
            _sideEffect.Volume = EditorGUILayout.FloatField("Volume", _sideEffect.Volume);
            _sideEffect.Container = (EContainer)EditorGUILayout.EnumPopup("Container", _sideEffect.Container);
        }

        public override ESideEffect GetSideEffectType()
        {
            return ESideEffect.SetVolumeSideEffect;
        }
#endif
    }
}