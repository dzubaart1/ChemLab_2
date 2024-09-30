using System.Threading.Tasks;
using BioEngineerLab.Machines;

namespace BioEngineerLab.Core
{
    public class MachinesService : IService
    {
        public KrussMachine KrussMachine { get; private set; }
        
        public Task Initialize()
        {
            return Task.CompletedTask;
        }

        public void Destroy()
        {
        }

        public void RegisterKrussMachine(KrussMachine machine)
        {
            KrussMachine = machine;
        }
    }
}