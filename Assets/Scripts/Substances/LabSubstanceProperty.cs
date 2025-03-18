using System;
using Core;
using UnityEngine;
using Object = System.Object;

namespace BioEngineerLab.Tasks
{
    [Serializable]
    public class LabSubstanceProperty
    {
        public string SubstanceName;
        public ESubstanceMode SubstanceMode;
        public ESubstanceLayer SubstanceLayer;
        public bool IsLiquid;
        public float SubstanceColorR;
        public float SubstanceColorG;
        public float SubstanceColorB;
        public float SubstanceColorA;
        public bool HasTexture;
        public String TexturePath;
        public string HintName;

        public LabSubstanceProperty()
        {
        }

        public LabSubstanceProperty(LabSubstanceProperty labSubstanceProperty)
        {
            SubstanceName = labSubstanceProperty.SubstanceName;
            SubstanceMode = labSubstanceProperty.SubstanceMode;
            SubstanceLayer = labSubstanceProperty.SubstanceLayer;
            IsLiquid = labSubstanceProperty.IsLiquid;
            SubstanceColorA = labSubstanceProperty.SubstanceColorA;
            SubstanceColorB = labSubstanceProperty.SubstanceColorB;
            SubstanceColorG = labSubstanceProperty.SubstanceColorG;
            SubstanceColorR = labSubstanceProperty.SubstanceColorR;
            HasTexture = labSubstanceProperty.HasTexture;
            TexturePath = labSubstanceProperty.TexturePath;
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
                   SubstanceLayer == labSubstanceProperty.SubstanceLayer;
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