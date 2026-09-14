using DiskCardGame;
using InscryptionAPI.Card;

namespace RevenantMod;

public partial class RevenantMod
{
	private void Add_Card_Vampbones()
	{
		const string cardName = "DrBones";
		const string cardDescription = "Whatever doesn't kill you simply makes you, saner.";
		const CardTemple Part2CardTemple = CardTemple.Undead;

		CardManager.New(modPrefix: PluginGUID, cardName, "Dr.Bones", 2, 4, cardDescription)
		.SetDefaultPart1Card()
        .SetCardTemple(Part2CardTemple)
        .SetCost(bonesCost: 8)
        .SetPixelPortrait(Loader.LoadPortrait(cardName, true))
        .SetPortrait(Loader.LoadPortrait(cardName))
		.SetEmissivePortrait(Loader.LoadEmission(cardName))
		.AddAbilities(Ability.ActivatedHeal, Ability.Sharp)
		.AddTribes(Tribe.Reptile)
		.AddMetaCategories(GBCPack, GBCPlayable, GrimoraChoice, UndeadRegion, WizardRegion)
		;
	}
}
