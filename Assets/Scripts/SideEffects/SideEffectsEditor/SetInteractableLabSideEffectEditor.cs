using BioEngineerLab.Tasks;
using BioEngineerLab.Tasks.SideEffects;
using Core;
using JetBrains.Annotations;
using UnityEditor;

namespace BioEngineerLab.Tasks.SideEffects
{
    public class SetInteractableLabSideEffectEditor : EditorSideEffect
    {
#if UNITY_EDITOR
        [CanBeNull] private SetInteractableSideEffect _sideEffect;
        [CanBeNull] private SOLabSubstanceProperty _newSoLabSubstanceProperty;
        
        public SetInteractableLabSideEffectEditor(LabSideEffect labSideEffect)
            : base(labSideEffect)
        {
            if (labSideEffect is SetInteractableSideEffect handler)
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
            _sideEffect.InteractableObject = (EInteractable)EditorGUILayout.EnumPopup("Interactable Object", _sideEffect.InteractableObject);
            _sideEffect.IsInteractable = EditorGUILayout.Toggle("Is Interactable", _sideEffect.IsInteractable);
        }

        public override ESideEffect GetSideEffectType()
        {
            return ESideEffect.SetInteractableSideEffect;
        }
#endif
    }
}