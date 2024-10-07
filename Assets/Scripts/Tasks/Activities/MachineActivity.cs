using UnityEditor;

namespace BioEngineerLab.Activities
{
    public class MachineActivity : Activity
    {
        private MachineActivityType _machineActivityType;
        private MachineType _machineType;

        public MachineActivity(MachineActivityType machineActivityType = MachineActivityType.OnEnter, MachineType machineType = MachineType.CoatMachine)
            : base(EActivity.MachineActivity)
        {
            _machineActivityType = machineActivityType;
            _machineType = machineType;
        }
        
        public override void ShowInEditor()
        {
            _machineActivityType = (MachineActivityType)EditorGUILayout.EnumPopup("Machine Activity Type", _machineActivityType);
            _machineType = (MachineType)EditorGUILayout.EnumPopup("Machine Type", _machineType);
        }

        public override bool CompleteActivity(Activity activity)
        {
            if (activity is not MachineActivity machineActivity)
            {
                return false;
            }
            
            return _machineActivityType == machineActivity._machineActivityType & 
                   _machineType == machineActivity._machineType;
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