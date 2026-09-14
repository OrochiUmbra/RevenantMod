using DiskCardGame;
using InscryptionAPI.Card;
using System.Collections;
using UnityEngine;

namespace RevenantMod;

public class OuroCoreBones : OuroCoreBase, IOnCardAddedToGraveyard
{
    const string PluginGUID = "orochiumbra.inscryption.revenantmod";
    const string abilityName = nameof(OuroCoreBones);
    public static SpecialTriggeredAbility specialAbility = SpecialTriggeredAbilityManager.Add(PluginGUID, abilityName, typeof(OuroCoreBones)).Id;

    public bool RespondsToAddDeadCardToPile(CardInfo deadCard, bool wasSacrifice, bool opponentCard, CardInfo killer)
    {
        return deadCard.name == base.PlayableCard.Info.name;
    }
    public IEnumerator OnAddDeadCardToPile(CardInfo deadCard, bool wasSacrifice, bool opponentCard, CardInfo killer)
    {
        Debug.Log($"Ourobones drawn");
        yield return Singleton<GraveyardManager>.Instance.DrawCard(deadCard);
    }

    public override CardInfo ModifyOuroCard(CardInfo ouroCard)
    {
        Debug.Log($"Modify Bone Cost");
        ouroCard.Mods.Add(new() { bonesCostAdjustment = PlayableCard.HasAbility(Ability.Brittle) ? 1 : 2 });
        Debug.Log($"Bone Cost : " + $"{ouroCard.BonesCost}");
        return ouroCard;
    }
}
