using BioEngineerLab.Activities;
using Core;
using JetBrains.Annotations;
using UnityEditor;

namespace Activities.ActivitiesEditor
{
    public class PulverizatorLabActivityEditor : EditorActivity
    {
#if UNITY_EDITOR
        [CanBeNull] private PulverizatorLabActivity _pulverizatorLabActivity;
        
        public PulverizatorLabActivityEditor(LabActivity labActivity)
            : base(labActivity)
        {
            if (labActivity is PulverizatorLabActivity handler)
            {
                _pulverizatorLabActivity = handler;
            }
        }
        
        public override void ShowInEditor()
        {
            if (_pulverizatorLabActivity == null)
            {
                return;
            }
            
            _pulverizatorLabActivity.Hits = (EPulverizatorHits)EditorGUILayout.EnumPopup("Hits", _pulverizatorLabActivity.Hits);
        }

        public override EActivity GetActivityType()
        {
            return EActivity.PulveriazatorActivity;
        }
#endif
    }
}