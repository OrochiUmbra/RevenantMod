using DiskCardGame;
using InscryptionAPI.Card;
using Sirenix.Utilities;
using System.Collections;
using System.Linq;

namespace RevenantMod;

public class MacabreBonePulse : AbilityBehaviour
{
    public static Ability ability;
    public override Ability Ability => ability;

    public override bool RespondsToResolveOnBoard() => true;

    public override IEnumerator OnResolveOnBoard()
    {
        yield return PreSuccessfulTriggerSequence();
        if (!Card.OpponentCard)
            foreach (CardSlot slot in Singleton<BoardManager>.Instance.PlayerSlotsCopy.Where(slot => slot.Card != null && !slot.Card.Dead))
            {
                slot.Card.Anim.LightNegationEffect();
                yield return Singleton<ResourcesManager>.Instance.AddBones(2, Card.Slot);
            }
        yield return Card.Die(false, null, true);
        yield return base.LearnAbility(0.5f);
    }
}

public partial class RevenantMod
{
    public void Add_Ability_MacabreBonePulse()
    {
        const string rulebookName = nameof(MacabreBonePulse);
        const string rulebookDescription = "When [creature] is played, it perishes immediately, dropping Bones equal to double the amount of creatures on the owner's side.";

        AbilityInfo ability = AbilityManager.New(
            PluginGUID,
            rulebookName.SplitPascalCase(),
            rulebookDescription,
            typeof(MacabreBonePulse),
            Loader.LoadIcon(rulebookName)
            )
            //.SetDefaultPart1Ability()
            .SetPixelAbilityIcon(Loader.LoadIcon(rulebookName, true))
            ;
        ability.powerLevel = -1;
        MacabreBonePulse.ability = ability.ability;
    }
}
