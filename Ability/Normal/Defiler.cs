using DiskCardGame;
using InscryptionAPI.Card;
using Pixelplacement;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RevenantMod;

public class Defiler : AbilityBehaviour
{
    public static Ability ability;
    public override Ability Ability => ability;

    private bool HasPile => Singleton<CardDrawPiles3D>.Instance && Singleton<CardDrawPiles3D>.Instance.Pile;

    public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer) => true;

    public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
    {
        yield return PreSuccessfulTriggerSequence();
        Singleton<ViewManager>.Instance.SwitchToView(View.Hand, lockAfter: true);
        yield return new WaitForSeconds(0.2f);
        if (HasPile)
        {
            (CardDrawPiles.Instance as CardDrawPiles3D).pile.Draw();
        }
        yield return Singleton<CardDrawPiles>.Instance.DrawCardFromDeck();
        yield return new WaitForSeconds(0.2f);
        List<CardInfo> list = Singleton<PlayerHand>.Instance.CardsInHand.FindAll(x => x != Singleton<PlayerHand>.Instance.ChoosingSlotCard).Select(x => x.Info).ToList();
        if (list.Count > 0)
        {
            (CardDrawPiles.Instance as CardDrawPiles3D).pile.Draw();
            Singleton<ViewManager>.Instance.SwitchToView(View.DeckSelection, lockAfter: true);
            yield return new WaitForSeconds(0.2f);
            CardInfo selectedCard = null;
            yield return ChooseCard(list, delegate (CardInfo c)
            {
                selectedCard = c;
            });
            if (Singleton<ViewManager>.Instance.CurrentView != View.Hand)
            {
                yield return new WaitForSeconds(0.2f);
                Singleton<ViewManager>.Instance.SwitchToView(View.Hand, lockAfter: true);
                yield return new WaitForSeconds(0.2f);
            }
            PlayableCard defile = Singleton<PlayerHand>.Instance.CardsInHand.Find(x => x.Info.name == selectedCard.name && x.Info.Mods == selectedCard.Mods);
            if (defile != null)
            {
                yield return new WaitForSeconds(0.2f);
                yield return RemoveCard(defile);
                yield return new WaitForSeconds(0.2f);
            }
        }
        Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
        yield return LearnAbility(0.5f);
    }

    private IEnumerator ChooseCard(List<CardInfo> list, Action<CardInfo> cardSelectedCallback)
    {
        Singleton<ViewManager>.Instance.SwitchToView(View.DeckSelection, false, true);
        SelectableCard selectedCard = null;
        yield return Singleton<BoardManager>.Instance.CardSelector.SelectCardFrom(list, null, delegate (SelectableCard x)
        {
            selectedCard = x;
        }, null, true);
        Tween.Position(selectedCard.transform, selectedCard.transform.position + Vector3.back * 4f, 0.1f, 0f, Tween.EaseIn, Tween.LoopType.None, null, null, true);
        Destroy(selectedCard.gameObject, 0.1f);
        cardSelectedCallback(selectedCard.Info);
    }

    private IEnumerator RemoveCard(PlayableCard card)
    {
        card.SetInteractionEnabled(false);
        card.Anim.PlayDeathAnimation(true);
        Singleton<PlayerHand>.Instance.RemoveCardFromHand(card);
        yield return Singleton<PlayerHand>.Instance.CleanUpCard(card);
    }
}

public partial class RevenantMod
{
    public void Add_Ability_Defiler()
    {
        const string rulebookName = nameof(Defiler);
        const string rulebookDescription = "When [creature] perishes, draw a card then choose one to discard from your hand.";

        AbilityInfo ability = AbilityManager.New(
            PluginGUID,
            rulebookName.SplitPascalCase(),
            rulebookDescription,
            typeof(Defiler),
            AbilitiesUtil.LoadAbilityIcon(Ability.FileSizeDamage.ToString())
            )
            //.SetDefaultPart1Ability()
            //.AddMetaCategories(GuidManager.GetEnumValue<AbilityMetaCategory>("arackulele.inscryption.grimoramod", "ElectricChairLevel3"))
            .SetPixelAbilityIcon(Loader.LoadIcon(rulebookName, true))
            ;
        ability.powerLevel = -1;
        Defiler.ability = ability.ability;
    }
}
