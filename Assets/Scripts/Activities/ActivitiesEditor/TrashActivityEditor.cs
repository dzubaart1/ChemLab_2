using BioEngineerLab.Activities;
using Core;
using JetBrains.Annotations;
using UnityEditor;

namespace Activities.ActivitiesEditor
{
    public class TrashActivityEditor : EditorActivity
    {
#if UNITY_EDITOR
        [CanBeNull] private TrashLabActivity _trashLabActivity;
        
        public TrashActivityEditor(LabActivity labActivity)
            : base(labActivity)
        {
            if (labActivity is TrashLabActivity handler)
            {
                _trashLabActivity = handler;
            }
        }
        
        public override void ShowInEditor()
        {
            if (_trashLabActivity == null)
            {
                return;
            }
            
            _trashLabActivity.TrashType = (ETrashType)EditorGUILayout.EnumPopup("Trash Type", _trashLabActivity.TrashType);
            _trashLabActivity.TrashObject = (ETrashableObject)EditorGUILayout.EnumPopup("Trash Object", _trashLabActivity.TrashObject);
        }

        public override EActivity GetActivityType()
        {
            return EActivity.TrashActivity;
        }
#endif
    }
}