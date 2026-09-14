using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Guid;
using System.Collections;
using UnityEngine;

namespace RevenantMod;

public class LatchDigger : Latch
{
    public static Ability ability;
    public override Ability Ability => ability;
    public override Ability LatchAbility => Ability.BoneDigger;

    public override bool RespondsToTurnEnd(bool playerTurnEnd) => Card.OpponentCard != playerTurnEnd && !Card.Dead;

    public override IEnumerator OnTurnEnd(bool playerTurnEnd)
    {
        yield return PreSuccessfulTriggerSequence();
        ViewManager.Instance.SwitchToView(View.Board, lockAfter: true);
        yield return new WaitForSeconds(0.1f);
        Card.Anim.LightNegationEffect();
        yield return Singleton<ResourcesManager>.Instance.AddBones(1, Card.Slot);
        yield return new WaitForSeconds(0.1f);
        Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
        yield return base.LearnAbility(0.5f);
    }
}

public partial class RevenantMod
{
    public void Add_Ability_LatchDigger()
    {
        const string rulebookName = nameof(LatchDigger);
        const string rulebookDescription = "At the end of the owner's turn, [creature] will generate 1 Bone. "
                                         + "When it perishes, its owner chooses a creature to gain the Bone Digger sigil.";

        AbilityInfo ability = AbilityManager.New(
            PluginGUID,
            "Digger Latch",
            rulebookDescription,
            typeof(LatchDigger),
            Loader.LoadIcon(rulebookName)
            )
            .SetDefaultPart1Ability()
            .AddMetaCategories(GuidManager.GetEnumValue<AbilityMetaCategory>("arackulele.inscryption.grimoramod", "ElectricChairLevel1"))
            .SetPixelAbilityIcon(Loader.LoadIcon(rulebookName, true))
            ;
        ability.powerLevel = 3;
        LatchDigger.ability = ability.ability;
    }
}
