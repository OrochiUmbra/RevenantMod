using DiskCardGame;
using InscryptionAPI.Card;
using UnityEngine;

namespace RevenantMod;

public partial class RevenantMod
{
	private void Add_Card_FrankHybrid()
	{
		const string cardName = "SteinNFrank";
		const string cardDescription = "Even hell was not hot enough to split them.";
		const CardTemple Part2CardTemple = CardTemple.Undead;

		CardManager.New(modPrefix: PluginGUID, cardName, "Stein & Frank", 2, 2, cardDescription)
		.SetDefaultPart1Card()
        .SetCardTemple(Part2CardTemple)
        .SetCost(bonesCost: 3, energyCost: 3)
        .SetPixelPortrait(ResourceBank.Get<Texture2D>("Art/GBC/Cards/PixelPortraits/pixelportrait_franknstein"))
        .SetPortrait(Loader.LoadPortrait(cardName))
		.SetEmissivePortrait(Loader.LoadEmission(cardName))
		.AddTribes(Tribe.Reptile)
		.AddTraits(Trait.Fused)
		.AddMetaCategories(GBCPack, GBCPlayable, GrimoraChoice, UndeadRegion, WizardRegion, TechRegion, NeutralRegion)
		;
	}
}
