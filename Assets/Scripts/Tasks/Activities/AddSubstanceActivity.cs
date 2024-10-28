using BioEngineerLab.Tasks.Activities;
using UnityEditor;

namespace BioEngineerLab.Activities
{
    public class AddSubstanceActivity : Activity
    {
        public EContainer FromContainer;
        public EContainer ToContainer;
        public ESubstanceName SubstanceName;
        public ESubstanceMode SubstanceMode;

        public AddSubstanceActivity(EContainer fromContainer = EContainer.KuvetkaContainer, EContainer toContainer = EContainer.KuvetkaContainer, ESubstanceName substanceName = ESubstanceName.Bad, ESubstanceMode substanceMode = ESubstanceMode.Dry) 
            : base(EActivity.AddSubstanceActivity)
        {
            FromContainer = fromContainer;
            ToContainer = toContainer;
            SubstanceName = substanceName;
            SubstanceMode = substanceMode;
        }
        
        public override void ShowInEditor()
        {
            FromContainer = (EContainer)EditorGUILayout.EnumPopup("From Container Type", FromContainer);
            ToContainer = (EContainer)EditorGUILayout.EnumPopup("To Container Type", ToContainer);
            SubstanceName = (ESubstanceName)EditorGUILayout.EnumPopup("Adding Substance Name", SubstanceName);
            SubstanceMode = (ESubstanceMode)EditorGUILayout.EnumPopup("Adding Substance Mode", SubstanceMode);
        }

        public override bool CompleteActivity(Activity activity)
        {
            if (activity is not AddSubstanceActivity addSubstanceActivity)
            {
                return false;
            }

            return FromContainer == addSubstanceActivity.FromContainer &
                   ToContainer == addSubstanceActivity.ToContainer &
                   SubstanceName == addSubstanceActivity.SubstanceName &
                   SubstanceMode == addSubstanceActivity.SubstanceMode;
        }
    }
}