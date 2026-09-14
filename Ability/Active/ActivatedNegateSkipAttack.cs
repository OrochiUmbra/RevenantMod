using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Guid;
using InscryptionAPI.Triggers;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;

namespace RevenantMod;

public class ActivatedNegateSkipAttack : ActivatedAbilityBehaviour, IGetAttackingSlots
{
    public static Ability ability;
    public override Ability Ability => ability;
    public override int BonesCost => Card.Attack;

    //private const int BONES_COST = 1;

    private bool active = false; // this doesn't need to be static

    public override bool CanActivate() => !active;

    public override IEnumerator Activate()
    {
        yield return base.PreSuccessfulTriggerSequence();
        active = true;
        if (Singleton<BoardManager>.Instance is BoardManager3D)
            base.Card.RenderInfo.OverrideAbilityIcon(ability, Loader.LoadIcon("dormant_active"));
        base.Card.RenderCard();
    }

    public override bool RespondsToUpkeep(bool playerUpkeep) => active && Card.OpponentCard != playerUpkeep && !Card.Dead;

    public override IEnumerator OnUpkeep(bool playerUpkeep)
    {
        if (Singleton<BoardManager>.Instance is BoardManager3D)
            base.Card.RenderInfo.OverrideAbilityIcon(ability, Loader.LoadIcon("dormant"));
        base.Card.RenderCard();
        active = false;
        yield return base.LearnAbility(0.5f);
    }

    public bool RespondsToGetAttackingSlots(bool playerIsAttacker, List<CardSlot> originalSlots, List<CardSlot> currentSlots) => !active;

    public List<CardSlot> GetAttackingSlots(bool playerIsAttacker, List<CardSlot> originalSlots, List<CardSlot> currentSlots)
    {
        base.Card.Anim.StrongNegationEffect();
        currentSlots.Remove(base.Card.Slot);
        return currentSlots;
    }

    public int TriggerPriority(bool playerIsAttacker, List<CardSlot> originalSlots) => 0;
}

public partial class RevenantMod
{
    public void Add_Ability_ActivatedNegateSkipAttack()
    {
        const string rulebookName = "Dormant";
        const string rulebookDescription = "Pay Bones equal to the Power of [creature] to disable this sigil for 1 turn. [creature] cannot attack normally.";

        AbilityInfo ability = AbilityManager.New(
            PluginGUID,
            rulebookName.SplitPascalCase(),
            rulebookDescription,
            typeof(ActivatedNegateSkipAttack),
            Loader.LoadIcon(rulebookName)
            )
            //.SetDefaultPart1Ability()
            .SetActivated()
            //.AddMetaCategories(GuidManager.GetEnumValue<AbilityMetaCategory>("arackulele.inscryption.grimoramod", "ElectricChairLevel3"))
            //.SetPixelAbilityIcon(Asset.LoadTexture("pixelability_activated_" + rulebookName), FilterMode.Point)
            ;
        ability.powerLevel = -2;
        ability.pixelIcon = Loader.LoadSprite("pixelability_activated_" + rulebookName);
        ActivatedNegateSkipAttack.ability = ability.ability;
    }
}
