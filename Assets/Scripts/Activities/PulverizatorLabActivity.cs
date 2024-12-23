using System;
using Core;

namespace BioEngineerLab.Activities
{
    public class PulverizatorLabActivity : LabActivity
    {
        public EPulverizatorHits Hits;

        public PulverizatorLabActivity()
            : base(EActivity.PulveriazatorActivity)
        {
        }

        public PulverizatorLabActivity(PulverizatorLabActivity pulverizatorLabAcitivity)
            : base(EActivity.PulveriazatorActivity)
        {
            Hits = pulverizatorLabAcitivity.Hits;
        }
        
        public PulverizatorLabActivity(EPulverizatorHits hits)
            : base(EActivity.PulveriazatorActivity)
        {
            Hits = hits;
        }

        public override bool Equals(Object obj)
        {
            if (obj is not PulverizatorLabActivity pulverizatorLabAcitivity)
            {
                return false;
            }

            return Hits == pulverizatorLabAcitivity.Hits;
        }

        public override int GetHashCode()
        {
            return (int)Hits;
        }
    }
}