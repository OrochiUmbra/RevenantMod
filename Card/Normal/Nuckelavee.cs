using DiskCardGame;
using InscryptionAPI.Card;
using Sirenix.Utilities;

namespace RevenantMod;

public partial class RevenantMod
{
	private void Add_Card_Nuckelavee()
	{
		const string cardName = "Nuckelavee";
		const string cardDescription = "The Administrator of its sea, you better hope it's breath doesn't taint your Thralls!";
		const CardTemple Part2CardTemple = CardTemple.Undead;

		CardManager.New(modPrefix: PluginGUID, cardName, cardName.SplitPascalCase(), 1, 1, cardDescription)
		.SetDefaultPart1Card()
        .SetCardTemple(Part2CardTemple)
        .SetCost(bonesCost: 5)
        .SetPixelPortrait(Loader.LoadPortrait(cardName, true))
        .SetPortrait(Loader.LoadPortrait(cardName))
		.SetEmissivePortrait(Loader.LoadEmission(cardName))
		.AddAbilities(Ability.SkeletonStrafe, Ability.Sentry)
		.AddTribes(Tribe.Hooved)
		.AddMetaCategories(GBCPack, GBCPlayable, GrimoraChoice, UndeadRegion, WizardRegion)
		;
	}
}
