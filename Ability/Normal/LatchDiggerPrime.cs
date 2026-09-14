using DiskCardGame;
using InscryptionAPI.Card;
using System.Collections;
using UnityEngine;

namespace RevenantMod;

public class LatchDiggerPrime : Latch
{
    public static Ability ability;
    public override Ability Ability => ability;
    public override Ability LatchAbility => ability;

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
    public void Add_Ability_LatchDiggerPrime()
    {
        const string rulebookName = nameof(LatchDiggerPrime);
        const string rulebookDescription = "At the end of the owner's turn, [creature] will generate 1 Bone. "
                                         + "When it perishes, its owner chooses a creature to gain this sigil.";

        AbilityInfo ability = AbilityManager.New(
            PluginGUID,
            "Primed Digger Latch",
            rulebookDescription,
            typeof(LatchDiggerPrime),
            Loader.LoadIcon(rulebookName)
            )
            .SetDefaultPart1Ability()
            .SetPixelAbilityIcon(Loader.LoadIcon(rulebookName, true))
            ;
        ability.powerLevel = 5;
        LatchDiggerPrime.ability = ability.ability;
    }
}
