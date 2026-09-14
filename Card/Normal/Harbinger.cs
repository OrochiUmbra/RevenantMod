using DiskCardGame;
using InscryptionAPI.Card;
using Sirenix.Utilities;

namespace RevenantMod;

public partial class RevenantMod
{
	private void Add_Card_Harbinger()
	{
		const string cardName = "Harbinger";
		const string cardDescription = "A powerful fiend that extols a heavy price for its servitude.";
		const CardTemple Part2CardTemple = CardTemple.Undead;

		CardManager.New(modPrefix: PluginGUID, cardName, cardName.SplitPascalCase(), 2, 2, cardDescription)
		.SetDefaultPart1Card()
        .SetCardTemple(Part2CardTemple)
        .SetCost(energyCost: 3)
        .SetPixelPortrait(Loader.LoadPortrait(cardName, true))
        .SetPortrait(Loader.LoadPortrait(cardName))
		.SetEmissivePortrait(Loader.LoadEmission(cardName))
		.AddAbilities(SelfScaleDamage.ability)
		.AddTribes(Tribe.Reptile)
		.AddMetaCategories(GBCPack, GBCPlayable, GrimoraChoice, UndeadRegion, WizardRegion, NeutralRegion)
		;
	}
}
