using System;
using BioEngineerLab.Tasks;
using UnityEngine;

namespace BioEngineerLab.Substances
{
    [Serializable]
    [CreateAssetMenu(fileName = "SubstanceProperty", menuName = "SubstanceProperties/SubstanceProperty", order = 1)]
    public class SOLabSubstanceProperty : ScriptableObject
    {
        public LabSubstanceProperty LabSubstanceProperty = new LabSubstanceProperty();
    }
}