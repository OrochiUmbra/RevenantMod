using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Guid;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;

namespace RevenantMod;

public class CorpseParty : DrawCreatedCard
{
    public static Ability ability;
    public override Ability Ability => ability;

    public override CardInfo CardToDraw => CardLoader.GetCardByName(GrimoraModHelpers.GrimoraModIsActive() ? "arackulele.inscryption.grimoramod_Zombie" : "Opossum");

    public override List<CardModificationInfo> CardToDrawTempMods => null;

    public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer) =>
        Card.OnBoard && card != null && killer != null && fromCombat && Card.Slot.IsPlayerSlot && deathSlot.IsPlayerSlot;

    public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
    {
        if (card != null && !card.Info.HasAbility(Ability.Brittle) && !card.Info.HasTrait(Trait.Terrain) && !card.Info.HasTrait(Trait.Pelt))
        {
            yield return PreSuccessfulTriggerSequence();
            Card.Anim.StrongNegationEffect();
            yield return CreateDrawnCard();
            yield return LearnAbility(0.5f);
        }
    }
}

public partial class RevenantMod
{
    public void Add_Ability_CorpseParty()
    {
        const string rulebookName = nameof(CorpseParty);
        const string rulebookDescription = "When a non-brittle creature on the owner's side perishes by combat"
                                         + ", a Zombie enters your hand. A Zombie is defined as: 1 Power, 1 Health.";

        AbilityInfo ability = AbilityManager.New(
            PluginGUID,
            rulebookName.SplitPascalCase(),
            rulebookDescription,
            typeof(CorpseParty),
            Loader.LoadIcon(rulebookName)
            )
            .SetDefaultPart1Ability()
            .AddMetaCategories(GuidManager.GetEnumValue<AbilityMetaCategory>("arackulele.inscryption.grimoramod", "ElectricChairLevel1"))
            .SetPixelAbilityIcon(Loader.LoadIcon(rulebookName, true))
            ;
        ability.powerLevel = 2;
        CorpseParty.ability = ability.ability;
    }
}
