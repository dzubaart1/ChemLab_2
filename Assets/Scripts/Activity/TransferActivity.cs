using System;
using BioEngineerLab.Containers;
using BioEngineerLab.Substances;

namespace BioEngineerLab.Activities
{
    public class TransferActivity : Activity
    {
        public ContainerType FromContainer;
        public ContainerType ToContainer;
        public SubstanceName TransferSubstanceName;
        public SubstanceMode TransferSubstanceMode;

        public TransferActivity()
        {
            ActivityType = ActivityType.TransferActivity;
        }
        
        public TransferActivity(ContainerType from, ContainerType to, SubstanceName name, SubstanceMode mode)
        {
            ActivityType = ActivityType.TransferActivity;
            FromContainer = from;
            ToContainer = to;
            TransferSubstanceName = name;
            TransferSubstanceMode = mode;
        }

        public override bool EqualsActivity(Activity activity)
        {
            if (activity is not TransferActivity transferActivity)
            {
                return false;
            }
            
            return FromContainer == transferActivity.FromContainer &
                   ToContainer == transferActivity.ToContainer &
                   TransferSubstanceName == transferActivity.TransferSubstanceName &
                   TransferSubstanceMode == transferActivity.TransferSubstanceMode;
        }
    }

}