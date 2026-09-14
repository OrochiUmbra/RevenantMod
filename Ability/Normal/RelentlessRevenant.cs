
using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Guid;
using Sirenix.Utilities;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace RevenantMod;

public class RelentlessRevenant : AbilityBehaviour
{
    public static Ability ability;
    public override Ability Ability => ability;

    public override bool RespondsToUpkeep(bool playerUpkeep) => Card.OpponentCard != playerUpkeep && !Card.Dead;

    public override IEnumerator OnUpkeep(bool playerUpkeep)
    {
        yield return base.PreSuccessfulTriggerSequence();
        Singleton<ViewManager>.Instance.SwitchToView(View.Board, lockAfter: true);
        foreach (CardSlot slot in Singleton<BoardManager>.Instance.GetAdjacentSlots(base.Card.Slot)
            .Where(slot => slot.Card != null && !slot.Card.Info.name.ToLowerInvariant().EndsWith("revenant") && !slot.Card.Info.HasAbility(ability)))
        {
            yield return slot.Card.TransformIntoCard(CardLoader.GetCardByName("Revenant"));
            slot.Card.Status.damageTaken = 0;
        }
        yield return new WaitForSeconds(0.5f);
        Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
        yield return base.LearnAbility(0.5f);
    }
}

public partial class RevenantMod
{
    public void Add_Ability_RelentlessRevenant()
    {
        const string rulebookName = nameof(RelentlessRevenant);
        const string rulebookDescription = "At the start of the owner's turn, creatures adjacent to [creature] transform into a Revenant. "
                                         + "A Revenant is defined as: 3 Power, 1 Health, Brittle.";

        AbilityInfo ability = AbilityManager.New(
            PluginGUID,
            rulebookName.SplitPascalCase(),
            rulebookDescription,
            typeof(RelentlessRevenant),
            Loader.LoadIcon(rulebookName)
            )
            .SetDefaultPart1Ability()
            .AddMetaCategories(GuidManager.GetEnumValue<AbilityMetaCategory>("arackulele.inscryption.grimoramod", "ElectricChairLevel3"))
            .SetPixelAbilityIcon(Loader.LoadIcon(rulebookName, true))
            ;
        ability.powerLevel = 5;
        RelentlessRevenant.ability = ability.ability;
    }
}
