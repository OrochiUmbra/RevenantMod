using InscryptionAPI.Card;

namespace RevenantMod;

public partial class RevenantMod
{
	private void Add_Blood_Card_Goblin()
	{
		CardManager.New(modPrefix: PluginGUID, "Goblin", "Goblin", 2, 1, "A loyal barbaric beast, a fine addition to your army.")
		//.SetDefaultPart1Card()
		.SetCost(bloodCost: 1)
		.SetPortrait(Loader.LoadTexture("Goblin.png"))
		.SetEmissivePortrait(Loader.LoadTexture("Goblin_emission.png"))
		;
	}
}
