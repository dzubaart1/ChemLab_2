using BioEngineerLab.Configurations;
using BioEngineerLab.Core;
using UnityEngine;

namespace BioEngineerLab.Substances
{
    public class Substance
    {
        public float Weight
        {
            get
            {
                return _weight;
            }
        }
        public SubstanceProperty SubstanceProperty { get; private set; }

        private float _weight;

        private SubstanceColorsService _substanceColorsService;
        
        public Substance(SubstanceProperty substanceProperty, float weight)
        {
            _weight = weight;
            SubstanceProperty = substanceProperty;
            _substanceColorsService = Engine.GetService<SubstanceColorsService>();

        }

        public Substance(Substance substance)
        {
            _weight = substance._weight;
            SubstanceProperty = substance.SubstanceProperty;
            _substanceColorsService = Engine.GetService<SubstanceColorsService>();

        }

        public void RemoveWeight(float weight)
        {
            _weight -= weight;
        }

        public void AddWeight(float weight)
        {
            _weight += weight;
        }

        public Color GetColor()
        {
            if(_substanceColorsService.TryGetColorByType(SubstanceProperty.SubstanceColor, out ColorConfig config))
            {
                return config.Color;
            }
            
            return Color.black;
        }
    }
}