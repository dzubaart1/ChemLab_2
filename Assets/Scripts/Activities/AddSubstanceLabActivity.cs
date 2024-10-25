using BioEngineerLab.Tasks;
using BioEngineerLab.Tasks.Activities;

namespace BioEngineerLab.Activities
{
    public class AddSubstanceLabActivity : LabActivity
    {
        public EContainer FromContainer;
        public EContainer ToContainer;
        public LabSubstanceProperty LabSubstanceProperty;

        public AddSubstanceLabActivity()
            : base(EActivity.AddSubstanceActivity)
        {
        }

        public AddSubstanceLabActivity(AddSubstanceLabActivity addSubstanceLabActivity)
            : base(EActivity.AddSubstanceActivity)
        {
            FromContainer = addSubstanceLabActivity.FromContainer;
            ToContainer = addSubstanceLabActivity.ToContainer;
            LabSubstanceProperty = new LabSubstanceProperty(addSubstanceLabActivity.LabSubstanceProperty);
        }

        public AddSubstanceLabActivity(EContainer fromContainer, EContainer toContainer, LabSubstanceProperty labSubstanceProperty) 
            : base(EActivity.AddSubstanceActivity)
        {
            FromContainer = fromContainer;
            ToContainer = toContainer;
            LabSubstanceProperty = new LabSubstanceProperty(labSubstanceProperty);
        }

        public override bool Equals(LabActivity labActivity)
        {
            if (labActivity is not AddSubstanceLabActivity handlerAddSubstanceActivity)
            {
                return false;
            }

            return FromContainer == handlerAddSubstanceActivity.FromContainer &&
                   ToContainer == handlerAddSubstanceActivity.ToContainer &&
                   LabSubstanceProperty.Equals(handlerAddSubstanceActivity.LabSubstanceProperty);
        }
    }
}