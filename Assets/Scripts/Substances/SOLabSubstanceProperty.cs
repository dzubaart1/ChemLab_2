using System;
using UnityEngine;

namespace Substances
{
    [Serializable]
    [CreateAssetMenu(fileName = "SubstanceProperty", menuName = "SubstanceProperties/SubstanceProperty", order = 1)]
    public class SOLabSubstanceProperty : ScriptableObject
    {
        public LabSubstanceProperty LabSubstanceProperty = new LabSubstanceProperty();
    }
}