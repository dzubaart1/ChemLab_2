using BioEngineerLab.Activities;
using Core;
using JetBrains.Annotations;
using UnityEditor;

namespace Activities.ActivitiesEditor
{
    public class AnchorActivityEditor : EditorActivity
    {
#if UNITY_EDITOR
        [CanBeNull] private AnchorLabActivity _anchorActivity;
        
        public AnchorActivityEditor(LabActivity labActivity)
            : base(labActivity)
        {
            if (labActivity is AnchorLabActivity handler)
            {
                _anchorActivity = handler;
            }
        }
        
        public override void ShowInEditor()
        {
            if (_anchorActivity == null)
            {
                return;
            }
            
            _anchorActivity.ContainerType = (EContainer)EditorGUILayout.EnumPopup("Container Type", _anchorActivity.ContainerType);
        }

        public override EActivity GetActivityType()
        {
            return EActivity.AnchorActivity;
        }
#endif
    }
}