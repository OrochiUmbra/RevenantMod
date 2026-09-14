using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Guid;

namespace RevenantMod;

public class LatchEvolve : Latch
{
    public static Ability ability;
    public override Ability Ability => ability;
    public override Ability LatchAbility => /*GrimoraModHelpers.GrimoraModIsActive() ? GuidManager.GetEnumValue<Ability>("arackulele.inscryption.grimoramod", "Fledgling") : */Ability.Evolve;
}

public partial class RevenantMod
{
    public void Add_Ability_LatchEvolve()
    {
        const string rulebookName = nameof(LatchEvolve);
        const string rulebookDescription = "When [creature] perishes, its owner chooses a creature to gain the Fledgling sigil.";

        AbilityInfo ability = AbilityManager.New(
            PluginGUID,
            "Rejuvenation",
            rulebookDescription,
            typeof(LatchEvolve),
            Loader.LoadIcon(rulebookName)
            )
            .SetDefaultPart1Ability()
            .AddMetaCategories(GuidManager.GetEnumValue<AbilityMetaCategory>("arackulele.inscryption.grimoramod", "ElectricChairLevel1"))
            .SetPixelAbilityIcon(Loader.LoadIcon(rulebookName, true))
            ;
        ability.powerLevel = 2;
        LatchEvolve.ability = ability.ability;
    }
}
