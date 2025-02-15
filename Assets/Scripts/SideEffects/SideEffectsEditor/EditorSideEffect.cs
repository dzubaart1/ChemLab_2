﻿using System;
using BioEngineerLab.Tasks.SideEffects;
using Core;

namespace BioEngineerLab.Tasks.SideEffects
{
    [Serializable]
    public abstract class EditorSideEffect
    {
        #if UNITY_EDITOR
        protected LabSideEffect labSideEffect { get; private set; }

        protected EditorSideEffect(LabSideEffect labSideEffect)
        {
            this.labSideEffect = labSideEffect;
        }

        public abstract void ShowInEditor();
        public abstract ESideEffect GetSideEffectType();
        #endif
    }
}