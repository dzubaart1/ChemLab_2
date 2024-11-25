using System;
using Core;
using Substances;

namespace Activities
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

        public override bool Equals(Object obj)
        {
            if (obj is not AddSubstanceLabActivity handlerAddSubstanceActivity)
            {
                return false;
            }

            return FromContainer == handlerAddSubstanceActivity.FromContainer &&
                   ToContainer == handlerAddSubstanceActivity.ToContainer &&
                   LabSubstanceProperty.Equals(handlerAddSubstanceActivity.LabSubstanceProperty);
        }

        public override int GetHashCode()
        {
            int sum = (int)FromContainer;

            sum += (int)ToContainer;
            sum += LabSubstanceProperty.GetHashCode();

            return sum;
        }
    }
}