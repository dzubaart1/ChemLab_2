using System.Collections.Generic;
using BioEngineerLab.Substances;

namespace BioEngineerLab.Configurations
{
    public class CraftConfiguration : Configuration
    {
        public SubstanceProperty BadSubstance;
        public List<SubstanceProperty> SubstanceProperties;
        public List<Craft> Crafts;
    }
}
