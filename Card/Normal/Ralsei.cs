using DiskCardGame;
using InscryptionAPI.Card;

namespace RevenantMod;

public partial class RevenantMod
{
	private void Add_Card_Ralsei()
	{
		const string cardName = "Ralsei";
		const string cardDescription = "The power of fluffy bois shines within you.";
		const CardTemple Part2CardTemple = CardTemple.Wizard;

		CardManager.New(modPrefix: PluginGUID, cardName, "Dark Prince", 0, 1, cardDescription)
		.SetDefaultPart1Card()
        .SetCardTemple(Part2CardTemple)
        .SetCost(energyCost: 4)
        .SetPixelPortrait(Loader.LoadPortrait(cardName, true))
        .SetPortrait(Loader.LoadPortrait(cardName))
		.SetEmissivePortrait(Loader.LoadEmission(cardName))
		.AddAbilities(ComfortAura.ability, BuffNeighboursHealth.ability)
		.AddTribes(Tribe.Hooved)
        .AddTraits(Trait.Goat)
        .AddMetaCategories(GBCPack, GBCPlayable, GrimoraChoice, WizardRegion, NatureRegion)
		;
	}
}
