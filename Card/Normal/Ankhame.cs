using DiskCardGame;
using InscryptionAPI.Card;
using Sirenix.Utilities;
using UnityEngine;

namespace RevenantMod;

public partial class RevenantMod
{
	private void Add_Card_Ankhame()
	{
		const string cardName = "Ankhame";
		const string cardDescription = "A relic that is drawn to your blood, perhaps you could give it a swing!";
		const CardTemple Part2CardTemple = CardTemple.Wizard;

		CardManager.New(modPrefix: PluginGUID, cardName, cardName.SplitPascalCase(), 0, 2, cardDescription)
		.SetDefaultPart1Card()
        .SetCardTemple(Part2CardTemple)
        .SetCost(bonesCost: 3)
        .SetPixelPortrait(ResourceBank.Get<Texture2D>("Art/GBC/Cards/PixelPortraits/pixelportrait_cat_undead"))
        .SetPortrait(Loader.LoadPortrait(cardName))
        .SetEmissivePortrait(Loader.LoadEmission(cardName))
        .AddAbilities(ActivatedBloodDrawMain.ability, Ability.Sharp)
		.AddTribes(Tribe.Insect, Tribe.Bird)
        .AddTraits(Trait.KillsSurvivors, Trait.Goat)
        .AddMetaCategories(GBCPack, GBCPlayable, GrimoraChoice, UndeadRegion, WizardRegion, NeutralRegion)
		;
	}
}