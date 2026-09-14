using DiskCardGame;
using InscryptionAPI.Card;
using Sirenix.Utilities;

namespace RevenantMod;

public partial class RevenantMod
{
	private void Add_Card_Guts()
	{
		const string cardName = "Guts";
		const string cardDescription = "Annoying guts.";
        const CardTemple Part2CardTemple = CardTemple.Undead;

        CardManager.New(modPrefix: PluginGUID, cardName, cardName.SplitPascalCase(), 0, 1, cardDescription)
		.SetCardTemple(Part2CardTemple)
		.SetTerrain()
        .SetPixelPortrait(Loader.LoadPortrait(cardName, true))
        .SetPortrait(Loader.LoadPortrait(cardName))
		.SetEmissivePortrait(Loader.LoadEmission(cardName))
		.AddAbilities(Ability.BuffEnemy)
		.AddTribes(Tribe.Canine)
		;
	}
}
