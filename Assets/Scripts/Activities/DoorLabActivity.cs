using System;
using Core;

namespace BioEngineerLab.Activities
{
    public class DoorLabActivity : LabActivity
    {
        public EDoor Door;
        public EDoorActivity DoorActivity;

        public DoorLabActivity()
            : base(EActivity.DoorActivity)
        {
        }

        public DoorLabActivity(DoorLabActivity doorLabActivity)
            : base(EActivity.DoorActivity)
        {
            Door = doorLabActivity.Door;
            DoorActivity = doorLabActivity.DoorActivity;
        }
        
        public DoorLabActivity(EDoor door, EDoorActivity doorActivity)
            : base(EActivity.DoorActivity)
        {
            Door = door;
            DoorActivity = doorActivity;
        }

        public override bool Equals(Object obj)
        {
            if (obj is not DoorLabActivity doorLabActivity)
            {
                return false;
            }

            return Door == doorLabActivity.Door &
                   DoorActivity == doorLabActivity.DoorActivity;
        }

        public override int GetHashCode()
        {
            return (int)DoorActivity + (int)Door;
        }
    }
}