using Core;

namespace BioEngineerLab.Activities
{
    public class BadLabActivity : LabActivity
    {
        public BadLabActivity() : base(EActivity.BadActivity)
        {
        }

        public override bool Equals(object obj)
        {
            return false;
        }

        public override int GetHashCode()
        {
            return 0;
        }
    }
}