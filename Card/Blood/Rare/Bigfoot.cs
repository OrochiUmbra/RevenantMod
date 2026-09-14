using DiskCardGame;
using InscryptionAPI.Card;

namespace RevenantMod;

public partial class RevenantMod
{
	private void Add_Blood_Card_Bigfoot()
	{
		CardManager.New(modPrefix: PluginGUID, "Bigfoot", "Bigfoot", 4, 6, "a mysterious creature lurking the forests, only few have claimed to see one.")
		.AddAppearances(CardAppearanceBehaviour.Appearance.RareCardBackground)
		//.SetRare()
		.SetCost(bloodCost: 4)
		.SetPortrait(Loader.LoadTexture("Bigfoot.png"))
		.SetEmissivePortrait(Loader.LoadTexture("Bigfoot_emission.png"))
		.AddAbilities(Ability.DoubleStrike)
		;
	}
}
