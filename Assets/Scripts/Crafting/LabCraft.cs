using System;
using System.Linq;
using BioEngineerLab;
using BioEngineerLab.Tasks;

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
        
        public bool Equals(LabCraft labCraft)
        {
            if (labCraft == null)
            {
                return false;
            }
            
            return SubstancesFrom.All(labCraft.SubstancesFrom.Contains) &&
                   labCraft.SubstancesFrom.All(SubstancesFrom.Contains) &&
                   SubstancesRes.All(labCraft.SubstancesRes.Contains) &&
                   labCraft.SubstancesRes.All(SubstancesRes.Contains) &&
                   CraftType == labCraft.CraftType;
        }
    }
}