using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Guid;
using Sirenix.Utilities;
using System.Collections;
using System.Linq;

namespace RevenantMod;

public class ActivatedEnergyRevive : ActivatedAbilityBehaviour
{
    public static Ability ability;
    public override Ability Ability => ability;

    private const int ENERGY_COST = 3;
    public override int EnergyCost => ENERGY_COST;

    public override bool CanActivate() => Singleton<GraveyardManager>.Instance.playerCards.Count(x => x.DeadCardInfo.LacksAbility(Ability.Brittle)) > 0;

    public override IEnumerator Activate()
    {
        yield return base.PreSuccessfulTriggerSequence();
        base.Card.Anim.StrongNegationEffect();
        CardInfo cardToDraw = Singleton<GraveyardManager>.Instance.playerCards.FirstOrDefault(x => x.DeadCardInfo.LacksAbility(Ability.Brittle))?.DeadCardInfo ?? null;
        if (cardToDraw == null)
            yield break;
        yield return Singleton<GraveyardManager>.Instance.DrawCard(cardToDraw);
        yield return base.LearnAbility(0.5f);
    }

    //public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer) => card != Card && card.OpponentCard == Card.OpponentCard && card.LacksAbility(Ability.Brittle);
    /*
    public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
    {
        CardInfo cloneInfo = card.Info;
        cloneInfo.Mods = new(cloneInfo.Mods);
        cardlist.Add(cloneInfo);
        yield break;
    }*/
}

public partial class RevenantMod
{
    public void Add_Ability_ActivatedEnergyRevive()
    {
        const string rulebookName = "Evocation";
        const string rulebookDescription = "Pay 3 Energy to return the oldest non-Brittle allied creature from the graveyard to your hand.";

        AbilityInfo ability = AbilityManager.New(
            PluginGUID,
            rulebookName.SplitPascalCase(),
            rulebookDescription,
            typeof(ActivatedEnergyRevive),
            Loader.LoadIcon(rulebookName)
            )
            .SetDefaultPart1Ability()
            .SetActivated()
            .AddMetaCategories(GuidManager.GetEnumValue<AbilityMetaCategory>("arackulele.inscryption.grimoramod", "ElectricChairLevel3"))
            //.SetPixelAbilityIcon(Asset.LoadTexture("pixelability_activated_" + rulebookName), FilterMode.Point)
            ;
        ability.powerLevel = 4;
        ability.pixelIcon = Loader.LoadSprite("pixelability_activated_" + rulebookName);
        ActivatedEnergyRevive.ability = ability.ability;
    }
}
