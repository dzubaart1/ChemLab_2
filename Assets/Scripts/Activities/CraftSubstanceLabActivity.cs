using System;
using System.Linq;
using BioEngineerLab.Tasks;
using BioEngineerLab.Tasks.Activities;
using Crafting;

namespace BioEngineerLab.Activities
{
    public class CraftSubstanceLabActivity : LabActivity
    {
        public LabCraft LabCraft;
        public EContainer Container;

        public CraftSubstanceLabActivity()
            : base(EActivity.CraftSubstanceActivity)
        {
        }

        public CraftSubstanceLabActivity(CraftSubstanceLabActivity craftSubstanceLabActivity)
            : base(EActivity.CraftSubstanceActivity)
        {
            Container = craftSubstanceLabActivity.Container;
            LabCraft = new LabCraft(craftSubstanceLabActivity.LabCraft);
        }

        public CraftSubstanceLabActivity(EContainer container, LabCraft labCraft) 
            : base(EActivity.CraftSubstanceActivity)
        {
            Container = container;
            LabCraft = new LabCraft(labCraft);
        }

        public override bool Equals(LabActivity labActivity)
        {
            if (labActivity is not CraftSubstanceLabActivity craftSubstanceActivity)
            {
                return false;
            }

            return Container == craftSubstanceActivity.Container &&
                   LabCraft.Equals(craftSubstanceActivity.LabCraft);
        }
    }
}