using DiskCardGame;
using InscryptionAPI.Card;
using Sirenix.Utilities;

namespace RevenantMod;

public partial class RevenantMod
{
	private void Add_Card_Undertaker()
	{
		const string cardName = "Undertaker";
		const string cardDescription = "In every town, in every city, someone must work to bury the dead. In time, they too will be buried.";
		const CardTemple Part2CardTemple = CardTemple.Undead;

		CardManager.New(modPrefix: PluginGUID, cardName, cardName.SplitPascalCase(), 0, 1, cardDescription)
		.SetDefaultPart1Card()
        .SetCardTemple(Part2CardTemple)
        .SetCost(bonesCost: 3)
        .SetPixelPortrait(Loader.LoadPortrait(cardName, true))
        .SetPortrait(Loader.LoadPortrait(cardName))
		.SetEmissivePortrait(Loader.LoadEmission(cardName))
		.AddAbilities(LatchDigger.ability)
		.AddTribes(Tribe.Canine)
		.AddMetaCategories(GBCPack, GBCPlayable, GrimoraChoice, UndeadRegion, NatureRegion, NeutralRegion)
		;
	}
}