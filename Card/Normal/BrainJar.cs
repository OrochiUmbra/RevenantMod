using DiskCardGame;
using InscryptionAPI.Card;
using Sirenix.Utilities;

namespace RevenantMod;

public partial class RevenantMod
{
	private void Add_Card_BrainJar()
	{
		const string cardName = "BrainJar";
		const string cardDescription = "One shudders to imagine what inhuman thoughts lie inside that jar...";
		const CardTemple Part2CardTemple = CardTemple.Tech;

		CardManager.New(modPrefix: PluginGUID, cardName, cardName.SplitPascalCase(), 0, 1, cardDescription)
		.SetDefaultPart1Card()
        .SetCardTemple(Part2CardTemple)
        .SetCost(bonesCost: 3, energyCost: 3)
        .SetPixelPortrait(Loader.LoadPortrait(cardName, true))
        .SetPortrait(Loader.LoadPortrait(cardName))
		.SetEmissivePortrait(Loader.LoadEmission(cardName))
		.AddAbilities(Ability.BuffNeighbours, Ability.Sentry)
		.AddTribes(Tribe.Canine)
		.AddTraits(Trait.Goat)
		.AddMetaCategories(GBCPack, GBCPlayable, GrimoraChoice, WizardRegion, TechRegion, NeutralRegion)
		;
	}
}
