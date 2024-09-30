using System;
using System.Collections.Generic;
using UnityEngine;

namespace BioEngineerLab.Substances
{
    [CreateAssetMenu(fileName = "SubstanceProperty", menuName = "Substances/SubstanceProperty", order = 1)]
    public class SubstanceProperty : ScriptableObject
    {
        public SubstanceName SubstanceName;
        public SubstanceMode SubstanceMode;
        public string HintName;
        public Color Color;
        public List<SubSubstanceProperty> SubSubstanceProperties = new();

        [HideInInspector] public int SumSubSubstancePropertiesWeights;
        public SubstanceProperty()
        {
            foreach (var subSubstanceProperty in SubSubstanceProperties)
            {
                SumSubSubstancePropertiesWeights += subSubstanceProperty.SubstanceWeight;
            }
        }

        public string GetSubstanceName()
        {
            return $"{Enum.GetName(typeof(SubstanceName), SubstanceName)}";
        }
    }

    [Serializable]
    public class SubSubstanceProperty
    {
        public SubstanceProperty SubstanceProperty;
        public int SubstanceWeight;
    }

    public enum SubstanceMode
    {
        Normal,
        Dry,
        Split,
        HeatStir,
        Mix
    }

    public enum SubstanceName
    {
        H20,
        CACL2,
        H2PO4,
        Heptan,
        PAF,
        H2OPAF,
        Bad,
        Empty
    }
}