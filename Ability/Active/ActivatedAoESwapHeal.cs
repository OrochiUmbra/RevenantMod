using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Guid;
using Sirenix.Utilities;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace RevenantMod;

public class ActivatedAoESwapHeal : ActivatedAbilityBehaviour
{
    public static Ability ability;
    public override Ability Ability => ability;
    
    private const int BONES_COST = 2;
    public override int BonesCost => BONES_COST;

    private bool active = false; // this doesn't need to be static

    //public override bool CanActivate() => !active;

    public override IEnumerator Activate()
    {
        yield return base.PreSuccessfulTriggerSequence();
        this.SwitchState();
    }

    public override bool RespondsToTurnEnd(bool playerUpkeep) => base.Card.OpponentCard != playerUpkeep && !base.Card.Dead;

    public override IEnumerator OnTurnEnd(bool playerUpkeep)
    {
        yield return base.PreSuccessfulTriggerSequence();
        if (Singleton<BoardManager>.Instance is BoardManager3D)
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, lockAfter: true);
        foreach (CardSlot slot in Singleton<BoardManager>.Instance.AllSlotsCopy.Where(slot => slot.Card != null && !slot.Card.Dead))
        {
            if (active)
            {
                yield return slot.Card.Status.damageTaken += 1;
                slot.Card.Anim.PlayHitAnimation();
            }
            else
            {
                slot.Card.Anim.StrongNegationEffect();
                slot.Card.HealDamage(1);
            }
            if (slot.Card.Health <= 0)
                yield return slot.Card.Die(false);
        }
        yield return new WaitForSeconds(0.5f);
        if (!base.Card.Dead)
        {
            InochiCalamity component = base.Card.GetComponent<InochiCalamity>();
            if (component != null && base.Card.Info.name.ToLowerInvariant().EndsWith("umbra"))
                this.SwitchState();
        }
        Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
        yield return base.LearnAbility(0.5f);
    }
    
    private void SwitchState()
    {
        if (!base.Card.Dead)
        {
            base.Card.Anim.PlayTransformAnimation();
            active = !active;
            if (Singleton<BoardManager>.Instance is BoardManager3D)
                base.Card.RenderInfo.OverrideAbilityIcon(ability, Loader.LoadIcon(active ? "dansemacabre_active" : "dansemacabre"));
            base.Card.RenderCard();
        }
    }
}

public partial class RevenantMod
{
    public void Add_Ability_ActivatedAoESwapHeal()
    {
        const string rulebookName = "DanseMacabre";
        const string rulebookDescription = "Pay 2 Bones to reverse this sigil effect. At the end of the owner's turn, all creatures on the board gain 1 health.";

        AbilityInfo ability = AbilityManager.New(
            PluginGUID,
            rulebookName.SplitPascalCase(),
            rulebookDescription,
            typeof(ActivatedAoESwapHeal),
            Loader.LoadIcon(rulebookName)
            )
            .SetDefaultPart1Ability()
            .SetActivated()
            .AddMetaCategories(GuidManager.GetEnumValue<AbilityMetaCategory>("arackulele.inscryption.grimoramod", "ElectricChairLevel3"))
            //.SetPixelAbilityIcon(Asset.LoadTexture("pixelability_activated_" + rulebookName), FilterMode.Point)
            ;
        ability.powerLevel = 5;
        ability.pixelIcon = Loader.LoadSprite("pixelability_activated_" + rulebookName);
        ActivatedAoESwapHeal.ability = ability.ability;
    }
}
