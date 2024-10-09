using UnityEditor;

namespace BioEngineerLab.Activities
{
    public class AnchorActivity : Activity
    {
        public EContainer ContainerType { get; private set; }
        
        public AnchorActivity(EContainer containerType = EContainer.KuvetkaContainer)
            : base(EActivity.AnchorActivity)
        {
            ContainerType = containerType;
        }
        
        public override void ShowInEditor()
        {
            ContainerType = (EContainer)EditorGUILayout.EnumPopup("Container Type", ContainerType);
        }

        public override bool CompleteActivity(Activity activity)
        {
            if (activity is not AnchorActivity anchorActivity)
            {
                return false;
            }

            return ContainerType == anchorActivity.ContainerType;
        }
    }
}