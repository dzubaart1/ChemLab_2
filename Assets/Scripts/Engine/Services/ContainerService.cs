using System.Collections.Generic;
using BioEngineerLab.Containers;
using UnityEngine;

namespace BioEngineerLab.Core
{
    public class ContainerService : IService
    {
        private List<Container> _containers = new List<Container>();
        
        public void Initialize()
        {
        }

        public void Destroy()
        {
        }

        public void RegisterContainer(Container container)
        {
            _containers.Add(container);
        }

        public List<Container> GetReagentsContainer()
        {
            List<Container> res = new List<Container>();

            foreach (var container in _containers)
            {
                if (container.IsReagentsContainer)
                {
                    res.Add(container);
                }
            }

            return res;
        }
    }
}