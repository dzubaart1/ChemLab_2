using BioEngineerLab.Activities;
using Core;
using JetBrains.Annotations;
using UnityEditor;

namespace Activities.ActivitiesEditor
{
    public class ButtonClickedActivityEditor : EditorActivity
    {
#if UNITY_EDITOR
        [CanBeNull] private ButtonClickedActivity _buttonClickedActivity;
        
        public ButtonClickedActivityEditor(LabActivity labActivity)
            : base(labActivity)
        {
            if (labActivity is ButtonClickedActivity handler)
            {
                _buttonClickedActivity = handler;
            }
        }
        
        public override void ShowInEditor()
        {
            if (_buttonClickedActivity == null)
            {
                return;
            }
            
            _buttonClickedActivity.ButtonType = (EButton)EditorGUILayout.EnumPopup("Button Type", _buttonClickedActivity.ButtonType);
        }

        public override EActivity GetActivityType()
        {
            return EActivity.ButtonClickedActivity;
        }
#endif
    }
}