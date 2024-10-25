using System;
using Database;

namespace BioEngineerLab.Tasks.Activities
{
    [Serializable]
    public abstract class LabActivity : ILabSerializable
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
