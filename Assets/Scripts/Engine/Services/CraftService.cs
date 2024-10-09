using System;
using System.Collections.Generic;
using System.Linq;
using BioEngineerLab.Activities;
using BioEngineerLab.Configurations;
using BioEngineerLab.Containers;
using BioEngineerLab.Substances;
using JetBrains.Annotations;
using Debug = UnityEngine.Debug;

namespace BioEngineerLab.Core
{
    public class CraftService : IService
    {
        public CraftConfiguration Configuration { get; private set; }

        private TasksService _tasksService;
        
        public CraftService(CraftConfiguration configuration, TasksService tasksService)
        {
            Configuration = configuration;

            _tasksService = tasksService;
        }
        
        public void Transfer(Container fromContainer, Container toContainer)
        {
            Debug.Log("FROM");
            fromContainer.PrintContainerInfo();
            Debug.Log("TO");
            toContainer.PrintContainerInfo();
            
            if (fromContainer.IsSpoonContainer & fromContainer.GetSubstancesCount() == 0 &
                toContainer.GetSubstancesCount() != 0)
            {
                Add(toContainer, fromContainer);
                return;
            }

            if (fromContainer.IsSpoonContainer & fromContainer.GetSubstancesCount() != 0 &
                toContainer.GetSubstancesCount() != 0)
            {
                Mix(fromContainer, toContainer);
                return;
            }

            if (fromContainer.IsSpoonContainer & fromContainer.GetSubstancesCount() != 0 &
                toContainer.GetSubstancesCount() == 0)
            {
                Add(fromContainer, toContainer);
                return;
            }
            
            if (fromContainer.GetSubstancesCount() == 0)
            {
                return;
            }

            if (toContainer.GetSubstancesCount() != 0)
            {
                Mix(fromContainer, toContainer);
                return;
            }

            if (toContainer.GetSubstancesCount() == 0)
            {
                Add(fromContainer, toContainer);
            }
        }

        public void HeatStir(Container container)
        {
            CraftConfig heatStirCraft = FindCraft(container.GetSubstanceProperties(), ECraft.HeatStir);

            if (heatStirCraft == null)
            {
                return;
            }

            float heatStirWeight = container.GetSubstancesWeight();
            float weightForEachSubstances = heatStirWeight / heatStirCraft.SubstancesRes.Length;

            container.ClearContainer();
            foreach (var substanceProperty in heatStirCraft.SubstancesRes)
            {
                container.PutSubstance(new Substance(substanceProperty, weightForEachSubstances));
            }
            
            _tasksService.TryCompleteTask(new CraftSubstanceActivity(container.ContainerType, heatStirCraft));
        }


        public void Dry(Container container)
        {
            CraftConfig dryConfig = FindCraft(container.GetSubstanceProperties(), ECraft.Dry);

            if (dryConfig == null)
            {
                return;
            }

            float dryWeight = container.GetSubstancesWeight();
            float weightForEachSubstances = dryWeight / dryConfig.SubstancesRes.Length;

            container.ClearContainer();
            foreach (var substanceProperty in dryConfig.SubstancesRes)
            {
                container.PutSubstance(new Substance(substanceProperty, weightForEachSubstances));
            }
            
            _tasksService.TryCompleteTask(new CraftSubstanceActivity(container.ContainerType, dryConfig));
        }

        public void Split(Container container)
        {
            CraftConfig splitConfig = FindCraft(container.GetSubstanceProperties(), ECraft.Split);

            if (splitConfig == null)
            {
                return;
            }

            float splitWeight = container.GetSubstancesWeight();
            float weightForEachSubstances = splitWeight / splitConfig.SubstancesRes.Length;

            container.ClearContainer();
            foreach (var substanceProperty in splitConfig.SubstancesRes)
            {
                container.PutSubstance(new Substance(substanceProperty, weightForEachSubstances));
            }
            
            _tasksService.TryCompleteTask(new CraftSubstanceActivity( container.ContainerType, splitConfig));
        }
        
        private void Add(Container fromContainer, Container toContainer)
        {
            Substance transferSubstance = fromContainer.GetTopSubstance();

            if (transferSubstance == null)
            {
                return;
            }
            
            float transferWeight = Math.Min(toContainer.GetAvailableWeight(), transferSubstance.Weight);

            Substance toContainerSubstance = new Substance(transferSubstance.SubstanceProperty, transferWeight);
            toContainer.PutSubstance(toContainerSubstance);

            if (transferSubstance.Weight > transferWeight)
            {
                transferSubstance.RemoveWeight(transferWeight);
            }
            else
            {
                fromContainer.DeleteSubstanceByLayer(transferSubstance.SubstanceProperty.SubstanceLayer);
            }
            
            _tasksService.TryCompleteTask(new AddSubstanceActivity(fromContainer.ContainerType, toContainer.ContainerType, transferSubstance.SubstanceProperty));
        }

        private void Mix(Container fromContainer, Container toContainer)
        {
            CraftConfig mixConfig = FindCraft(fromContainer.GetSubstanceProperties(), ECraft.Mix);

            if (mixConfig == null)
            {
                return;
            }
            
            float transferWeight = Math.Min(toContainer.GetAvailableWeight(), fromContainer.GetSubstancesWeight());
            float weightForEachSubstance = transferWeight / mixConfig.SubstancesRes.Length;
            
            toContainer.ClearContainer();
            foreach (SubstanceProperty substancePropertyRes in mixConfig.SubstancesRes)
            {
                toContainer.PutSubstance(new Substance(substancePropertyRes, weightForEachSubstance));
            }
            
            foreach (Substance substance in fromContainer.Substances)
            {
                if (substance.Weight > weightForEachSubstance)
                {
                    substance.RemoveWeight(weightForEachSubstance);
                }
                else
                {
                    fromContainer.DeleteSubstanceByLayer(substance.SubstanceProperty.SubstanceLayer);
                }
            }
            
            _tasksService.TryCompleteTask(new CraftSubstanceActivity(toContainer.ContainerType, mixConfig));
        }
        
        [CanBeNull]
        private CraftConfig FindCraft(IReadOnlyCollection<SubstanceProperty> from, ECraft craftType)
        {
            return Configuration.Crafts.FirstOrDefault(config => 
                config.CraftType == craftType &
                config.SubstancesFrom.Length == from.Count &&
                config.SubstancesFrom.All(from.Contains)
                );
        }
        
        public void Initialize()
        {
        }

        public void Destroy()
        {
        }
    }
}
