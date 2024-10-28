using System;
using System.Collections.Generic;
using BioEngineerLab.Activities;
using BioEngineerLab.Tasks.Activities;
using BioEngineerLab.Tasks.SideEffects;

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

        public ActivityConfig ActivityConfig;
        
        public List<SideEffectConfig> SideEffectConfigs = new List<SideEffectConfig>();
    }

    [Serializable]
    public class ActivityConfig
    {
        public EActivity ActivityType = EActivity.AnchorActivity;
        public Activity Activity = new AnchorActivity();
    }

    [Serializable]
    public class SideEffectConfig
    {
        public ESideEffect SideEffectType = ESideEffect.AddReagentsSideEffect;
        public SideEffect SideEffect = new AddReagentsSideEffect();
    }
}
