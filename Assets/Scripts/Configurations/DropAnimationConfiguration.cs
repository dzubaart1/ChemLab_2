using System;
using System.Collections.Generic;
using BioEngineerLab.Substances;
using UnityEngine;

namespace BioEngineerLab.Configurations
{
    public class DropAnimationConfiguration : Configuration
    {
        [Serializable]
        public struct DropAnimationSubstance
        {
            public SubstanceName SubstanceName;
            public List<Sprite> HeaderSprites;
            public List<Sprite> SubstanceSprites;
            public float AnimationDuration;
            public string AnimationName;
        }

        public List<DropAnimationSubstance> DropAnimationSubstances;
    }
}