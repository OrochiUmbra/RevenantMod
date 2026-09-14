using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Guid;
using Sirenix.Utilities;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace RevenantMod;

public class CalamityEffusion : AbilityBehaviour
{
    public static Ability ability;
    public override Ability Ability => ability;

    public override bool RespondsToTurnEnd(bool playerUpkeep) => base.Card.OpponentCard != playerUpkeep && !base.Card.Dead;

    public override IEnumerator OnTurnEnd(bool playerUpkeep)
    {
        yield return PreSuccessfulTriggerSequence();
        Singleton<ViewManager>.Instance.SwitchToView(View.Board, lockAfter: true);
        foreach (CardSlot slot in Singleton<BoardManager>.Instance.AllSlotsCopy.Where(slot => slot.Card != null && !slot.Card.Dead))
        {
            yield return slot.Card.Status.damageTaken += 1;
            slot.Card.Anim.PlayHitAnimation();

            if (slot.Card.Health <= 0)
                yield return slot.Card.Die(false);
        }
        yield return new WaitForSeconds(0.5f);
        Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
        yield return base.LearnAbility(0.5f);
    }
}

public partial class RevenantMod
{
    public void Add_Ability_CalamityEffusion()
    {
        const string rulebookName = nameof(CalamityEffusion);
        const string rulebookDescription = "At the end of the owner's turn, all creatures on the board lose 1 health.";

        AbilityInfo ability = AbilityManager.New(
            PluginGUID,
            rulebookName.SplitPascalCase(),
            rulebookDescription,
            typeof(CalamityEffusion),
            Loader.LoadIcon(rulebookName)
            )
            .SetDefaultPart1Ability()
            .AddMetaCategories(GuidManager.GetEnumValue<AbilityMetaCategory>("arackulele.inscryption.grimoramod", "ElectricChairLevel3"))
            .SetPixelAbilityIcon(Loader.LoadIcon(rulebookName, true))
            ;
        ability.powerLevel = 5;
        CalamityEffusion.ability = ability.ability;
    }
}
