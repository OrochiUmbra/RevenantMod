using DiskCardGame;
using InscryptionAPI.Card;
using Sirenix.Utilities;

namespace RevenantMod;

public partial class RevenantMod
{
	private void Add_Card_Gregor()
	{
		const string cardName = "PrometheusFlame";
		const string cardDescription = "This one is quite unique, the lingering spoils of a thief to benefit humanity! Fancy liver by chance?";
		const CardTemple Part2CardTemple = CardTemple.Tech;

		CardManager.New(modPrefix: PluginGUID, cardName, cardName.SplitPascalCase(), 2, 1, cardDescription)
		.SetDefaultPart1Card()
        .SetCardTemple(Part2CardTemple)
        .SetCost(energyCost: 5)
        .SetPixelPortrait(Loader.LoadPortrait(cardName, true))
        .SetPortrait(Loader.LoadPortrait(cardName))
		.SetEmissivePortrait(Loader.LoadEmission(cardName))
		.AddAbilities(DrawCopySkeleton.ability, /*GrimoraModHelpers.GrimoraModIsActive() ? GuidManager.GetEnumValue<Ability>("arackulele.inscryption.grimoramod", "Random Ability") : */Ability.RandomAbility)
		.AddTribes(Tribe.Bird)
		.AddMetaCategories(GBCPack, GBCPlayable, GrimoraChoice, WizardRegion, UndeadRegion)
		;
	}
}
