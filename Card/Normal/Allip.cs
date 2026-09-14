using DiskCardGame;
using InscryptionAPI.Card;
using Sirenix.Utilities;

namespace RevenantMod;

public partial class RevenantMod
{
	private void Add_Card_Allip()
	{
		const string cardName = "Allip";
		const string cardDescription = "The remnants of the insane! You never know what story lies behind that face, but it's sure to be filled with fervor.";
		const CardTemple Part2CardTemple = CardTemple.Undead;

		CardManager.New(modPrefix: PluginGUID, cardName, cardName.SplitPascalCase(), 2, 1, cardDescription)
		.SetDefaultPart1Card()
        .SetCardTemple(Part2CardTemple)
        .SetCost(energyCost: 5)
		.SetPixelPortrait(Loader.LoadPortrait(cardName, true))
        .SetPortrait(Loader.LoadPortrait(cardName))
		.SetEmissivePortrait(Loader.LoadEmission(cardName))
		.AddAbilities(Ability.OpponentBones, Ability.Sentry)
		.AddTribes(Tribe.Bird)
		.AddMetaCategories(GBCPack, GBCPlayable, GrimoraChoice, UndeadRegion)
		;
	}
}
