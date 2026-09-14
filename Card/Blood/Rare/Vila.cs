using DiskCardGame;
using InscryptionAPI.Card;

namespace RevenantMod;

public partial class RevenantMod
{
	private void Add_Blood_Card_Vila()
	{
		CardManager.New(modPrefix: PluginGUID, "Vila", "Vila", 0, 1, "Love at first sight, at least that's what it wants you to think.")
		.AddAppearances(CardAppearanceBehaviour.Appearance.RareCardBackground)
		//.SetRare()
		.SetCost(bloodCost: 1)
		.SetPortrait(Loader.LoadTexture("Vila.png"))
		.SetEmissivePortrait(Loader.LoadTexture("Vila_emission.png"))
		.AddAbilities(//GuidManager.GetEnumValue<Ability>("arackulele.inscryption.grimoramod", "Hook Line And Sinker"),
		Ignition.ability)
		;
	}
}
