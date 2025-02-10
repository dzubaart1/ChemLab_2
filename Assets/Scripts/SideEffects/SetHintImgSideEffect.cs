using System;
using BioEngineerLab.Tasks.SideEffects;
using Core;
using UnityEngine;

namespace BioEngineerLab.Tasks.SideEffects
{
    public class SetHintImgSideEffect : LabSideEffect
    {
        public string HintImageFullName;

        public SetHintImgSideEffect()
            : base(ESideEffect.SetHintImgSideEffect, ESideEffectTime.StartTask)
        {
            
        }

        public SetHintImgSideEffect(SetHintImgSideEffect sideEffect)
            : base(ESideEffect.SetHintImgSideEffect, sideEffect.SideEffectTimeType)
        {
            HintImageFullName = sideEffect.HintImageFullName;
        }

        public SetHintImgSideEffect(String hintImageFullName)
            : base(ESideEffect.SetHintImgSideEffect, ESideEffectTime.StartTask)
        {
            HintImageFullName = hintImageFullName;
        }
    }
}