using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Guid;
using Sirenix.Utilities;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace RevenantMod;

public class ExplodingCorpse : AbilityBehaviour
{
    public static Ability ability;
    public override Ability Ability => ability;

    public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer) => !wasSacrifice;

    public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
    {
        yield return PreSuccessfulTriggerSequence();
        Singleton<ViewManager>.Instance.SwitchToView(View.Board, lockAfter: true);
        foreach (CardSlot slot in Singleton<BoardManager>.Instance.AllSlotsCopy.Where(slot => slot.Card == null))
            yield return Singleton<BoardManager>.Instance.CreateCardInSlot(CardLoader.GetCardByName($"{RevenantMod.PluginGUID}_Guts"), slot, 0.1f);
        yield return new WaitForSeconds(0.5f);
        Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
        yield return base.LearnAbility(0.5f);
    }
}

public partial class RevenantMod
{
    public void Add_Ability_ExplodingCorpse()
    {
        const string rulebookName = nameof(ExplodingCorpse);
        const string rulebookDescription = "When [creature] perishes by combat, all empty spaces on the board are filled with a Guts card.";

        AbilityInfo ability = AbilityManager.New(
            PluginGUID,
            rulebookName.SplitPascalCase(),
            rulebookDescription,
            typeof(ExplodingCorpse),
            AbilitiesUtil.LoadAbilityIcon(Ability.ExplodingCorpse.ToString())
            )
            .SetDefaultPart1Ability()
            .AddMetaCategories(GuidManager.GetEnumValue<AbilityMetaCategory>("arackulele.inscryption.grimoramod", "ElectricChairLevel2"))
            .SetPixelAbilityIcon(Loader.LoadIcon(rulebookName, true))
            ;
        ability.powerLevel = 2;
        ExplodingCorpse.ability = ability.ability;
    }
}
