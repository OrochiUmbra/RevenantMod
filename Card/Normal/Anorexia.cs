using DiskCardGame;
using InscryptionAPI.Card;
using Sirenix.Utilities;

namespace RevenantMod;

public partial class RevenantMod
{
	private void Add_Card_Anorexia()
	{
		const string cardName = "Anorexia";
		const string cardDescription = "Overflowing you with self loathing, you never know what form it may take on next.";
        const CardTemple Part2CardTemple = CardTemple.Nature;

		CardManager.New(modPrefix: PluginGUID, cardName, cardName.SplitPascalCase(), 2, 1, cardDescription)
		.SetDefaultPart1Card()
        .SetCardTemple(Part2CardTemple)
        .SetCost(energyCost: 3)
        .SetPixelPortrait(Loader.LoadPortrait(cardName, true))
        .SetPortrait(Loader.LoadPortrait(cardName))
		.SetEmissivePortrait(Loader.LoadEmission(cardName))
		.AddAbilities(SplittingCorpse.ability, Necrosis.ability)
		.AddTribes(Tribe.Canine)
		.AddMetaCategories(GBCPack, GBCPlayable, GrimoraChoice, UndeadRegion, NatureRegion)
		;
	}
}
