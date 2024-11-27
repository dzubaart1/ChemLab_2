using UnityEngine;

namespace BioEngineerLab.Tasks
{
    public class LabSubstance
    {
        public float Weight
        {
            get
            {
                return _weight;
            }
        }
        public LabSubstanceProperty SubstanceProperty { get; private set; }

        private float _weight;
        
        public LabSubstance(LabSubstanceProperty substanceProperty, float weight)
        {
            _weight = weight;
            SubstanceProperty = substanceProperty;
        }

        public LabSubstance(LabSubstance labSubstance)
        {
            _weight = labSubstance._weight;
            SubstanceProperty = labSubstance.SubstanceProperty;
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
            return new Color(SubstanceProperty.SubstanceColorR, SubstanceProperty.SubstanceColorG, SubstanceProperty.SubstanceColorB, SubstanceProperty.SubstanceColorA);
        }
    }
}