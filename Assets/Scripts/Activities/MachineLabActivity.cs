using BioEngineerLab.Tasks.Activities;

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

        public override bool Equals(LabActivity labActivity)
        {
            if (labActivity is not MachineLabActivity handlerMachineActivity)
            {
                return false;
            }

            return MachineActivityType == handlerMachineActivity.MachineActivityType &
                   MachineType == handlerMachineActivity.MachineType;
        }
    }
}