using BioEngineerLab.Activities;
using Core;
using JetBrains.Annotations;
using UnityEditor;

namespace Activities.ActivitiesEditor
{
    public class DoorLabActivityEditor : EditorActivity
    {
#if UNITY_EDITOR
        [CanBeNull] private DoorLabActivity _doorLabActivity;
        
        public DoorLabActivityEditor(LabActivity labActivity)
            : base(labActivity)
        {
            if (labActivity is DoorLabActivity handler)
            {
                _doorLabActivity = handler;
            }
        }
        
        public override void ShowInEditor()
        {
            if (_doorLabActivity == null)
            {
                return;
            }
            
            _doorLabActivity.Door = (EDoor)EditorGUILayout.EnumPopup("Door Type", _doorLabActivity.Door);
            _doorLabActivity.DoorActivity = (EDoorActivity)EditorGUILayout.EnumPopup("Door Activity", _doorLabActivity.DoorActivity);
        }

        public override EActivity GetActivityType()
        {
            return EActivity.DoorActivity;
        }
#endif
    }
}