using System;
using Core;

namespace SideEffects.SideEffectsEditor
{
    [Serializable]
    public abstract class EditorSideEffect
    {
        protected LabSideEffect labSideEffect { get; private set; }

        protected EditorSideEffect(LabSideEffect labSideEffect)
        {
            this.labSideEffect = labSideEffect;
        }

        public abstract void ShowInEditor();
        public abstract ESideEffect GetSideEffectType();
    }
}