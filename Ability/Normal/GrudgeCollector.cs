using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Guid;
using InscryptionAPI.Triggers;
using Sirenix.Utilities;
using System.Collections;
using UnityEngine;

namespace RevenantMod;

public class GrudgeCollector : AbilityBehaviour, IOnCardDealtDamageDirectly
{
    public static Ability ability;
    public override Ability Ability => ability;

    private int hitTaken = 0;
    public const string ModSingletonId = "RevenantMod_GrudgeCollector";
    private CardModificationInfo _modInfo;

    private void Start()
    {
        _modInfo = new CardModificationInfo
        {
            singletonId = ModSingletonId
        };
        Card.AddTemporaryMod(_modInfo);
    }

    public bool RespondsToCardDealtDamageDirectly(PlayableCard attacker, CardSlot cardSlot, int amount) => attacker == attacker.OpponentCard && amount > 0;

    public IEnumerator OnCardDealtDamageDirectly(PlayableCard attacker, CardSlot cardSlot, int amount)
    {
        yield return PreSuccessfulTriggerSequence();
        hitTaken++;
        Card.Anim.StrongNegationEffect();
        Card.RenderInfo.OverrideAbilityIcon(ability, Loader.LoadIcon(nameof(GrudgeCollector) + (hitTaken <= 0 ? "" : hitTaken)));
        Card.RenderCard();
        yield return new WaitForSeconds(0.5f);
        if (hitTaken > 1)
        {
            Singleton<ViewManager>.Instance.SwitchToView(View.Board, lockAfter: true);
            Card.Anim.PlayTransformAnimation();
            _modInfo.attackAdjustment++;
            hitTaken = 0;
            Card.RenderInfo.OverrideAbilityIcon(ability, Loader.LoadIcon(nameof(GrudgeCollector)));
            Card.OnStatsChanged();
            Card.RenderCard();
            yield return new WaitForSeconds(0.5f);
            Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
        }
        yield return LearnAbility(0.5f);
    }
}

public partial class RevenantMod
{
    public void Add_Ability_GrudgeCollector()
    {
        const string rulebookName = nameof(GrudgeCollector);
        const string rulebookDescription = "[creature] gains 1 power for every 2 times opponent creatures deal direct damage to its owner.";

        AbilityInfo ability = AbilityManager.New(
            PluginGUID,
            rulebookName.SplitPascalCase(),
            rulebookDescription,
            typeof(GrudgeCollector),
            Loader.LoadIcon(rulebookName)
            )
            .SetDefaultPart1Ability()
            .AddMetaCategories(GuidManager.GetEnumValue<AbilityMetaCategory>("arackulele.inscryption.grimoramod", "ElectricChairLevel2"))
            .SetPixelAbilityIcon(Loader.LoadIcon(rulebookName, true))
            ;
        ability.powerLevel = 2;
        GrudgeCollector.ability = ability.ability;
    }
}
