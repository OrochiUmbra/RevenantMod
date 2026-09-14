using DiskCardGame;
using InscryptionAPI.Card;

namespace RevenantMod;

public partial class RevenantMod
{
	private void Add_Card_Ourobones()
	{
		// This builds our card information.
		CardInfo Ourobones = CardManager.New(

			// Card ID Prefix
			modPrefix: PluginGUID,

			// Card internal name.
			"Ourobones",

			// Card display name.
			"Ourobones",

			// Attack.
			1,

			// Health.
			1,

			// Descryption.
			description: "An endless cycle of life and death has taken a toll on these discarded remains. The pain was real, but death is never final!"

		)
		.SetRare()

        .SetCardTemple(CardTemple.Undead)

        .SetCost(bonesCost: 2)

		.AddAbilities(Ability.Brittle)

		.AddSpecialAbilities(OuroCoreBones.specialAbility)

        .SetPixelPortrait(Loader.LoadTexture("pixelportrait_ourobones.png"))
        .SetPortrait(Loader.LoadTexture("portrait_ourobones.png"))
		.SetEmissivePortrait(Loader.LoadTexture("portrait_ourobones_emission.png"))

		.AddTribes(Tribe.Reptile)

		.AddMetaCategories(GBCPack, GBCPlayable, GrimoraChoice, NeutralRegion)
		;

		// Pass the card to the API.
		CardManager.Add(PluginGUID, Ourobones);
	}
}
