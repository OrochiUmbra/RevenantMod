using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Guid;
using InscryptionAPI.Triggers;
using Sirenix.Utilities;
using System.Linq;

namespace RevenantMod;

public class BuffNeighboursHealth : AbilityBehaviour, IPassiveHealthBuff
{
    public static Ability ability;
    public override Ability Ability => ability;

    /*public override bool RespondsToTurnEnd(bool playerTurnEnd)
    {
        return base.Card.OpponentCard != playerTurnEnd;
    }

    public override IEnumerator OnTurnEnd(bool playerTurnEnd)
    {
        yield return PreSuccessfulTriggerSequence();
        Singleton<ViewManager>.Instance.SwitchToView(View.Board, lockAfter: true);
        foreach (CardSlot slot in from slot in Singleton<BoardManager>.Instance.GetAdjacentSlots(base.Card.Slot) where slot.Card != null select slot)
        {
            if (slot.Card.Health <= slot.Card.Info.Health)
            {
                slot.Card.Anim.StrongNegationEffect();
                slot.Card.HealDamage(1);
            }
        }
        yield return new WaitForSeconds(0.5f);
        Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
        yield return base.LearnAbility(0.5f);
    }*/

    public int GetPassiveHealthBuff(PlayableCard target)
    {
        return Card.OnBoard && Singleton<BoardManager>.Instance.GetAdjacentSlots(target.Slot)
            .Where(slot => slot != null && slot.Card != null && slot.Card.HasAbility(ability)).Count() > 0 ? 1 : 0;
    }
}

public partial class RevenantMod
{
    public void Add_Ability_BuffNeighborHealth()
    {
        const string rulebookName = "Protector";
        const string rulebookDescription = "Creatures adjacent to [creature] gain 1 max health.";

        AbilityInfo ability = AbilityManager.New(
            PluginGUID,
            rulebookName.SplitPascalCase(),
            rulebookDescription,
            typeof(BuffNeighboursHealth),
            Loader.LoadIcon(rulebookName)
            )
            .SetDefaultPart1Ability()
            .AddMetaCategories(GuidManager.GetEnumValue<AbilityMetaCategory>("arackulele.inscryption.grimoramod", "ElectricChairLevel2"))
            .SetPixelAbilityIcon(Loader.LoadIcon(rulebookName, true))
            ;
        ability.powerLevel = 2;
        BuffNeighboursHealth.ability = ability.ability;
    }
}
