using System;
using Core;

namespace BioEngineerLab.Activities
{
    public class MachineLabActivity : LabActivity
    {
        public EMachineActivity MachineActivityType;
        public EMachine MachineType;

        public MachineLabActivity()
            : base(EActivity.MachineActivity)
        {
        }

        public MachineLabActivity(MachineLabActivity machineLabActivity)
            : base(EActivity.MachineActivity)
        {
            MachineActivityType = machineLabActivity.MachineActivityType;
            MachineType = machineLabActivity.MachineType;
        }
        
        public MachineLabActivity(EMachineActivity machineActivityType, EMachine machineType)
            : base(EActivity.MachineActivity)
        {
            MachineActivityType = machineActivityType;
            MachineType = machineType;
        }

        public override bool Equals(Object obj)
        {
            if (obj is not MachineLabActivity handlerMachineActivity)
            {
                return false;
            }

            return MachineActivityType == handlerMachineActivity.MachineActivityType &
                   MachineType == handlerMachineActivity.MachineType;
        }

        public override int GetHashCode()
        {
            return (int)MachineActivityType + (int)MachineType;
        }
    }
}