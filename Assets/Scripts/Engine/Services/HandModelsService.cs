using BioEngineerLab.Configurations;

namespace BioEngineerLab.Core
{
    public class HandModelsService : IService
    {
        public HandModelConfiguration Configuration { get; private set; }

        public HandModelsService(HandModelConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void Initialize()
        {
        }

        public void Destroy()
        {
        }
    }
}