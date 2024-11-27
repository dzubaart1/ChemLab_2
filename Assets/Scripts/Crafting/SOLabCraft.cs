using BioEngineerLab.Tasks;
using UnityEngine;


namespace Crafting
{
    [CreateAssetMenu(fileName = "Craft", menuName = "Crafts/Craft", order = 1)]
    public class SOLabCraft : ScriptableObject
    {
        public LabCraft LabCraft = new LabCraft();

        public void AddFromSubstanceProperty()
        {
            LabSubstanceProperty[] newArray = new LabSubstanceProperty[LabCraft.SubstancesFrom.Length + 1];

            for (int i = 0; i < LabCraft.SubstancesFrom.Length; i++)
            {
                newArray[i] = LabCraft.SubstancesFrom[i];
            }

            newArray[^1] = new LabSubstanceProperty();

            LabCraft.SubstancesFrom = newArray;
        }

        public void DeleteFromSubstanceProperty(int id)
        {
            LabSubstanceProperty[] newArray = new LabSubstanceProperty[LabCraft.SubstancesFrom.Length - 1];

            for (int i = 0, j = 0; j < LabCraft.SubstancesFrom.Length & i < newArray.Length; j++)
            {
                if (j == id)
                {
                    newArray[i++] = LabCraft.SubstancesFrom[j];
                }
            }

            LabCraft.SubstancesFrom = newArray;
        }

        public void DeleteResSubstanceProperty(int id)
        {
            LabSubstanceProperty[] newArray = new LabSubstanceProperty[LabCraft.SubstancesRes.Length - 1];

            for (int i = 0, j = 0; j < LabCraft.SubstancesRes.Length & i < newArray.Length; j++)
            {
                if (j == id)
                {
                    newArray[i++] = LabCraft.SubstancesRes[j];
                }
            }

            LabCraft.SubstancesRes = newArray;
        }

        public void AddResSubstanceProperty()
        {
            LabSubstanceProperty[] newArray = new LabSubstanceProperty[LabCraft.SubstancesRes.Length + 1];

            for (int i = 0; i < LabCraft.SubstancesRes.Length; i++)
            {
                newArray[i] = LabCraft.SubstancesRes[i];
            }

            newArray[^1] = new LabSubstanceProperty();

            LabCraft.SubstancesRes = newArray;
        }
    }
}