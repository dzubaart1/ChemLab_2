using BioEngineerLab.Tasks.SideEffects;
using Core;

namespace BioEngineerLab.Tasks.SideEffects
{
    public class SetInteractableSideEffect : LabSideEffect
    {
        public EInteractable InteractableObject;
        public bool IsInteractable;

        public SetInteractableSideEffect()
            : base(ESideEffect.SetInteractableSideEffect, ESideEffectTime.EndTask)
        {
            
        }

        public SetInteractableSideEffect(SetInteractableSideEffect sideEffect)
            : base(ESideEffect.SetInteractableSideEffect, sideEffect.SideEffectTimeType)
        {
            InteractableObject = sideEffect.InteractableObject;
            IsInteractable = sideEffect.IsInteractable;
        }

        public SetInteractableSideEffect(EInteractable eInteractable, bool isInteractable)
            : base(ESideEffect.SetInteractableSideEffect, ESideEffectTime.EndTask)
        {
            InteractableObject = eInteractable;
            IsInteractable = isInteractable;
        }
    }
}