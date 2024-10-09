using System;
using UnityEngine;

namespace BioEngineerLab.Substances
{
    [CreateAssetMenu(fileName = "SubstanceProperty", menuName = "Substances/SubstanceProperty", order = 1)]
    public class SubstanceProperty : ScriptableObject
    {
        public ESubstanceName SubstanceName;
        public ESubstanceMode SubstanceMode;
        public ESubstanceLayer SubstanceLayer;
        public ESubstanceColor SubstanceColor;
        public string HintName;
        
        public string GetSubstanceName()
        {
            return $"{Enum.GetName(typeof(ESubstanceName), SubstanceName)}";
        }
    }
}