using DiskCardGame;
using InscryptionAPI.Card;
using Sirenix.Utilities;

namespace RevenantMod;

public partial class RevenantMod
{
	private void Add_Card_Soulgate()
	{
		const string cardName = "Soulgate";
		const string cardDescription = "A chance to enter the soul realm, how delightful? I hope you can afford the price.";
		const CardTemple Part2CardTemple = CardTemple.Wizard;

		CardManager.New(modPrefix: PluginGUID, cardName, cardName.SplitPascalCase(), 0, 2, cardDescription)
		.SetDefaultPart1Card()
        .SetCardTemple(Part2CardTemple)
        .SetCost(energyCost: 1)
        .SetPixelPortrait(Loader.LoadPortrait(cardName, true))
        .SetPortrait(Loader.LoadPortrait(cardName))
		.SetEmissivePortrait(Loader.LoadEmission(cardName))
		.AddAbilities(ActivatedBonesToEnergy.ability)
		.AddTribes(Tribe.Bird)
		.AddMetaCategories(GBCPack, GBCPlayable, GrimoraChoice, UndeadRegion, WizardRegion, TechRegion, NeutralRegion)
		;
	}
}
