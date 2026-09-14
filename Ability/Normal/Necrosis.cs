using DiskCardGame;
using InscryptionAPI.Card;
using Sirenix.Utilities;
using System.Collections;

namespace RevenantMod;

public class Necrosis : AbilityBehaviour
{
    public static Ability ability;
    public override Ability Ability => ability;

    public const string ModSingletonId = "RevenantMod_Necrosis";
    private CardModificationInfo _modInfo;

    private void Start()
    {
        _modInfo = new CardModificationInfo
        {
            singletonId = ModSingletonId
        };
        Card.AddTemporaryMod(_modInfo);
    }

    public override bool RespondsToUpkeep(bool playerUpkeep) => Card.OpponentCard != playerUpkeep && !Card.Dead;

    public override IEnumerator OnUpkeep(bool playerUpkeep)
    {
        yield return PreSuccessfulTriggerSequence();
        Card.Anim.StrongNegationEffect();
        _modInfo.attackAdjustment -= 1;
        _modInfo.healthAdjustment -= 1;
        Card.Anim.StrongNegationEffect();
        yield return LearnAbility(0.5f);

        if (Card.Health <= 0)
            yield return Card.Die(false);
    }
}

public partial class RevenantMod
{
    public void Add_Ability_Necrosis()
    {
        const string rulebookName = nameof(Necrosis);
        const string rulebookDescription = "[creature] loses 1 power and 1 max health after each turn on the board.";

        AbilityInfo ability = AbilityManager.New(
            PluginGUID,
            rulebookName.SplitPascalCase(),
            rulebookDescription,
            typeof(Necrosis),
            Loader.LoadIcon(rulebookName)
            )
            //.SetDefaultPart1Ability()
            //.AddMetaCategories(GuidManager.GetEnumValue<AbilityMetaCategory>("arackulele.inscryption.grimoramod", "ElectricChairLevel3"))
            .SetPixelAbilityIcon(Loader.LoadIcon(rulebookName, true))
            ;
        ability.powerLevel = -3;
        Necrosis.ability = ability.ability;
    }
}
