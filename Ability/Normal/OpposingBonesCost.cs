using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Guid;
using System;
using System.Collections;

namespace RevenantMod;

public class OpposingBonesCost : AbilityBehaviour
{
    public static Ability ability;
    public override Ability Ability => ability;

    public override bool RespondsToResolveOnBoard() => Card.Slot.opposingSlot.Card != null;

    public override IEnumerator OnResolveOnBoard()
    {
        yield return PreSuccessfulTriggerSequence();
        Card.Anim.LightNegationEffect();
        int boneTax = Card.Slot.opposingSlot.Card.Info.BonesCost;
        yield return Singleton<ResourcesManager>.Instance.AddBones(Math.Min(boneTax, 15), Card.Slot.opposingSlot);
        yield return base.LearnAbility(0.5f);
    }
}

public partial class RevenantMod
{
    public void Add_Ability_Exciseman()
    {
        const string rulebookName = nameof(OpposingBonesCost);
        const string rulebookDescription = "When [creature] is played, Bones equal to the Bone cost of the opposing creature are granted to the player.";

        AbilityInfo ability = AbilityManager.New(
            PluginGUID,
            "Exorcise",
            rulebookDescription,
            typeof(OpposingBonesCost),
            Loader.LoadIcon(rulebookName)
            )
            .SetDefaultPart1Ability()
            .AddMetaCategories(GuidManager.GetEnumValue<AbilityMetaCategory>("arackulele.inscryption.grimoramod", "ElectricChairLevel3"))
            .SetPixelAbilityIcon(Loader.LoadIcon(rulebookName, true))
            ;
        ability.powerLevel = 4;
        OpposingBonesCost.ability = ability.ability;
    }
}
