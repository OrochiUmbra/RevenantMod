using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Guid;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RevenantMod;

public class DrawCopyDelay : DrawCreatedCard
{
    public static Ability ability;
    public override Ability Ability => ability;

    private int turn = 0;

    public override CardInfo CardToDraw => base.Card.Info;

    public override List<CardModificationInfo> CardToDrawTempMods => null;

    public override bool RespondsToTurnEnd(bool playerTurnEnd) => Card.OpponentCard != playerTurnEnd && Card.Info.HasAbility(ability);

    public override IEnumerator OnTurnEnd(bool playerTurnEnd)
    {
        yield return base.PreSuccessfulTriggerSequence();
        turn++;
        Card.RenderInfo.OverrideAbilityIcon(ability, Loader.LoadIcon(nameof(DrawCopyDelay) + (turn <= 0 ? "" : turn)));
        Card.RenderCard();
        Singleton<ViewManager>.Instance.SwitchToView(View.Board, false, false);
        base.Card.Anim.StrongNegationEffect();

        if (turn > 1)
        {
            foreach (Ability stack in base.Card.Info.Abilities.Where(copy => copy == ability))
                yield return base.CreateDrawnCard();

            yield return new WaitForSeconds(0.2f);
            base.Card.Anim.PlayTransformAnimation();
            base.Card.AddTemporaryMod(new() { negateAbilities = new List<Ability> { ability } });
            yield return new WaitForSeconds(0.5f);
        }
        yield return LearnAbility(0.5f);
    }
}

public partial class RevenantMod
{
    public void Add_Ability_DrawCopyDelay()
    {
        const string rulebookName = nameof(DrawCopyDelay);
        const string rulebookDescription = "[creature] creates a copy of it in your hand after 2 turns on the board.";

        AbilityInfo ability = AbilityManager.New(
            PluginGUID,
            "Encroachment",
            rulebookDescription,
            typeof(DrawCopyDelay),
            Loader.LoadIcon(rulebookName)
            )
            .SetDefaultPart1Ability()
            .AddMetaCategories(GuidManager.GetEnumValue<AbilityMetaCategory>("arackulele.inscryption.grimoramod", "ElectricChairLevel1"))
            .SetPixelAbilityIcon(Loader.LoadIcon(rulebookName, true))
            ;
        ability.powerLevel = 2;
        DrawCopyDelay.ability = ability.ability;
    }
}
