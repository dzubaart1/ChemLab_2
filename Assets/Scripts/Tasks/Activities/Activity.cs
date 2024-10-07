using System;

namespace BioEngineerLab.Activities
{
    [Serializable]
    public abstract class Activity
    {
        public EActivity ActivityType { get; private set; }

        protected Activity(EActivity activityType)
        {
            ActivityType = activityType;
        }

        public abstract void ShowInEditor();
        public abstract bool CompleteActivity(Activity activity);
    }
}
