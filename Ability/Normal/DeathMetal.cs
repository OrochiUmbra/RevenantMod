using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Guid;
using InscryptionAPI.Helpers.Extensions;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RevenantMod;

public class DeathMetal : AbilityBehaviour
{
    public static Ability ability;
    public override Ability Ability => ability;

    public List<CardSlot> GetAoESlots(CardSlot cardSlot)
    {
        List<CardSlot> slotsAoE = new();
        if (cardSlot.opposingSlot != null)
            slotsAoE.Add(cardSlot.opposingSlot);
        slotsAoE.AddRange(cardSlot.GetAdjacentSlots(true));
        slotsAoE.AddRange(cardSlot.opposingSlot.GetAdjacentSlots(true));
        slotsAoE.Sort((slot1, slot2) => slot1.Index - slot2.Index);
        return slotsAoE;
    }

    public override bool RespondsToResolveOnBoard() => true;

    public override IEnumerator OnResolveOnBoard()
    {
        yield return PreSuccessfulTriggerSequence();
        Card.Anim.StrongNegationEffect();
        int num = 0;
        foreach (CardSlot slot in GetAoESlots(Card.Slot).Where(x => x.Card != null && !x.Card.Dead))
        {
            slot.Card.Anim.StrongNegationEffect();
            yield return slot.Card.TakeDamage(1, Card);
            num++;
        }
        if (!Card.Dead)
        {
            Card.Anim.StrongNegationEffect();
            Card.Anim.PlayTransformAnimation();
            Card.AddTemporaryMod(new CardModificationInfo(num, 0));
            yield return new WaitForSeconds(0.5f);
        }
        yield return base.LearnAbility(0.5f);
    }
}

public partial class RevenantMod
{
    public void Add_Ability_DeathMetal()
    {
        const string rulebookName = nameof(DeathMetal);
        const string rulebookDescription = "When [creature] is played, adjacent creatures and opposing creatures to the left, right, and center of it take 1 damage. "
                                         + "It gains 1 Power for each affected creature.";

        AbilityInfo ability = AbilityManager.New(
            PluginGUID,
            rulebookName.SplitPascalCase(),
            rulebookDescription,
            typeof(DeathMetal),
            AbilitiesUtil.LoadAbilityIcon(Ability.Apparition.ToString())
            )
            .SetDefaultPart1Ability()
            .AddMetaCategories(GuidManager.GetEnumValue<AbilityMetaCategory>("arackulele.inscryption.grimoramod", "ElectricChairLevel3"))
            .SetPixelAbilityIcon(Loader.LoadIcon(rulebookName, true))
            ;
        ability.powerLevel = 4;
        DeathMetal.ability = ability.ability;
    }
}
