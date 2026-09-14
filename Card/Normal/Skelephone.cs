using DiskCardGame;
using InscryptionAPI.Card;
using Sirenix.Utilities;

namespace RevenantMod;

public partial class RevenantMod
{
	private void Add_Card_Skelephone()
	{
		const string cardName = "Skelephone";
		const string cardDescription = "A way to communicate with the dead, for a price.";
		const CardTemple Part2CardTemple = CardTemple.Tech;

		CardManager.New(modPrefix: PluginGUID, cardName, cardName.SplitPascalCase(), 1, 1, cardDescription)
		.SetDefaultPart1Card()
        .SetCardTemple(Part2CardTemple)
        .SetCost(energyCost: 4)
        .SetPixelPortrait(Loader.LoadPortrait(cardName, true))
        .SetPortrait(Loader.LoadPortrait(cardName))
		.SetEmissivePortrait(Loader.LoadEmission(cardName))
		.AddAbilities(TutorDead.ability, Defiler.ability)
		.AddTribes(Tribe.Insect)
		.AddMetaCategories(GrimoraChoice, WizardRegion, TechRegion)
		;
	}
}
