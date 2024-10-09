using BioEngineerLab.Configurations;
using JetBrains.Annotations;
using UnityEngine;

namespace BioEngineerLab.Core
{
    public class SubstanceColorsService : IService
    {
        public SubstanceColorsConfiguration Configuration { get; private set; }
        
        public SubstanceColorsService(SubstanceColorsConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public void Initialize()
        {
        }

        public void Destroy()
        {
        }


        public bool TryGetColorByType(ESubstanceColor color, out ColorConfig res)
        {
            res = null;
            
            foreach(ColorConfig config in Configuration.ColorConfigs)
            {
                if (config.SubstanceColorType == color)
                {
                    res = config;
                    return true;
                }
            }

            Debug.LogError("Can't Find Color!");
            return false;
        }
    }
}