using BioEngineerLab.Tasks.Activities;
using JetBrains.Annotations;
using UnityEditor;

namespace BioEngineerLab.Activities
{
    public class AnchorActivityEditor : EditorActivity
    {
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
    }
}