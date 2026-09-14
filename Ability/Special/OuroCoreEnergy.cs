using DiskCardGame;
using InscryptionAPI.Card;
using UnityEngine;

namespace RevenantMod;

public class OuroCoreEnergy : OuroCoreBase
{
    const string PluginGUID = "orochiumbra.inscryption.revenantmod";
    const string abilityName = nameof(OuroCoreEnergy);
    public static SpecialTriggeredAbility specialAbility = SpecialTriggeredAbilityManager.Add(PluginGUID, abilityName, typeof(OuroCoreEnergy)).Id;

    public override CardInfo ModifyOuroCard(CardInfo ouroCard)
    {
        Debug.Log($"Modify Energy Cost");
        ouroCard.Mods.Add(new() { energyCostAdjustment = ouroCard.EnergyCost < 5 ? (PlayableCard.HasAbility(Ability.Brittle) ? 1 : 2) : ouroCard.EnergyCost == 5 ? 1 : 0 });
        Debug.Log($"Energy Cost : " + $"{ouroCard.EnergyCost}");
        return ouroCard;
    }
}
