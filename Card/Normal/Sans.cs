using DiskCardGame;
using InscryptionAPI.Card;

namespace RevenantMod;

public partial class RevenantMod
{
	private void Add_Card_Sans()
	{
		const string cardName = "Sans";
		const string cardDescription = "A slothful skeleton with a knack for jokes. I adore him.";
		const CardTemple Part2CardTemple = CardTemple.Undead;

		CardManager.New(modPrefix: PluginGUID, cardName, "Lazy Bones", 3, 3, cardDescription)
		.SetDefaultPart1Card()
        .SetCardTemple(Part2CardTemple)
        .SetCost(bonesCost: 2)
        .SetPixelPortrait(Loader.LoadPortrait(cardName, true))
        .SetPortrait(Loader.LoadPortrait(cardName))
		.SetEmissivePortrait(Loader.LoadEmission(cardName))
		.AddAbilities(Torpid.ability, DeathKnock.ability)
		.AddTribes(Tribe.Reptile)
		.AddMetaCategories(GBCPack, GBCPlayable, GrimoraChoice, WizardRegion, TechRegion)
		;
	}
}
