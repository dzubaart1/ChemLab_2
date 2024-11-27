using System;
using System.Linq;
using BioEngineerLab.Tasks;
using Core;

namespace Crafting
{
    [Serializable]
    public class LabCraft
    {
        public LabSubstanceProperty[] SubstancesFrom = Array.Empty<LabSubstanceProperty>();
        public ECraft CraftType = ECraft.Dry;
        public LabSubstanceProperty[] SubstancesRes = Array.Empty<LabSubstanceProperty>();

        public LabCraft()
        {
        }
        
        public LabCraft(LabCraft labCraft)
        {
            SubstancesFrom = labCraft.SubstancesFrom;
            CraftType = labCraft.CraftType;
            SubstancesRes = labCraft.SubstancesRes;
        }

        public LabCraft(LabSubstanceProperty[] from, LabSubstanceProperty[] res, ECraft craftType)
        {
            SubstancesFrom = from;
            SubstancesRes = res;
            CraftType = craftType;
        }
        
        public override bool Equals(Object obj)
        {
            if (obj is not LabCraft labCraft)
            {
                return false;
            }
            
            return SubstancesFrom.All(labCraft.SubstancesFrom.Contains) &&
                   labCraft.SubstancesFrom.All(SubstancesFrom.Contains) &&
                   SubstancesRes.All(labCraft.SubstancesRes.Contains) &&
                   labCraft.SubstancesRes.All(SubstancesRes.Contains) &&
                   CraftType == labCraft.CraftType;
        }
        
        public override int GetHashCode()
        {
            int sum = 0;

            foreach (var substance in SubstancesFrom)
            {
                sum += substance.GetHashCode();
            }

            foreach (var substance in SubstancesRes)
            {
                sum += substance.GetHashCode();
            }

            sum += (int)CraftType;

            return sum;
        }
    }
}