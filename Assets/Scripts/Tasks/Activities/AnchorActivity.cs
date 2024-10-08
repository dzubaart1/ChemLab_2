using BioEngineerLab.Containers;
using UnityEditor;

namespace BioEngineerLab.Activities
{
    public class AnchorActivity : Activity
    {
        private EContainer _containerType;
        
        public AnchorActivity(EContainer containerType = EContainer.KuvetkaContainer)
            : base(EActivity.AnchorActivity)
        {
            _containerType = containerType;
        }
        
        public override void ShowInEditor()
        {
            _containerType = (EContainer)EditorGUILayout.EnumPopup("Container Type", _containerType);
        }

        public override bool CompleteActivity(Activity activity)
        {
            if (activity is not AnchorActivity anchorActivity)
            {
                return false;
            }

            return _containerType == anchorActivity._containerType;
        }
    }
}