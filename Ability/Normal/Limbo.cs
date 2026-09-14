using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using InscryptionAPI.Guid;
using Sirenix.Utilities;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace RevenantMod;

public class Limbo : AbilityBehaviour, IModifyDeadCard
{
    public static Ability ability;
    public override Ability Ability => ability;

    private float newCardYPosition;

    public override bool RespondsToPreDeathAnimation(bool wasSacrifice) => true;

    public bool RespondsToModifyDeadCard(CardInfo deadCard, bool wasSacrifice, bool opponentCard, CardInfo killer) => true;

    public CardInfo OnModifyDeadCard(CardInfo deadCard, bool wasSacrifice, bool opponentCard, CardInfo killer)
    {
        CardInfo changedInfo = deadCard.Clone() as CardInfo;
        changedInfo.Mods = new(deadCard.Mods);
        if (!changedInfo.Mods.Exists(x => x.singletonId == "orochi:Limbo"))
            changedInfo.Mods.Add(new() { singletonId = "orochi:Limbo" });
        /*OuroCoreBase component = base.Card.GetComponent<OuroCoreEnergy>();
        if (component != null)
            changedInfo.Mods.Add(new(1, 1) { energyCostAdjustment = changedInfo.EnergyCost < 5 ? (changedInfo.HasAbility(Ability.Brittle) ? 1 : 2) : changedInfo.EnergyCost == 5 ? 1 : 0 });*/
        CardDrawPiles.Instance.Deck.AddCard(changedInfo);

        if (Singleton<BoardManager>.Instance is BoardManager3D)
        {
            GameObject obj = CardDrawPiles3D.Instance.Pile.CreateCard();
            obj.transform.localPosition = new Vector3(0f, newCardYPosition, 0f);
            obj.transform.localEulerAngles = CardDrawPiles3D.Instance.Pile.GetRandomCardEulers(15f);
            new WaitForSeconds(0.5f);
            Singleton<ViewManager>.Instance.SwitchToView(View.Default);
        }
        return null; // don't add to graveyard
    }

    public override IEnumerator OnPreDeathAnimation(bool wasSacrifice)
    {
        yield return PreSuccessfulTriggerSequence();

        base.Card.Anim.SetFaceDown(true);
        yield return new WaitForSeconds(0.5f);

        if (Singleton<BoardManager>.Instance is BoardManager3D)
        {
            newCardYPosition = CardDrawPiles3D.Instance.Pile.cardYSpacing * CardDrawPiles3D.Instance.Pile.cards.Count;
            Singleton<CardDrawPiles3D>.Instance.Pile.MoveCardToPile(Card, false, 0f, newCardYPosition);
            if (ViewManager.Instance.CurrentView != View.CardPiles)
            {
                yield return new WaitForSeconds(0.2f);
                ViewManager.Instance.SwitchToView(View.CardPiles);
                yield return new WaitForSeconds(0.2f);
            }
        }
        yield return LearnAbility(0.5f);
    }
}

[HarmonyPatch(typeof(CardDrawPiles), nameof(CardDrawPiles.DrawCardFromDeck))]
internal class LimboPatch
{
    [HarmonyPrefix]
    private static bool DrawLimboCard(ref CardInfo specificCard)
    {
        RevenantMod.Log.LogDebug($"Recursive start");
        if (Singleton<TurnManager>.Instance.PlayerPhase != TurnManager.PlayerTurnPhase.Draw)
            return true;

        RevenantMod.Log.LogDebug($"Recursive IsPlayerDraw");
        if (CardDrawPiles.Instance.Deck.Cards.Any(x => x.HasAbility(Limbo.ability) && x.Mods.Exists(x => x.singletonId == "orochi:Limbo")))
        {
            RevenantMod.Log.LogInfo($"Recursive Exists");
            specificCard = CardDrawPiles.Instance.Deck.Cards.Find(x => x.HasAbility(Limbo.ability) && x.Mods.Exists(x => x.singletonId == "orochi:Limbo"));
        }

        return true;
    }
}

public partial class RevenantMod
{
    public void Add_Ability_Recursive()
    {
        const string rulebookName = nameof(Limbo);
        const string rulebookDescription = "When [creature] perishes, it returns to the top of your deck.";

        AbilityInfo ability = AbilityManager.New(
            PluginGUID,
            rulebookName.SplitPascalCase(),
            rulebookDescription,
            typeof(Limbo),
            Loader.LoadIcon(rulebookName))
            .SetDefaultPart1Ability()
            .AddMetaCategories(GuidManager.GetEnumValue<AbilityMetaCategory>("arackulele.inscryption.grimoramod", "ElectricChairLevel2"))
            .SetPixelAbilityIcon(Loader.LoadIcon(rulebookName, true))
            ;
        ability.powerLevel = 2;
        Limbo.ability = ability.ability;
    }

    /*[HarmonyPatch(typeof(CardDrawPiles), "DrawPhaseSequence")]
    [HarmonyPostfix]
    public static IEnumerator RecursiveWhenExhaust(IEnumerator enumerator, CardDrawPiles __instance)
    {
        List<CardInfo> recursiveList = Singleton<BoardManager>.Instance.GetComponent<GraveyardManager>().graveyardList.FindAll(x => x.HasAbility(Recursive.ability));
        if (recursiveList.Count <= 0)
            yield return enumerator;
        else
        {
            int num;
            for (int i = 0; i < __instance.NumDrawPhaseDraws || i < recursiveList.Count; i = num + 1)
            {
                yield return __instance.ChooseDraw(i);
                num = i;
            }
        }
        yield break;
    }*/
}
