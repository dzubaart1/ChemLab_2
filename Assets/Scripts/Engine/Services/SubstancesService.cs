using System.Threading.Tasks;
using System;
using BioEngineerLab.Activities;
using BioEngineerLab.Configurations;
using BioEngineerLab.Containers;
using BioEngineerLab.Substances;
using Debug = UnityEngine.Debug;

namespace BioEngineerLab.Core
{
    /// <summary>
    ///     Класс работает с самой верхней Substance по Stack
    /// </summary>
    public class SubstancesService : IService
    {
        public SubstancesConfiguration Configuration { get; private set; }

        private TasksService _tasksService;
        
        public SubstancesService(SubstancesConfiguration configuration, TasksService tasksService)
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
            if (fromContainer.IsSpoonContainer & fromContainer.SubstancesList.Count == 0 &
                toContainer.SubstancesList.Count != 0)
            {
                Debug.Log("Transfer 1");
                Add(toContainer, fromContainer);
                SendTransferTask(toContainer, fromContainer);
                return;
            }

            if (fromContainer.IsSpoonContainer & fromContainer.SubstancesList.Count != 0 &
                toContainer.SubstancesList.Count != 0)
            {
                Debug.Log("Transfer 2");
                Mix(fromContainer, toContainer);
                SendTransferTask(fromContainer, toContainer);
                return;
            }

            if (fromContainer.SubstancesList.Count == 0)
            {
                Debug.Log("Transfer 3");
                return;
            }

            if (toContainer.SubstancesList.Count != 0 && toContainer.IsMixableContainer)
            {
                Debug.Log("Transfer 4");
                Mix(fromContainer, toContainer);
                SendTransferTask(fromContainer, toContainer);
                return;
            }

            Add(fromContainer, toContainer);
            SendTransferTask(fromContainer, toContainer);
        }

        public void HeatStir(Container container)
        {
            if(container.SubstancesList.Count == 0)
            {
                return;
            }

            var heatingSubstance = container.RemoveLastSubstance();

            var resSubstanceProperty = FindSubstancePropertyByMode(heatingSubstance.SubstanceProperty.SubstanceName, SubstanceMode.HeatStir);
            
            if(resSubstanceProperty.SubSubstanceProperties.Count == 0)
            {
                var newSubstance = new Substance(resSubstanceProperty, heatingSubstance.Weight);
                container.AddSubstance(newSubstance);
                return;
            }

            GenerateSubSubstanceToContainer(heatingSubstance.Weight, resSubstanceProperty, container);
        }

        public void Dry(Container container)
        {
            if (container.SubstancesList.Count == 0)
            {
                return;
            }

            var dryingSubstance = container.RemoveLastSubstance();

            var resSubstanceProperty = FindSubstancePropertyByMode(dryingSubstance.SubstanceProperty.SubstanceName, SubstanceMode.Dry);

            if (resSubstanceProperty.SubSubstanceProperties.Count == 0)
            {
                var newSubstance = new Substance(resSubstanceProperty, dryingSubstance.Weight);
                container.AddSubstance(newSubstance);
                return;
            }

            GenerateSubSubstanceToContainer(dryingSubstance.Weight, resSubstanceProperty, container);
        }

        public void Split(Container container)
        {
            if (container.SubstancesList.Count == 0)
            {
                return;
            }

            var splitingSubstance = container.RemoveLastSubstance();

            var resSubstanceProperty = FindSubstancePropertyByMode(splitingSubstance.SubstanceProperty.SubstanceName, SubstanceMode.Split);

            GenerateSubSubstanceToContainer(splitingSubstance.Weight, resSubstanceProperty, container);
        }
        
        private void Add(Container fromContainer, Container toContainer)
        {
            var transferSubstance = fromContainer.RemoveLastSubstance();
            var transferWeight = Math.Min(toContainer.GetAvailableWeight(), transferSubstance.Weight);

            var toContainerSubstance = new Substance(transferSubstance.SubstanceProperty, transferWeight);
            toContainer.AddSubstance(toContainerSubstance);

            if (transferSubstance.Weight > transferWeight)
            {
                var fromContainerSubstance = new Substance(transferSubstance.SubstanceProperty, transferSubstance.Weight - transferWeight);
                fromContainer.AddSubstance(fromContainerSubstance);
            }
        }

        private void Mix(Container fromContainer, Container toContainer)
        {
            Debug.Log("MIX ENTER");
            Debug.Log("FROM");
            fromContainer.PrintContainerInfo();
            Debug.Log("TO");
            toContainer.PrintContainerInfo();
            var fromContainerSubstance = fromContainer.RemoveLastSubstance();
            var toContainerSubstance = toContainer.RemoveLastSubstance();
            var transferWeight = Math.Min(toContainer.GetAvailableWeight(), fromContainerSubstance.Weight);

            if (fromContainerSubstance.SubstanceProperty.SubstanceName == toContainerSubstance.SubstanceProperty.SubstanceName)
            {
                toContainerSubstance.AddWeight(fromContainerSubstance.Weight);
                toContainer.AddSubstance(toContainerSubstance);

                if(fromContainerSubstance.Weight > transferWeight)
                {
                    fromContainerSubstance.RemoveWeight(transferWeight);
                    fromContainer.AddSubstance(fromContainerSubstance);
                }
                return;
            }
            Debug.Log($"1 {fromContainerSubstance == null}");
            Debug.Log($"2 {fromContainerSubstance.SubstanceProperty == null}");
            Debug.Log($"3 {toContainerSubstance.SubstanceProperty == null}");
            Debug.Log($"4 {toContainerSubstance.SubstanceProperty == null}");
            var mixSubstanceProperty = FindMixSubstanceProperty(fromContainerSubstance.SubstanceProperty, toContainerSubstance.SubstanceProperty);
            toContainer.AddSubstance(new Substance(mixSubstanceProperty, transferWeight));
            
            if (fromContainerSubstance.Weight > transferWeight)
            {
                fromContainerSubstance.RemoveWeight(transferWeight);
                fromContainer.AddSubstance(fromContainerSubstance);
            }
        }

        private SubstanceProperty FindSubstancePropertyByMode(SubstanceName name, SubstanceMode mode)
        {
            foreach (var substanceProperty in Configuration.SubstanceProperties)
            {
                if (substanceProperty.SubstanceName == name
                     && substanceProperty.SubstanceMode == mode)
                {
                    return substanceProperty;
                }
            }
            
            return Configuration.BadSubstance;
        }

        private SubstanceProperty FindMixSubstanceProperty(SubstanceProperty sub1, SubstanceProperty sub2)
        {
            bool isSub1Find = false;
            bool IsSub2Find = false;
            foreach (var substanceProperty in Configuration.SubstanceProperties)
            {
                isSub1Find = false;
                IsSub2Find = false;
                if (substanceProperty.SubstanceMode != SubstanceMode.Mix || substanceProperty.SubSubstanceProperties.Count != 2)
                {
                    continue;
                }

                foreach (var subSubstanceProperty in substanceProperty.SubSubstanceProperties)
                {
                    if (subSubstanceProperty.SubstanceProperty == sub1)
                    {
                        isSub1Find = true;
                    }

                    if (subSubstanceProperty.SubstanceProperty == sub2)
                    {
                        IsSub2Find = true;
                    }
                }

                if (isSub1Find && IsSub2Find)
                {
                    return substanceProperty;
                }
            }

            return Configuration.BadSubstance;
        }

        private void GenerateSubSubstanceToContainer(float substanceWeight, SubstanceProperty substanceProperty, Container container)
        {
            if(substanceProperty.SubSubstanceProperties.Count == 0)
            {
                return;
            }

            foreach(var subSubstanceProperty in substanceProperty.SubSubstanceProperties)
            {
                var newSubstance = new Substance(substanceProperty, substanceWeight * (subSubstanceProperty.SubstanceWeight / substanceProperty.SumSubSubstancePropertiesWeights));
                container.AddSubstance(newSubstance);
            }
        }

        private void SendTransferTask(Container fromContainer, Container toContainer)
        {
            _tasksService.TryCompleteTask(new TransferActivity(
                fromContainer.ContainerType,
                toContainer.ContainerType,
                toContainer.PeekLastSubstance().SubstanceProperty.SubstanceName,
                toContainer.PeekLastSubstance().SubstanceProperty.SubstanceMode));
        }
        
        public Task Initialize()
        {
            return Task.CompletedTask;
        }

        public void Destroy()
        {
        }
    }
}
