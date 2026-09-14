using DiskCardGame;
using InscryptionAPI.Card;
using Sirenix.Utilities;

namespace RevenantMod;

public partial class RevenantMod
{
	private void Add_Card_Combustor()
	{
		const string cardName = "Combustor";
		const string cardDescription = "Oh dear, what a fall from such a stimulatingly high status, now all that's left is a metal carcass, and the last of that watchful substance...";
		const CardTemple Part2CardTemple = CardTemple.Tech;

		CardManager.New(modPrefix: PluginGUID, cardName, cardName.SplitPascalCase(), 0, 5, cardDescription)
		.SetDefaultPart1Card()
        .SetCardTemple(Part2CardTemple)
        .SetCost(bonesCost: 3)
        .SetPixelPortrait(Loader.LoadPortrait(cardName, true))
        .SetPortrait(Loader.LoadPortrait(cardName))
		.SetEmissivePortrait(Loader.LoadEmission(cardName))
		.AddAbilities(Ignition.ability)
		.AddTribes(Tribe.Insect)
		.AddSpecialAbilities(OrochiRitual.specialAbility)
		.AddMetaCategories(GBCPack, GBCPlayable, GrimoraChoice, WizardRegion, NeutralRegion)
		;
	}
}
