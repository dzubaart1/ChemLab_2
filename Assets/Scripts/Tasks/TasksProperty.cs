using System;
using BioEngineerLab.Activities;
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
        
        public ActivityConfig ActivityConfig = new ActivityConfig();
        
        public SideEffectConfig[] SideEffectConfigs = Array.Empty<SideEffectConfig>();
    }

    [Serializable]
    public class ActivityConfig
    {
        public EActivity ActivityType;
        public Activity Activity = new AnchorActivity();
    }

    [Serializable]
    public class SideEffectConfig
    {
        public ESideEffect SideEffectType;
        public SideEffect SideEffect = new Effect1();
    }
}
