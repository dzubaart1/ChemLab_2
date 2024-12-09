using BioEngineerLab.Tasks;
using BioEngineerLab.Tasks.SideEffects;
using Core;
using JetBrains.Annotations;
using UnityEditor;

namespace BioEngineerLab.Tasks.SideEffects
{
    public class SetDozatorVolumeLabSideEffectEditor : EditorSideEffect
    {
#if UNITY_EDITOR
        [CanBeNull] private SetDozatorVolumeLabSideEffect _sideEffect;
        [CanBeNull] private SOLabSubstanceProperty _newSoLabSubstanceProperty;
        
        public SetDozatorVolumeLabSideEffectEditor(LabSideEffect labSideEffect)
            : base(labSideEffect)
        {
            if (labSideEffect is SetDozatorVolumeLabSideEffect handler)
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
            
            _sideEffect.DozatorVolume = EditorGUILayout.FloatField("Dozator Volume", _sideEffect.DozatorVolume);
        }

        public override ESideEffect GetSideEffectType()
        {
            return ESideEffect.SetDozatorVolumeSideEffect;
        }
#endif
    }
}