using System;
using System.Collections.Generic;
using UnityEngine;

namespace BioEngineerLab.Configurations
{
    public class SubstanceColorsConfiguration : Configuration
    {
        public List<ColorConfig> ColorConfigs;
    }

    [Serializable]
    public class ColorConfig
    {
        public ESubstanceColor SubstanceColorType;
        public Color Color;
    }
}