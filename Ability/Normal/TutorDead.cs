using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Guid;
using InscryptionAPI.Helpers.Extensions;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RevenantMod;

public class TutorDead : AbilityBehaviour
{
    public static Ability ability;
    public override Ability Ability => ability;

    //private bool HasPile => Singleton<CardDrawPiles3D>.Instance && Singleton<CardDrawPiles3D>.Instance.Pile;

    /*private readonly List<CardInfo> cardlist = new();

    public override bool RespondsToOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer) => card != Card && card.OpponentCard == Card.OpponentCard;

    public override IEnumerator OnOtherCardDie(PlayableCard card, CardSlot deathSlot, bool fromCombat, PlayableCard killer)
    {
        CardInfo cloneInfo = card.Info;
        cloneInfo.Mods = new(cloneInfo.Mods);
        cardlist.Add(cloneInfo);
        yield break;
    }*/
    //private static readonly Dictionary<CardInfo, bool> cardList = Singleton<GraveyardManager>.Instance.playerCards;

    private List<CardInfo> GetCardList()
    {
        List<CardInfo> list = new();
        foreach (GraveyardManager.DeadCardData deadCardData in Singleton<GraveyardManager>.Instance.AllDeadCards)
        {
            list.Add(deadCardData.DeadCardInfo);
        }
        return list;
    }

    public override bool RespondsToResolveOnBoard() => true;

    public override IEnumerator OnResolveOnBoard()
    {
        yield return PreSuccessfulTriggerSequence();
        if (Singleton<GraveyardManager>.Instance.AllDeadCards.Count > 0)
            yield return Tutor();
        else
        {
            Card.Anim.StrongNegationEffect();
            yield return TextDisplayer.Instance.ShowUntilInput("Oh... There are no dead cards yet.");
        }
        Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
        yield return LearnAbility(0.5f);
    }

    private IEnumerator Tutor()
    {
        CardInfo selectedCard = null;
        if (Singleton<BoardManager>.Instance is BoardManager3D)
        {
            yield return ChooseCard(delegate (CardInfo c)
            {
                selectedCard = c;
            });
            yield return new WaitForSeconds(0.2f);
            ViewManager.Instance.SwitchToView(View.Default);
            yield return new WaitForSeconds(0.2f);
            yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(selectedCard);
        }
        else
        {
            CardInfo cardToDraw = Singleton<GraveyardManager>.Instance.AllDeadCards.GetRandom().DeadCardInfo;
            if (cardToDraw != null)
                yield return Singleton<GraveyardManager>.Instance.DrawCard(cardToDraw);
        }
    }

    private IEnumerator ChooseCard(Action<CardInfo> cardSelectedCallback)
    {
        Singleton<ViewManager>.Instance.SwitchToView(View.DeckSelection, immediate: false, lockAfter: true);
        SelectableCard selectedCard = null;
        yield return Singleton<BoardManager>.Instance.CardSelector.SelectCardFrom(GetCardList(), null, delegate (SelectableCard x)
        {
            selectedCard = x;
        });
        Singleton<GraveyardManager>.Instance.AllDeadCards.Remove(selectedCard.Info);
        Tween.Position(selectedCard.transform, selectedCard.transform.position + Vector3.back * 4f, 0.1f, 0f, Tween.EaseIn);
        Destroy(selectedCard.gameObject, 0.1f);
        cardSelectedCallback(selectedCard.Info);
    }
}

public partial class RevenantMod
{
    public void Add_Ability_TutorDead()
    {
        const string rulebookName = nameof(TutorDead);
        const string rulebookDescription = "When [creature] is played, you may choose a card that has already been put to rest, to bring back to your hand.";

        AbilityInfo ability = AbilityManager.New(
            PluginGUID,
            "Grim Epitaph",
            rulebookDescription,
            typeof(TutorDead),
            Loader.LoadIcon(rulebookName)
            )
            //.SetDefaultPart1Ability()
            .AddMetaCategories(GuidManager.GetEnumValue<AbilityMetaCategory>("arackulele.inscryption.grimoramod", "ElectricChairLevel3"), AbilityMetaCategory.Part1Rulebook)
            .SetPixelAbilityIcon(Loader.LoadIcon(rulebookName, true))
            ;
        ability.powerLevel = 4;
        TutorDead.ability = ability.ability;
    }
}
