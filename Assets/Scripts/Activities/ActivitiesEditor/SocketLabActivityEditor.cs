using BioEngineerLab;
using BioEngineerLab.Activities;
using BioEngineerLab.Tasks.Activities;
using JetBrains.Annotations;
using UnityEditor;

namespace Activities.ActivitiesEditor
{
    public class SocketLabActivityEditor : EditorActivity
    {
        [CanBeNull] private SocketLabActivity _socketLabActivity;
        
        public SocketLabActivityEditor(LabActivity labActivity)
            : base(labActivity)
        {
            if (labActivity is SocketLabActivity handler)
            {
                _socketLabActivity = handler;
            }
        }
        
        public override void ShowInEditor()
        {
            if (_socketLabActivity == null)
            {
                return;
            }
            
            _socketLabActivity.SocketActivityType = (ESocketActivity)EditorGUILayout.EnumPopup("Socket Activity Type", _socketLabActivity.SocketActivityType);
            _socketLabActivity.SocketType = (ESocket)EditorGUILayout.EnumPopup("Scoket Type", _socketLabActivity.SocketType);
        }

        public override EActivity GetActivityType()
        {
            return EActivity.AddSubstanceActivity;
        }
    }
}