using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Triggers;
using Sirenix.Utilities;
using System.Collections.Generic;

namespace RevenantMod;

public class Torpid : AbilityBehaviour, IGetAttackingSlots
{
    public static Ability ability;
    public override Ability Ability => ability;

    public bool RespondsToGetAttackingSlots(bool playerIsAttacker, List<CardSlot> originalSlots, List<CardSlot> currentSlots) => true;

    public List<CardSlot> GetAttackingSlots(bool playerIsAttacker, List<CardSlot> originalSlots, List<CardSlot> currentSlots)
    {
        base.Card.Anim.StrongNegationEffect();
        currentSlots.Remove(base.Card.Slot);
        return currentSlots;
    }

    public int TriggerPriority(bool playerIsAttacker, List<CardSlot> originalSlots) => 0;
}
/*[HarmonyPatch(typeof(CombatPhaseManager), nameof(CombatPhaseManager.SlotAttackSequence))]
internal class TorpidPatch // don't trigger SlotAttackSequence if Torpid is active
{
    private static bool Prefix(CardSlot slot)
    {
        if (slot.Card != null && slot.Card.HasAbility(Torpid.ability))
        {
            slot.Card.Anim.StrongNegationEffect();
            return false;
        }

        return true;
    }
}*/
public partial class RevenantMod
{
    public void Add_Ability_Torpid()
    {
        const string rulebookName = nameof(Torpid);
        const string rulebookDescription = "[creature] cannot attack normally.";

        AbilityInfo ability = AbilityManager.New(
            PluginGUID,
            rulebookName.SplitPascalCase(),
            rulebookDescription,
            typeof(Torpid),
            Loader.LoadIcon(rulebookName)
            )
            //.SetDefaultPart1Ability()
            //.AddMetaCategories(GuidManager.GetEnumValue<AbilityMetaCategory>("arackulele.inscryption.grimoramod", "ElectricChairLevel3"))
            .SetPixelAbilityIcon(Loader.LoadIcon(rulebookName, true))
            ;
        ability.powerLevel = -3;
        Torpid.ability = ability.ability;
    }
}
