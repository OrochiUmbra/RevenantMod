using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Guid;
using Sirenix.Utilities;
using System.Collections;
using UnityEngine;

namespace RevenantMod;

public class DeathKnock : AbilityBehaviour
{
    public static Ability ability;
    public override Ability Ability => ability;

    public override bool RespondsToOtherCardPreDeath(CardSlot deathSlot, bool fromCombat, PlayableCard killer)
    {
        if (deathSlot.Card == Card)
            return killer != null && killer.NotDead() && killer.OpponentCard != Card.OpponentCard;

        return false;
    }

    public override IEnumerator OnOtherCardPreDeath(CardSlot deathSlot, bool fromCombat, PlayableCard killer)
    {
        yield return base.PreSuccessfulTriggerSequence();

        // copied straight from Daus special ability - attacks the killer after temporarily removing flying to avoid direct damage
        yield return new WaitForSeconds(0.25f);
        base.Card.Anim.StrongNegationEffect();
        yield return new WaitForSeconds(0.25f);
        CardModificationInfo removeFlyingMod = null;
        if (base.Card.HasAbility(Ability.Flying))
        {
            removeFlyingMod = new CardModificationInfo
            {
                negateAbilities = { Ability.Flying }
            };
            base.Card.AddTemporaryMod(removeFlyingMod);
        }
        yield return Singleton<TurnManager>.Instance.CombatPhaseManager.SlotAttackSlot(base.Card.Slot, killer.Slot);
        if (removeFlyingMod != null)
        {
            base.Card.RemoveTemporaryMod(removeFlyingMod);
        }
        yield return new WaitForSeconds(0.25f);

        // this is just the SlotAttackSequence code, minus some aspects
        /*yield return new WaitForSeconds(0.025f);
        if (base.Card.Anim.DoingAttackAnimation)
        {
            yield return new WaitUntil(() => !base.Card.Anim.DoingAttackAnimation);
            yield return new WaitForSeconds(0.25f);
        }

        // if attack is blocked (removed null check since that's already been done)
        if (base.Card.AttackIsBlocked(killer.Slot))
        {
            ProgressionData.SetAbilityLearned(Ability.PreventAttack);
            yield return Singleton<CombatPhaseManager>.Instance.ShowCardBlocked(base.Card);
        }
        else // regular attack
        {
            float heightOffset = (killer == null) ? 0f : killer.SlotHeightOffset;
            if (heightOffset > 0f)
                Tween.Position(base.Card.transform, base.Card.transform.position + Vector3.up * heightOffset, 0.05f, 0f, Tween.EaseInOut);

            base.Card.Anim.PlayAttackAnimation(base.Card.IsFlyingAttackingReach(), killer.Slot, null);
            yield return new WaitForSeconds(0.07f);
            base.Card.Anim.SetAnimationPaused(true);
            PlayableCard attackingCard = base.Card;
            yield return Singleton<GlobalTriggerHandler>.Instance.TriggerCardsOnBoard(Trigger.CardGettingAttacked, false, killer);
            if (attackingCard?.Slot != null && killer?.Slot != null)
            {
                base.Card.Slot = attackingCard.Slot;
                if (base.Card.IsFlyingAttackingReach())
                {
                    killer.Anim.PlayJumpAnimation();
                    yield return new WaitForSeconds(0.3f);
                    base.Card.Anim.PlayAttackInAirAnimation();
                }
                base.Card.Anim.SetAnimationPaused(false);
                yield return new WaitForSeconds(0.05f);
                int overkillDamage = base.Card.Attack - killer.Health;
                yield return killer.TakeDamage(base.Card.Attack, base.Card);
                yield return Singleton<CombatPhaseManager>.Instance.DealOverkillDamage(overkillDamage, base.Card.Slot, killer.Slot);
                if (base.Card != null && heightOffset > 0f)
                    yield return Singleton<BoardManager>.Instance.AssignCardToSlot(base.Card, base.Card.Slot, 0.1f, null, resolveTriggers: false);
            }
        }*/
        yield return base.LearnAbility(0.5f);
    }
}

public partial class RevenantMod
{
    public void Add_Ability_DeathKnock()
    {
        const string rulebookName = nameof(DeathKnock);
        const string rulebookDescription = "When [creature] perishes by combat, it strikes back then perishes.";

        AbilityInfo ability = AbilityManager.New(
            PluginGUID,
            rulebookName.SplitPascalCase(),
            rulebookDescription,
            typeof(DeathKnock),
            Loader.LoadIcon(rulebookName)
            )
            .SetDefaultPart1Ability()
            .AddMetaCategories(GuidManager.GetEnumValue<AbilityMetaCategory>("arackulele.inscryption.grimoramod", "ElectricChairLevel2"))
            .SetPixelAbilityIcon(Loader.LoadIcon(rulebookName, true))
            ;
        ability.powerLevel = 2;
        DeathKnock.ability = ability.ability;
    }
}
