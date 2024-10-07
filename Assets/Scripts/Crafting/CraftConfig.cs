using BioEngineerLab;
using BioEngineerLab.Substances;
using UnityEngine;

[CreateAssetMenu(fileName = "Craft", menuName = "Crafts/Craft", order = 1)]
public class CraftConfig : ScriptableObject
{
    public ECraft CraftType;
    public Substance[] SubstancesFrom;
    public Substance[] SubstancesTo;
}
