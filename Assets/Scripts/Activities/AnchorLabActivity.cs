using BioEngineerLab.Tasks;
using BioEngineerLab.Tasks.Activities;

namespace BioEngineerLab.Activities
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

        public override bool Equals(LabActivity labActivity)
        {
            if (labActivity is not AnchorLabActivity handlerAnchorActivity)
            {
                return false;
            }

            return ContainerType == handlerAnchorActivity.ContainerType;
        }
    }
}