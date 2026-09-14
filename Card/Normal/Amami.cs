using DiskCardGame;
using InscryptionAPI.Card;

namespace RevenantMod;

public partial class RevenantMod
{
	private void Add_Card_Amami()
	{
		const string cardName = "Amami";
		const string cardDescription = "Hiding their young in holes, they wish for their best safety.";
		const CardTemple Part2CardTemple = CardTemple.Nature;

		CardManager.New(modPrefix: PluginGUID, cardName, "Amami Rabbit", 0, 1, cardDescription)
		.SetDefaultPart1Card()
        .SetCardTemple(Part2CardTemple)
        .SetCost(energyCost: 3)
        .SetPixelPortrait(Loader.LoadPortrait(cardName, true))
        .SetPortrait(Loader.LoadPortrait(cardName))
		.SetEmissivePortrait(Loader.LoadEmission(cardName))
		.AddAbilities(Maternal.ability, Ability.DrawRabbits)
		.AddTribes(Tribe.Bird)
		.AddMetaCategories(GBCPack, GBCPlayable, GrimoraChoice, NatureRegion)
		;
	}
}
