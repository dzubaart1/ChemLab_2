using System;

namespace BioEngineerLab.Tasks
{
    [Serializable]
    public class LabSubstanceProperty
    {
        public string SubstanceName;
        public ESubstanceMode SubstanceMode;
        public ESubstanceLayer SubstanceLayer;
        public float SubstanceColorR;
        public float SubstanceColorG;
        public float SubstanceColorB;
        public float SubstanceColorA;
        public string HintName;

        public LabSubstanceProperty()
        {
        }

        public LabSubstanceProperty(LabSubstanceProperty labSubstanceProperty)
        {
            SubstanceName = labSubstanceProperty.SubstanceName;
            SubstanceMode = labSubstanceProperty.SubstanceMode;
            SubstanceLayer = labSubstanceProperty.SubstanceLayer;
            SubstanceColorA = labSubstanceProperty.SubstanceColorA;
            SubstanceColorB = labSubstanceProperty.SubstanceColorB;
            SubstanceColorG = labSubstanceProperty.SubstanceColorG;
            SubstanceColorR = labSubstanceProperty.SubstanceColorR;
            HintName = labSubstanceProperty.HintName;
        }

        public override bool Equals(Object obj)
        {
            if (obj is not LabSubstanceProperty labSubstanceProperty)
            {
                return false;
            }

            const double ACCURACY = 0.1f;
            return SubstanceName == labSubstanceProperty.SubstanceName &
                   SubstanceMode == labSubstanceProperty.SubstanceMode &
                   SubstanceLayer == labSubstanceProperty.SubstanceLayer &
                   Math.Abs(SubstanceColorR - labSubstanceProperty.SubstanceColorR) < ACCURACY &
                   Math.Abs(SubstanceColorG - labSubstanceProperty.SubstanceColorG) < ACCURACY &
                   Math.Abs(SubstanceColorB - labSubstanceProperty.SubstanceColorB) < ACCURACY &
                   Math.Abs(SubstanceColorA - labSubstanceProperty.SubstanceColorA) < ACCURACY &
                   HintName == labSubstanceProperty.HintName;
        }
        
        public override int GetHashCode()
        {
            int sum = SubstanceName.Length;
            
            sum += (int)SubstanceMode;
            sum += (int)SubstanceLayer;

            return sum;
        }
    }
}