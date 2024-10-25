using System;
using System.Collections.Generic;
using System.Linq;
using BioEngineerLab.Activities;
using BioEngineerLab.Configurations;
using BioEngineerLab.Containers;
using BioEngineerLab.Substances;
using BioEngineerLab.Tasks;
using Crafting;
using JetBrains.Annotations;
using Debug = UnityEngine.Debug;

namespace BioEngineerLab.Core
{
    public class CraftService : IService
    {
        public CraftConfiguration Configuration { get; private set; }

        private TasksService _tasksService;

        private List<SOLabSubstanceProperty> _soLabSubstanceProperties;
        private List<SOLabCraft> _soLabCrafts;
        
        public CraftService(CraftConfiguration configuration, TasksService tasksService)
        {
            Configuration = configuration;

            _tasksService = tasksService;
        }

        public void Transfer(LabContainer fromLabContainer, LabContainer toLabContainer)
        {
            Debug.Log("FROM");
            fromLabContainer.PrintContainerInfo();
            Debug.Log("TO");
            toLabContainer.PrintContainerInfo();
            
            if (fromLabContainer.IsSpoonContainer & fromLabContainer.GetSubstancesCount() == 0 &
                toLabContainer.GetSubstancesCount() != 0)
            {
                Add(toLabContainer, fromLabContainer);
                return;
            }

            if (fromLabContainer.IsSpoonContainer & fromLabContainer.GetSubstancesCount() != 0 &
                toLabContainer.GetSubstancesCount() != 0)
            {
                Mix(fromLabContainer, toLabContainer);
                return;
            }

            if (fromLabContainer.IsSpoonContainer & fromLabContainer.GetSubstancesCount() != 0 &
                toLabContainer.GetSubstancesCount() == 0)
            {
                Add(fromLabContainer, toLabContainer);
                return;
            }
            
            if (fromLabContainer.GetSubstancesCount() == 0)
            {
                return;
            }

            if (toLabContainer.GetSubstancesCount() != 0)
            {
                Mix(fromLabContainer, toLabContainer);
                return;
            }

            if (toLabContainer.GetSubstancesCount() == 0)
            {
                Add(fromLabContainer, toLabContainer);
            }
        }

        public void HeatStir(LabContainer labContainer)
        {
            LabCraft heatStirCraft = FindCraft(labContainer.GetSubstanceProperties(), ECraft.HeatStir);

            if (heatStirCraft == null)
            {
                return;
            }

            float heatStirWeight = labContainer.GetSubstancesWeight();
            float weightForEachSubstances = heatStirWeight / heatStirCraft.SubstancesRes.Length;

            labContainer.ClearContainer();
            foreach (var substanceProperty in heatStirCraft.SubstancesRes)
            {
                labContainer.PutSubstance(new LabSubstance(substanceProperty, weightForEachSubstances));
            }
            
            _tasksService.TryCompleteTask(new CraftSubstanceLabActivity(labContainer.ContainerType, heatStirCraft));
        }


        public void Dry(LabContainer labContainer)
        {
            LabCraft dryCraft = FindCraft(labContainer.GetSubstanceProperties(), ECraft.Dry);

            if (dryCraft == null)
            {
                return;
            }

            float dryWeight = labContainer.GetSubstancesWeight();
            float weightForEachSubstances = dryWeight / dryCraft.SubstancesRes.Length;

            labContainer.ClearContainer();
            foreach (var substanceProperty in dryCraft.SubstancesRes)
            {
                labContainer.PutSubstance(new LabSubstance(substanceProperty, weightForEachSubstances));
            }
            
            _tasksService.TryCompleteTask(new CraftSubstanceLabActivity(labContainer.ContainerType, dryCraft));
        }

        public void Split(LabContainer labContainer)
        {
            LabCraft splitCraft = FindCraft(labContainer.GetSubstanceProperties(), ECraft.Split);

            if (splitCraft == null)
            {
                return;
            }

            float splitWeight = labContainer.GetSubstancesWeight();
            float weightForEachSubstances = splitWeight / splitCraft.SubstancesRes.Length;

            labContainer.ClearContainer();
            foreach (var substanceProperty in splitCraft.SubstancesRes)
            {
                labContainer.PutSubstance(new LabSubstance(substanceProperty, weightForEachSubstances));
            }
            
            _tasksService.TryCompleteTask(new CraftSubstanceLabActivity( labContainer.ContainerType, splitCraft));
        }
        
        private void Add(LabContainer fromLabContainer, LabContainer toLabContainer)
        {
            LabSubstance transferLabSubstance = fromLabContainer.GetTopSubstance();

            if (transferLabSubstance == null)
            {
                return;
            }

            float transferWeight = Math.Min(toLabContainer.GetAvailableWeight(), transferLabSubstance.Weight);

            LabSubstance toContainerLabSubstance = new LabSubstance(transferLabSubstance.SubstanceProperty, transferWeight);
            toLabContainer.PutSubstance(toContainerLabSubstance);

            if (transferLabSubstance.Weight > transferWeight)
            {
                transferLabSubstance.RemoveWeight(transferWeight);
            }
            else
            {
                fromLabContainer.DeleteSubstanceByLayer(transferLabSubstance.SubstanceProperty.SubstanceLayer);
            }

            _tasksService.TryCompleteTask(new AddSubstanceLabActivity(fromLabContainer.ContainerType, toLabContainer.ContainerType, transferLabSubstance.SubstanceProperty));
        }

        private void Mix(LabContainer fromLabContainer, LabContainer toLabContainer)
        {
            LabCraft mixCraft = FindCraft(fromLabContainer.GetSubstanceProperties(), ECraft.Mix);

            if (mixCraft == null)
            {
                return;
            }
            
            float transferWeight = Math.Min(toLabContainer.GetAvailableWeight(), fromLabContainer.GetSubstancesWeight());
            float weightForEachSubstance = transferWeight / mixCraft.SubstancesRes.Length;
            
            toLabContainer.ClearContainer();
            foreach (LabSubstanceProperty substanceProperty in mixCraft.SubstancesRes)
            {
                toLabContainer.PutSubstance(new LabSubstance(substanceProperty, weightForEachSubstance));
            }
            
            foreach (LabSubstance substance in fromLabContainer.Substances)
            {
                if (substance.Weight > weightForEachSubstance)
                {
                    substance.RemoveWeight(weightForEachSubstance);
                }
                else
                {
                    fromLabContainer.DeleteSubstanceByLayer(substance.SubstanceProperty.SubstanceLayer);
                }
            }
            
            _tasksService.TryCompleteTask(new CraftSubstanceLabActivity(toLabContainer.ContainerType, mixCraft));
        }
        
        [CanBeNull]
        private LabCraft FindCraft(IReadOnlyCollection<LabSubstanceProperty> from, ECraft craftType)
        {
            SOLabCraft soLabCraft = _soLabCrafts.FirstOrDefault(craft => craft.LabCraft.SubstancesFrom.All(from.Contains) &
                                                        from.All(craft.LabCraft.SubstancesFrom.Contains) &
                                                        craftType == craft.LabCraft.CraftType);

            if (soLabCraft == null)
            {
                return null;
            }

            return soLabCraft.LabCraft;
        }
        
        public void Initialize()
        {
        }

        public void Destroy()
        {
        }
    }
}
