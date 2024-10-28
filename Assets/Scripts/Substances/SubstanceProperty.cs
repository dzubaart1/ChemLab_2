using System;
using UnityEngine;

namespace BioEngineerLab.Substances
{
    [Serializable]
    [CreateAssetMenu(fileName = "SubstanceProperty", menuName = "SubstanceProperties/SubstanceProperty", order = 1)]
    public class SubstanceProperty : ScriptableObject
    {
        public ESubstanceName SubstanceName;
        public ESubstanceMode SubstanceMode;
        public ESubstanceLayer SubstanceLayer;
        public Color SubstanceColor;
        public string HintName;
        
        public string GetSubstanceName()
        {
            return $"{Enum.GetName(typeof(ESubstanceName), SubstanceName)}";
        }
    }
}