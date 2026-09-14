using DiskCardGame;
using InscryptionAPI.Card;

namespace RevenantMod;

public partial class RevenantMod
{
	private void Add_Card_DeadStoat()
	{
		const string cardName = "DeadStoat";
		const string cardDescription = "A decaying Stoat whose oh-so perfect strategies failed him tremendously.";
		const CardTemple Part2CardTemple = CardTemple.Nature;

		CardManager.New(modPrefix: PluginGUID, cardName, "Misplayed Stoat", 1, 3, cardDescription)
		.SetDefaultPart1Card()
        .SetCardTemple(Part2CardTemple)
        .SetCost(energyCost: 6)
        .SetPixelPortrait(Loader.LoadPortrait(cardName, true))
        .SetPortrait(Loader.LoadPortrait(cardName))
		.SetEmissivePortrait(Loader.LoadEmission(cardName))
		.AddAbilities(Ability.Deathtouch)
		.AddTribes(Tribe.Reptile)
		.AddTraits(Trait.KillsSurvivors)
		.AddMetaCategories(GBCPack, GBCPlayable, GrimoraChoice, TechRegion, NatureRegion)
		;
	}
}
