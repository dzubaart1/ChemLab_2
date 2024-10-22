using System;

namespace BioEngineerLab.Tasks.Activities
{
    [Serializable]
    public class Activity
    {
        public EActivity ActivityType { get; private set; }

        protected Activity(EActivity activityType)
        {
            ActivityType = activityType;
        }

        public virtual void ShowInEditor(){}

        public virtual bool CompleteActivity(Activity activity)
        {
            return true;
        }
    }
}
