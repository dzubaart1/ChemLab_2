using System;
using Core;

namespace BioEngineerLab.Activities
{
    [Serializable]
    public abstract class LabActivity
    {
        public EActivity ActivityType { get; private set; }
        
        protected LabActivity(EActivity activityType)
        {
            ActivityType = activityType;
        }

        public abstract override bool Equals(Object obj);
        public abstract override int GetHashCode();
    }
}
