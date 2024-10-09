using BioEngineerLab.Substances;
using UnityEditor;

namespace BioEngineerLab.Activities
{
    public class AddSubstanceActivity : Activity
    {
        public EContainer FromContainer { get; private set; }
        public EContainer ToContainer { get; private set; }
        public SubstanceProperty SubstanceProperty { get; private set; }

        public AddSubstanceActivity(EContainer fromContainer = EContainer.KuvetkaContainer, EContainer toContainer = EContainer.KuvetkaContainer, SubstanceProperty substanceProperty = null) 
            : base(EActivity.AddSubstanceActivity)
        {
            FromContainer = fromContainer;
            ToContainer = toContainer;
            SubstanceProperty = substanceProperty;
        }
        
        public override void ShowInEditor()
        {
            FromContainer = (EContainer)EditorGUILayout.EnumPopup("From Container Type", FromContainer);
            ToContainer = (EContainer)EditorGUILayout.EnumPopup("To Container Type", ToContainer);
            SubstanceProperty = EditorGUILayout.ObjectField("Adding Substance", SubstanceProperty, typeof(SubstanceProperty), true) as SubstanceProperty;
        }

        public override bool CompleteActivity(Activity activity)
        {
            if (activity is not AddSubstanceActivity addSubstanceActivity)
            {
                return false;
            }

            return FromContainer == addSubstanceActivity.FromContainer &
                   ToContainer == addSubstanceActivity.ToContainer &
                   SubstanceProperty == addSubstanceActivity.SubstanceProperty;
        }
    }
}