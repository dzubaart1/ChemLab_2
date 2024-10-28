using BioEngineerLab.Tasks.Activities;
using UnityEditor;

namespace BioEngineerLab.Activities
{
    public class CraftSubstanceActivity : Activity
    {
        public EContainer Container;
        public ECraft CraftType;

        public CraftSubstanceActivity(EContainer container = EContainer.KuvetkaContainer, ECraft craftType = ECraft.Dry) 
            : base(EActivity.CraftSubstanceActivity)
        {
            Container = container;
            CraftType = craftType;
        }
        
        public override void ShowInEditor()
        {
            Container = (EContainer)EditorGUILayout.EnumPopup("Container Using To Craft", Container);
            CraftType = (ECraft)EditorGUILayout.EnumPopup("Craft Type", CraftType);
        }

        public override bool CompleteActivity(Activity activity)
        {
            if (activity is not CraftSubstanceActivity craftSubstanceActivity)
            {
                return false;
            }

            return Container == craftSubstanceActivity.Container &
                   CraftType == craftSubstanceActivity.CraftType;
        }
    }
}