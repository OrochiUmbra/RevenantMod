using DiskCardGame;
using InscryptionAPI.Card;
using Sirenix.Utilities;

namespace RevenantMod;

public partial class RevenantMod
{
	private void Add_Card_Wendigo()
	{
		const string cardName = "Wendigo";
		const string cardDescription = "Returned from the deep wilds, reminds me of a friend.";
		const CardTemple Part2CardTemple = CardTemple.Nature;

		CardManager.New(modPrefix: PluginGUID, cardName, cardName.SplitPascalCase(), 1, 2, cardDescription)
		.SetDefaultPart1Card()
        .SetCardTemple(Part2CardTemple)
        .SetCost(bonesCost: 5)
        .SetPixelPortrait(Loader.LoadPortrait(cardName, true))
        .SetPortrait(Loader.LoadPortrait(cardName))
		.SetEmissivePortrait(Loader.LoadEmission(cardName))
		.AddAbilities(Ability.DebuffEnemy, Ability.Strafe)
		.AddTribes(Tribe.Hooved)
		.AddMetaCategories(GBCPack, GBCPlayable, GrimoraChoice, UndeadRegion, NatureRegion)
		;
	}
}
