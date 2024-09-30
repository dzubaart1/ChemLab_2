using System;

namespace BioEngineerLab.Activities
{
    public class MachineActivity : Activity
    {
        public MachineActivityType MachineActivityType;
        public MachineType MachineType;

        public MachineActivity()
        {
            ActivityType = ActivityType.MachineActivity;
        }
        
        public MachineActivity(MachineActivityType activityType, MachineType machineType)
        {
            ActivityType = ActivityType.MachineActivity;
            MachineActivityType = activityType;
            MachineType = machineType;
        }

        public override bool EqualsActivity(Activity activity)
        {
            if (activity is not MachineActivity machineActivity)
            {
                return false;
            }
            
            return MachineActivityType == machineActivity.MachineActivityType & 
                   MachineType == machineActivity.MachineType;
        }
    }
    
    public enum MachineActivityType : byte
    {
        OnEnter,
        OnStart,
        OnFinish,
        OnExit,
    }

    public enum MachineType : byte
    {
        StirringMachine,
        HandModelChangerMachine,
        CoatMachine,
        SyringeCupMoveMachine
    }
}