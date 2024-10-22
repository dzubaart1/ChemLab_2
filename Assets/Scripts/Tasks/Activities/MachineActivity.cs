using BioEngineerLab.Tasks.Activities;
using UnityEditor;

namespace BioEngineerLab.Activities
{
    public class MachineActivity : Activity
    {
        public MachineActivityType MachineActivityType;
        public MachineType MachineType;

        public MachineActivity(MachineActivityType machineActivityType = MachineActivityType.OnEnter, MachineType machineType = MachineType.CoatMachine)
            : base(EActivity.MachineActivity)
        {
            MachineActivityType = machineActivityType;
            MachineType = machineType;
        }
        
        public override void ShowInEditor()
        {
            MachineActivityType = (MachineActivityType)EditorGUILayout.EnumPopup("Machine Activity Type", MachineActivityType);
            MachineType = (MachineType)EditorGUILayout.EnumPopup("Machine Type", MachineType);
        }

        public override bool CompleteActivity(Activity activity)
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