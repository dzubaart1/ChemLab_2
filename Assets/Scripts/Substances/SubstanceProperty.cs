using System;

using UnityEngine;
using UnityEngine.Serialization;

namespace BioEngineerLab.Substances
{
    [CreateAssetMenu(fileName = "SubstanceProperty", menuName = "Substances/SubstanceProperty", order = 1)]
    public class SubstanceProperty : ScriptableObject
    {
        public ESubstanceLayer SubstanceLayer;
        [FormerlySerializedAs("SubstanceName")] public ESubstanceName eSubstanceName;
        [FormerlySerializedAs("SubstanceMode")] public ESubstanceMode eSubstanceMode;
        
        public string HintName;
        public Color Color;
        
        public string GetSubstanceName()
        {
            return $"{Enum.GetName(typeof(ESubstanceName), eSubstanceName)}";
        }
    }
}