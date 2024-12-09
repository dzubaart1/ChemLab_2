using System;
using BioEngineerLab.Activities;
using Core;

namespace Activities.ActivitiesEditor
{
    [Serializable]
    public abstract class EditorActivity
    {
#if UNITY_EDITOR
        protected LabActivity labActivity { get; private set; }

        protected EditorActivity(LabActivity labActivity)
        {
            this.labActivity = labActivity;
        }

        public abstract void ShowInEditor();
        public abstract EActivity GetActivityType();
#endif
    }
}
