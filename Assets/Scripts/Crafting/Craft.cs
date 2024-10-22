using BioEngineerLab;
using BioEngineerLab.Substances;
using UnityEngine;
using Container = BioEngineerLab.Containers.Container;

[CreateAssetMenu(fileName = "Craft", menuName = "Crafts/Craft", order = 1)]
public class Craft : ScriptableObject
{
    public SubstanceProperty[] SubstancesFrom = new SubstanceProperty[Container.MAX_SUBSTANCE_COUNT];
    public ECraft CraftType;
    public SubstanceProperty[] SubstancesRes = new SubstanceProperty[Container.MAX_SUBSTANCE_COUNT]; 
}
