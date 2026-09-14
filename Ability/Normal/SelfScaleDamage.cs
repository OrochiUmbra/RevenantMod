using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Triggers;
using System.Collections;
using UnityEngine;

namespace RevenantMod;

public class SelfScaleDamage : AbilityBehaviour, IOnBellRung
{
    public static Ability ability;
    public override Ability Ability => ability;

    public bool RespondsToBellRung(bool playerCombatPhase) => Card.OpponentCard != playerCombatPhase && Card.OnBoard;

    public IEnumerator OnBellRung(bool playerCombatPhase)
    {
        yield return PreSuccessfulTriggerSequence();
        ViewManager.Instance.SwitchToView(View.Board);
        Card.Anim.StrongNegationEffect();
        yield return new WaitForSeconds(0.2f);
        yield return Singleton<LifeManager>.Instance.ShowDamageSequence(1, 1, !Card.OpponentCard, 0.125f, ResourceBank.Get<GameObject>("Prefabs/Environment/ScaleWeights/Weight_RealTooth"));
        yield return base.LearnAbility(0.5f);
    }
}

public partial class RevenantMod
{
    public void Add_Ability_SelfScaleDamage()
    {
        const string rulebookName = nameof(SelfScaleDamage);
        const string rulebookDescription = "At the end of the owner's turn, [creature] will deal 1 damage to the owner."/*"When the bell rings, [creature] will place a weight on the owner's scale."*/;

        AbilityInfo ability = AbilityManager.New(
            PluginGUID,
            "Bad Omen",
            rulebookDescription,
            typeof(SelfScaleDamage),
            Loader.LoadIcon(rulebookName)
            )
            //.SetDefaultPart1Ability()
            //.AddMetaCategories(GuidManager.GetEnumValue<AbilityMetaCategory>("arackulele.inscryption.grimoramod", "ElectricChairLevel3"))
            .SetPixelAbilityIcon(Loader.LoadIcon(rulebookName, true))
            ;
        ability.powerLevel = -3;
        SelfScaleDamage.ability = ability.ability;
    }
}
