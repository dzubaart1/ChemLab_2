using System;

namespace BioEngineerLab.Tasks
{
    [Serializable]
    public class LabSubstanceProperty
    {
        public ESubstanceName SubstanceName;
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

        public bool Equals(LabSubstanceProperty labSubstanceProperty)
        {
            if (labSubstanceProperty == null)
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
    }
}