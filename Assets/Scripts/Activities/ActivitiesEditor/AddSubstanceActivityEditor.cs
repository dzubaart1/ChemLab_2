using System.Linq;
using BioEngineerLab.Activities;
using BioEngineerLab.Tasks;
using Core;
using Database;
using JetBrains.Annotations;
using UnityEditor;

namespace Activities.ActivitiesEditor
{
    #if UNITY_EDITOR
    public class AddSubstanceActivityEditor : EditorActivity
    {
        [CanBeNull] private AddSubstanceLabActivity _addSubstanceActivity;
        [CanBeNull] private SOLabSubstanceProperty _soLabTransferSubstanceProperty;
        
        public AddSubstanceActivityEditor(LabActivity labActivity)
            : base(labActivity)
        {
            if (labActivity is AddSubstanceLabActivity handler)
            {
                _addSubstanceActivity = handler;
            }

            if (_addSubstanceActivity != null)
            {
                _soLabTransferSubstanceProperty = ResourcesDatabase.ReadWhereSubstanceProperties(
                        substanceProperty =>
                            substanceProperty.LabSubstanceProperty.Equals(_addSubstanceActivity.LabSubstanceProperty))
                    .FirstOrDefault();
            }
        }
        
        public override void ShowInEditor()
        {
            if (_addSubstanceActivity == null)
            {
                return;
            }

            _addSubstanceActivity.FromContainer = (EContainer)EditorGUILayout.EnumPopup("From Container", _addSubstanceActivity.FromContainer);
            _addSubstanceActivity.ToContainer = (EContainer)EditorGUILayout.EnumPopup("To Container", _addSubstanceActivity.ToContainer);
            _soLabTransferSubstanceProperty = EditorGUILayout.ObjectField("Adding Substance", _soLabTransferSubstanceProperty, typeof(SOLabSubstanceProperty), true) as SOLabSubstanceProperty;
            if (_soLabTransferSubstanceProperty != null)
            {
                _addSubstanceActivity.LabSubstanceProperty = new LabSubstanceProperty(_soLabTransferSubstanceProperty.LabSubstanceProperty);
            }
        }

        public override EActivity GetActivityType()
        {
            return EActivity.AddSubstanceActivity;
        }
    }
    #endif
}