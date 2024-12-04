using BioEngineerLab.Activities;
using Core;
using JetBrains.Annotations;
using UnityEditor;

namespace Activities.ActivitiesEditor
{
    #if UNITY_EDITOR
    public class MachineLabActivityEditor : EditorActivity
    {
        [CanBeNull] private MachineLabActivity _machineLabActivity;
        
        public MachineLabActivityEditor(LabActivity labActivity)
            : base(labActivity)
        {
            if (labActivity is MachineLabActivity handler)
            {
                _machineLabActivity = handler;
            }
        }
        
        public override void ShowInEditor()
        {
            if (_machineLabActivity == null)
            {
                return;
            }
            
            _machineLabActivity.MachineActivityType = (EMachineActivity)EditorGUILayout.EnumPopup("Machine Activity", _machineLabActivity.MachineActivityType);
            _machineLabActivity.MachineType = (EMachine)EditorGUILayout.EnumPopup("Machine Type", _machineLabActivity.MachineType);
        }

        public override EActivity GetActivityType()
        {
            return EActivity.MachineActivity;
        }
    }
    #endif
}