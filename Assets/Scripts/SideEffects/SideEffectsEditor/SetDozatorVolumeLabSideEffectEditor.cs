using System.Linq;
using BioEngineerLab.Substances;
using Database;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor;

namespace BioEngineerLab.Tasks.SideEffects
{
    public class SetDozatorVolumeLabSideEffectEditor : EditorSideEffect
    {
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
    }
}