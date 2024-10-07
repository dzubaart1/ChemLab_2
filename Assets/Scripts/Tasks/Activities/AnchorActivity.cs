using BioEngineerLab.Containers;
using UnityEditor;

namespace BioEngineerLab.Activities
{
    public class AnchorActivity : Activity
    {
        private EContainer _eContainer;
        
        public AnchorActivity(EContainer eContainer = EContainer.KuvetkaContainer)
            : base(EActivity.AnchorActivity)
        {
            _eContainer = eContainer;
        }
        
        public override void ShowInEditor()
        {
            _eContainer = (EContainer)EditorGUILayout.EnumPopup("Container Type", _eContainer);
        }

        public override bool CompleteActivity(Activity activity)
        {
            if (activity is not AnchorActivity anchorActivity)
            {
                return false;
            }

            return _eContainer == anchorActivity._eContainer;
        }
    }
}