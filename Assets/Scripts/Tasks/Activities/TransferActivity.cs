using BioEngineerLab.Containers;
using BioEngineerLab.Substances;
using UnityEditor;

namespace BioEngineerLab.Activities
{
    public class TransferActivity : Activity
    {
        private EContainer _fromEContainer;
        private EContainer _toEContainer;
        private SubstanceProperty _transferSubstance;

        public TransferActivity(EContainer fromEContainer = EContainer.KuvetkaContainer, EContainer toEContainer = EContainer.KuvetkaContainer, SubstanceProperty transferSubstance = null) 
            : base(EActivity.TransferActivity)
        {
            _fromEContainer = fromEContainer;
            _toEContainer = toEContainer;
            _transferSubstance = transferSubstance;
        }
        
        public override void ShowInEditor()
        {
            _fromEContainer = (EContainer)EditorGUILayout.EnumPopup("From Container Type", _fromEContainer);
            _toEContainer = (EContainer)EditorGUILayout.EnumPopup("To Container Type", _toEContainer);
            _transferSubstance = EditorGUILayout.ObjectField("Transfer Substance", _transferSubstance, typeof(SubstanceProperty), true) as SubstanceProperty;
        }

        public override bool CompleteActivity(Activity activity)
        {
            if (activity is not TransferActivity transferActivity)
            {
                return false;
            }

            return _fromEContainer == transferActivity._fromEContainer &
                   _toEContainer == transferActivity._toEContainer &
                   _transferSubstance == transferActivity._transferSubstance;
        }
    }
}