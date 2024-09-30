using System;
using BioEngineerLab.Activities;
using BioEngineerLab.Gameplay;
using BioEngineerLab.Substances;
using UnityEngine;

namespace BioEngineerLab.Tasks
{
    [Serializable]
    public class TaskProperty
    {
        public int Number;
        public string Title;
        public string Description;
        public string Warning;
        public bool SaveableTask;

        public bool UnlockSyringe;

        public bool IsTaskChangeSyringePos;
        public SyringeCupMove.SyringePos SyringePos;

        public bool ShouldChangeGateOpening;
        public bool IsOpenGate;

        public bool IsTaskChangeSprite;
        public string SpriteName;
        [NonSerialized]
        public Sprite Sprite;
        
        public bool HasHintSprite;
        public string HintSpriteName;
        [NonSerialized]
        public Sprite HintSprite;
        
        public bool IsSubstanceAdding;
        public SubstanceName SubstanceName;
        public float SubstanceWeight;
        
        public ActivityType ActivityType;
        public Activity TaskActivity;
    }
}
