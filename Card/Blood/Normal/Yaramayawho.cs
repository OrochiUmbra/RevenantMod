using DiskCardGame;
using InscryptionAPI.Card;

namespace RevenantMod;

public partial class RevenantMod
{
	private void Add_Blood_Card_Yaramayawho()
	{
		CardManager.New(modPrefix: PluginGUID, "Yaramayawho", "Yaramayawho", 2, 1, "This one can never seem to finish it's meals...")
		//.SetDefaultPart1Card()
		.SetCost(bloodCost: 2)
		.SetPortrait(Loader.LoadTexture("Yaramayawho.png"))
		.SetEmissivePortrait(Loader.LoadTexture("Yaramayawho_emission.png"))
		.AddAbilities(Ability.CreateEgg, Ability.Flying)
		;
	}
}
