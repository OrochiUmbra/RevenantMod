using DiskCardGame;
using InscryptionAPI.Card;

namespace RevenantMod;

public partial class RevenantMod
{
	private void Add_Card_Imp()
	{
		const string cardName = "Imp";
		const string cardDescription = "A tiny creature known for causing trouble and mayhem wherever it goes, with its mischievous grin and playful nature!";
		const CardTemple Part2CardTemple = CardTemple.Undead;

		CardManager.New(modPrefix: PluginGUID, cardName, "Common Imp", 2, 1, cardDescription)
		.SetDefaultPart1Card()
        .SetCardTemple(Part2CardTemple)
        .SetCost(bonesCost: 5)
        .SetPixelPortrait(Loader.LoadPortrait(cardName, true))
        .SetPortrait(Loader.LoadPortrait(cardName))
		.SetEmissivePortrait(Loader.LoadEmission(cardName))
		.AddAbilities(Ability.DrawRandomCardOnDeath, Ability.MoveBeside)
		.AddTribes(Tribe.Hooved)
		.AddMetaCategories(GBCPack, GBCPlayable, GrimoraChoice, WizardRegion, NatureRegion)
		;
	}
}
