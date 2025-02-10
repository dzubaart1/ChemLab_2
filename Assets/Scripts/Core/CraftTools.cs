using System;
using System.Collections.Generic;
using System.Linq;
using BioEngineerLab.Tasks;
using Containers;
using Crafting;

namespace Core.Services
{
    public static class CraftTools
    {
        public static void ApplyCraft(LabCraft craft, LabContainer labContainer)
        {
            float heatStirWeight = labContainer.GetSubstancesWeight();
            float weightForEachSubstances = heatStirWeight / craft.SubstancesRes.Length;

            labContainer.ClearContainer();
            foreach (var substanceProperty in craft.SubstancesRes)
            {
                labContainer.PutSubstance(new LabSubstance(substanceProperty, weightForEachSubstances));
            }
        }
        
        public static bool TryAdd(LabContainer fromLabContainer, LabContainer toLabContainer, out LabSubstance transferSubstance)
        {
            transferSubstance = fromLabContainer.GetTopSubstance();

            if (transferSubstance == null)
            {
                return false;
            }

            float transferWeight = Math.Min(toLabContainer.GetAvailableWeight(), transferSubstance.Weight);

            LabSubstance toContainerLabSubstance = new LabSubstance(transferSubstance.SubstanceProperty, transferWeight);
            toLabContainer.PutSubstance(toContainerLabSubstance);

            if (transferSubstance.Weight > transferWeight)
            {
                transferSubstance.RemoveWeight(transferWeight);
            }
            else
            {
                fromLabContainer.DeleteSubstanceByLayer(transferSubstance.SubstanceProperty.SubstanceLayer);
            }

            return true;
        }

        public static void Mix(LabCraft mixCraft, LabContainer fromLabContainer, LabContainer toLabContainer)
        {
            List<LabSubstanceProperty> mixSubstances = new List<LabSubstanceProperty>();
            mixSubstances.AddRange(fromLabContainer.GetSubstanceProperties());
            mixSubstances.AddRange(toLabContainer.GetSubstanceProperties());
            
            float transferWeight = Math.Min(toLabContainer.GetAvailableWeight(), fromLabContainer.GetSubstancesWeight());
            float toContainerWeight = toLabContainer.GetSubstancesWeight() + transferWeight;
            float fromContainerWeight = fromLabContainer.GetSubstancesWeight() - transferWeight;
            
            toLabContainer.ClearContainer();
            foreach (LabSubstanceProperty substanceProperty in mixCraft.SubstancesRes)
            {
                toLabContainer.PutSubstance(new LabSubstance(substanceProperty, toContainerWeight / mixCraft.SubstancesRes.Length));
            }
            
            foreach (LabSubstance substance in fromLabContainer.Substances)
            {
                if (substance == null)
                {
                    continue;
                }
                
                if (fromContainerWeight > 0)
                {
                    substance.RemoveWeight(transferWeight);
                }
                else
                {
                    fromLabContainer.DeleteSubstanceByLayer(substance.SubstanceProperty.SubstanceLayer);
                }
            }
        }
        
        public static bool TryFindCraft(IReadOnlyCollection<SOLabCraft> labCrafts,IReadOnlyCollection<LabSubstanceProperty> from, ECraft craftType, out SOLabCraft targetCraft)
        {
            targetCraft = labCrafts.FirstOrDefault(craft => craft.LabCraft.SubstancesFrom.All(from.Contains) &
                                                        from.All(craft.LabCraft.SubstancesFrom.Contains) &
                                                        craftType == craft.LabCraft.CraftType);

            return targetCraft != null;
        }
    }
}
