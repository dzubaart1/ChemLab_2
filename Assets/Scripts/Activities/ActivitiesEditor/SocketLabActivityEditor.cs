using BioEngineerLab.Activities;
using Core;
using JetBrains.Annotations;
using UnityEditor;

namespace Activities.ActivitiesEditor
{
    public class SocketLabActivityEditor : EditorActivity
    {
#if UNITY_EDITOR
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
            
            _socketLabActivity.SocketType = (ESocket)EditorGUILayout.EnumPopup("Socket Type", _socketLabActivity.SocketType);
            _socketLabActivity.SocketActivityType = (ESocketActivity)EditorGUILayout.EnumPopup("Socket Activity Type", _socketLabActivity.SocketActivityType);
        }

        public override EActivity GetActivityType()
        {
            return EActivity.SocketActivity;
        }
#endif
    }
}