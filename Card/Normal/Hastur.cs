using DiskCardGame;
using InscryptionAPI.Card;
using Sirenix.Utilities;

namespace RevenantMod;

public partial class RevenantMod
{
	private void Add_Card_Hastur()
	{
		const string cardName = "Hastur";
		const string cardDescription = "He came from beyond the stars, gifting pieces of his reflection to kings and fools alike. Will you accept this piece?";
		const CardTemple Part2CardTemple = CardTemple.Wizard;

		CardManager.New(modPrefix: PluginGUID, cardName, cardName.SplitPascalCase(), 0, 3, cardDescription)
		.SetDefaultPart1Card()
        .SetCardTemple(Part2CardTemple)
        .SetCost(bonesCost: 5)
        .SetPixelPortrait(Loader.LoadPortrait(cardName, true))
        .SetPortrait(Loader.LoadPortrait(cardName))
		.SetEmissivePortrait(Loader.LoadEmission(cardName))
		.AddAbilities(Ability.GainAttackOnKill)
		.AddTribes(Tribe.Hooved)
		.SetSpecialAbilities(SpecialTriggeredAbility.Mirror)
		.SetStatIcon(SpecialStatIcon.Mirror)
		.AddMetaCategories(GBCPack, GBCPlayable, GrimoraChoice, UndeadRegion, NatureRegion, WizardRegion)
		;
	}
}
