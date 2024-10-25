using System;
using System.Collections.Generic;
using BioEngineerLab.Activities;
using BioEngineerLab.Tasks.Activities;
using BioEngineerLab.Tasks.SideEffects;
using Database;

namespace BioEngineerLab.Tasks
{
    [Serializable]
    public class LabTask : ILabSerializable
    {
        public int Number;
        public string Title;
        public string Description;
        public string Warning;
        public bool SaveableTask;

        public LabActivity LabActivity = new AnchorLabActivity();
        public List<LabSideEffect> LabSideEffects = new List<LabSideEffect>();
    }
}