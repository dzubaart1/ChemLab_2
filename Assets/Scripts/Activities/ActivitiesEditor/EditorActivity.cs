using System;

namespace BioEngineerLab.Tasks.Activities
{
    [Serializable]
    public abstract class EditorActivity
    {
        protected LabActivity labActivity { get; private set; }

        protected EditorActivity(LabActivity labActivity)
        {
            this.labActivity = labActivity;
        }

        public abstract void ShowInEditor();
        public abstract EActivity GetActivityType();
    }
}
