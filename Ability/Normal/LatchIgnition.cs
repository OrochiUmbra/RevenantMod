using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Guid;

namespace RevenantMod;

public class LatchIgnition : Latch
{
    public static Ability ability;
    public override Ability Ability => ability;
    public override Ability LatchAbility => Ignition.ability;
}

public partial class RevenantMod
{
    public void Add_Ability_LatchIgnition()
    {
        const string rulebookName = nameof(LatchIgnition);
        const string rulebookDescription = "When [creature] perishes, its owner chooses a creature to gain the Ignition sigil.";

        AbilityInfo ability = AbilityManager.New(
            PluginGUID,
            "Ignite",
            rulebookDescription,
            typeof(LatchIgnition),
            Loader.LoadIcon(rulebookName)
            )
            .SetDefaultPart1Ability()
            .AddMetaCategories(GuidManager.GetEnumValue<AbilityMetaCategory>("arackulele.inscryption.grimoramod", "ElectricChairLevel2"))
            .SetPixelAbilityIcon(Loader.LoadIcon(rulebookName, true))
            ;
        ability.powerLevel = 1;
        LatchIgnition.ability = ability.ability;
    }
}
