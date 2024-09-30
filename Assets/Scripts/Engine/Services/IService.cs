using System.Threading.Tasks;

namespace BioEngineerLab.Core
{
    public interface IService
    {
        public Task Initialize();
        public void Destroy();
    }
}
