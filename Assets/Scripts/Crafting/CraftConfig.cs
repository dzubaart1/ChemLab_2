using BioEngineerLab;
using BioEngineerLab.Substances;
using UnityEngine;
using Container = BioEngineerLab.Containers.Container;

[CreateAssetMenu(fileName = "Craft", menuName = "Crafts/Craft", order = 1)]
public class CraftConfig : ScriptableObject
{
    public Substance[] SubstancesFrom = new Substance[Container.MAX_SUBSTANCE_COUNT];
    public ECraft CraftType;
    public Substance[] SubstancesTo = new Substance[Container.MAX_SUBSTANCE_COUNT]; 
}
