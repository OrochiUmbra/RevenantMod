using DiskCardGame;
using InscryptionAPI.Card;
using InscryptionAPI.Guid;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RevenantMod;

public class Revedue : Brittle, IModifyDeadCard
{
    public static Ability ability;
    public override Ability Ability => ability;

    private float newCardYPosition;

    public override bool RespondsToPreDeathAnimation(bool wasSacrifice) => true;

    public bool RespondsToModifyDeadCard(CardInfo deadCard, bool wasSacrifice, bool opponentCard, CardInfo killer) => true;

    public CardInfo OnModifyDeadCard(CardInfo deadCard, bool wasSacrifice, bool opponentCard, CardInfo killer)
    {
        if (wasSacrifice)
            return deadCard;
        else
        {
            List<Ability> abilities = new();

            foreach (CardModificationInfo temporaryMod in base.GetNonDefaultModsFromSelf(ability))
                abilities.AddRange(temporaryMod.abilities);
            if (abilities.Count > 4)
                abilities.RemoveRange(3, abilities.Count - 4);

            CardModificationInfo cardModificationInfo = new()
            {
                abilities = abilities,
                fromCardMerge = true,
                singletonId = "orochi:Revedue"
            };

            CardInfo changedInfo = CardLoader.GetCardByName("Revenant");
            //changedInfo.Mods = new(deadCard.Mods);
            if (!changedInfo.Mods.Exists(x => x.singletonId == "orochi:Revedue"))
                changedInfo.Mods.Add(cardModificationInfo);
            if (Singleton<BoardManager>.Instance is BoardManager3D)
            {
                CardDrawPiles3D.Instance.SideDeck.AddCard(changedInfo);
                CardDrawPiles3D.Instance.SideDeck.AddCard(changedInfo);
            }
            else
            {
                CardDrawPiles.Instance.Deck.AddCard(changedInfo);
            }
            return null;
        }
    }// don't add to graveyard

    public override IEnumerator OnPreDeathAnimation(bool wasSacrifice)
    {
        yield return PreSuccessfulTriggerSequence();

        if (!wasSacrifice)
        {
            base.Card.Anim.SetFaceDown(true);
            yield return new WaitForSeconds(0.5f);

            if (Singleton<BoardManager>.Instance is BoardManager3D)
            {
                newCardYPosition = CardDrawPiles3D.Instance.SidePile.cardYSpacing * CardDrawPiles3D.Instance.SidePile.cards.Count;
                Singleton<CardDrawPiles3D>.Instance.SidePile.MoveCardToPile(Card, false, 0f, newCardYPosition);
                if (ViewManager.Instance.CurrentView != View.CardPiles)
                {
                    yield return new WaitForSeconds(0.2f);
                    ViewManager.Instance.SwitchToView(View.CardPiles);
                    yield return new WaitForSeconds(0.2f);
                }

                GameObject obj = CardDrawPiles3D.Instance.SidePile.CreateCard();
                obj.transform.localPosition = new Vector3(0f, newCardYPosition, 0f);
                obj.transform.localEulerAngles = CardDrawPiles3D.Instance.SidePile.GetRandomCardEulers(15f);
                yield return new WaitForSeconds(0.5f);
                Singleton<ViewManager>.Instance.SwitchToView(View.Default);
            }
            yield return LearnAbility(0.5f);
        }
    }
}

public partial class RevenantMod
{
    public void Add_Ability_Revedue()
    {
        const string rulebookName = nameof(Revedue);
        const string rulebookDescription = "After attacking, [creature] perishes, then shuffle 2 Revenants with its old sigils into your side deck. "
                                         + "A Revenant is defined as: 3 Power, 1 Health, Brittle.";

        AbilityInfo ability = AbilityManager.New(
            PluginGUID,
            rulebookName.SplitPascalCase(),
            rulebookDescription,
            typeof(Revedue),
            Loader.LoadIcon(rulebookName)
            )
            //.SetDefaultPart1Ability()
            //.AddMetaCategories(GuidManager.GetEnumValue<AbilityMetaCategory>("arackulele.inscryption.grimoramod", "ElectricChairLevel3"))
            .SetPixelAbilityIcon(Loader.LoadIcon(rulebookName, true))
            ;
        ability.powerLevel = -2;
        Revedue.ability = ability.ability;
    }
}
