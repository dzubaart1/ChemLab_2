using UnityEditor;

namespace BioEngineerLab.Activities
{
    public class CraftSubstanceActivity : Activity
    {
        public EContainer Container { get; private set; }
        public CraftConfig CraftConfig { get; private set; }

        public CraftSubstanceActivity(EContainer container = EContainer.KuvetkaContainer, CraftConfig craftConfig = null) 
            : base(EActivity.CraftSubstanceActivity)
        {
            Container = container;
            CraftConfig = craftConfig;
        }
        
        public override void ShowInEditor()
        {
            Container = (EContainer)EditorGUILayout.EnumPopup("Container Using To Craft", Container);
            CraftConfig = EditorGUILayout.ObjectField("Craft Config", CraftConfig, typeof(CraftConfig), true) as CraftConfig;
        }

        public override bool CompleteActivity(Activity activity)
        {
            if (activity is not CraftSubstanceActivity craftSubstanceActivity)
            {
                return false;
            }

            return Container == craftSubstanceActivity.Container &
                   CraftConfig == craftSubstanceActivity.CraftConfig;
        }
    }
}