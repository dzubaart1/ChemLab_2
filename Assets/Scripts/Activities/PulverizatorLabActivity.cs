using System;
using Core;

namespace BioEngineerLab.Activities
{
    public class PulverizatorLabActivity : LabActivity
    {
        public EPulverizatorTarget Target;

        public PulverizatorLabActivity()
            : base(EActivity.PulveriazatorActivity)
        {
        }

        public PulverizatorLabActivity(PulverizatorLabActivity pulverizatorLabAcitivity)
            : base(EActivity.PulveriazatorActivity)
        {
            Target = pulverizatorLabAcitivity.Target;
        }
        
        public PulverizatorLabActivity(EPulverizatorTarget target)
            : base(EActivity.PulveriazatorActivity)
        {
            Target = target;
        }

        public override bool Equals(Object obj)
        {
            if (obj is not PulverizatorLabActivity pulverizatorLabAcitivity)
            {
                return false;
            }

            return Target == pulverizatorLabAcitivity.Target;
        }

        public override int GetHashCode()
        {
            return (int)Target;
        }
    }
}