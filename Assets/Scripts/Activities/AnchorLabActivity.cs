using System;
using Core;

namespace Activities
{
    public class AnchorLabActivity : LabActivity
    {
        public EContainer ContainerType;

        public AnchorLabActivity()
            : base(EActivity.AnchorActivity)
        {
        }

        public AnchorLabActivity(AnchorLabActivity anchorLabActivity)
            : base(EActivity.AnchorActivity)
        {
            ContainerType = anchorLabActivity.ContainerType;
        }
        
        public AnchorLabActivity(EContainer containerType)
            : base(EActivity.AnchorActivity)
        {
            ContainerType = containerType;
        }

        public override bool Equals(Object obj)
        {
            if (obj is not AnchorLabActivity handlerAnchorActivity)
            {
                return false;
            }

            return ContainerType == handlerAnchorActivity.ContainerType;
        }

        public override int GetHashCode()
        {
            return (int)ContainerType;
        }
    }
}