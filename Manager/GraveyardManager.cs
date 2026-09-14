using DiskCardGame;
using GBC;
using HarmonyLib;
using InscryptionAPI.Card;
using InscryptionAPI.Triggers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static RevenantMod.GraveyardManager;

namespace RevenantMod;

public class GraveyardManager : Singleton<GraveyardManager>
{
    // custom class to store the relevant data
    // can be modified to include more information in the future (like the killer if you care for that
    public class DeadCardData
    {
        public CardInfo DeadCardInfo;
        public readonly bool WasSacrifice;

        public DeadCardData(CardInfo deadCard, bool wasSacrifice)
        {
            DeadCardInfo = deadCard;
            WasSacrifice = wasSacrifice;
        }
    }

    // dictionary containing card infos and whether they died via sacrifice or not
    //public readonly Dictionary<CardInfo, bool> deadCards = new();

    public readonly List<DeadCardData> allDeadCards = new();
    public readonly List<DeadCardData> opponentCards = new();
    public readonly List<DeadCardData> playerCards = new();
    public List<DeadCardData> AllDeadCards
    {
        get
        {
            allDeadCards.Clear();
            allDeadCards.AddRange(playerCards);
            allDeadCards.AddRange(opponentCards);
            allDeadCards.RemoveAll(x => x.DeadCardInfo.HasAbility(Ability.DrawCopyOnDeath));
            allDeadCards.RemoveAll(x => x.DeadCardInfo.HasAnyOfTraits(Trait.Giant, Trait.Uncuttable, Trait.DeathcardCreationNonOption));
            return allDeadCards;
        }
    }

    public bool Exhausted => AllDeadCards.Count == 0;

    public int randomSeed;
    public void Initialise()
    {
        randomSeed = SaveManager.SaveFile.GetCurrentRandomSeed();
    }
    public void CleanUp()
    {
        playerCards.Clear();
        opponentCards.Clear();
        allDeadCards.Clear();
    }

    public IEnumerator DrawCard(CardInfo specificCard = null, DrawType poolToDrawnFrom = DrawType.Player)
    {
        List<DeadCardData> cardPool = GetCardDrawPool(poolToDrawnFrom);
        yield return DrawCard(specificCard, cardPool);
    }

    public IEnumerator DrawCard(Ability withAbility, DrawType poolToDrawnFrom = DrawType.Player)
    {
        List<DeadCardData> cardPool = GetCardDrawPool(poolToDrawnFrom);
        CardInfo cardToDraw = cardPool.FirstOrDefault(x => x.DeadCardInfo.HasAbility(withAbility)).DeadCardInfo;
        if (cardToDraw == null)
        {
            Debug.Log($"No card with ability:{withAbility} found in pool");
            yield break;
        }
        yield return DrawCard(cardToDraw, cardPool);
    }

    public IEnumerator DrawCard(bool wasSacrifice, DrawType poolToDrawnFrom = DrawType.Player)
    {
        List<DeadCardData> cardPool = GetCardDrawPool(poolToDrawnFrom);
        CardInfo cardToDraw = cardPool.FirstOrDefault(x => x.WasSacrifice).DeadCardInfo;
        if (cardToDraw == null)
        {
            Debug.Log($"No card with wasSacrifice:{wasSacrifice} found in pool");
            yield break;
        }
        yield return DrawCard(cardToDraw, cardPool);
    }

    private IEnumerator DrawCard(CardInfo specifiedCard, List<DeadCardData> cardPool)
    {
        if (Exhausted)
        {
            Debug.Log($"No cards/card does not exist");
            yield break;
        }
        DeadCardData deadCardData = cardPool.Find(x => x.DeadCardInfo == specifiedCard) ?? cardPool[SeededRandom.Range(0, cardPool.Count, randomSeed++)];
        cardPool.Remove(deadCardData);
        if (Singleton<ViewManager>.Instance.CurrentView != View.Default)
        {
            yield return new WaitForSeconds(0.2f);
            Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
            yield return new WaitForSeconds(0.2f);
        }
        yield return CardSpawner.Instance.SpawnCardToHand(deadCardData.DeadCardInfo, 0.15f);
    }
    private List<DeadCardData> GetCardDrawPool(DrawType poolToDrawnFrom)
    {
        return poolToDrawnFrom switch
        {
            DrawType.Player => playerCards,
            DrawType.Opponent => opponentCards,
            _ => allDeadCards
        };
    }
    public enum DrawType
    {
        Player,
        Opponent,
        All
    }
}

public static class GraveyardExts
{
    public static bool Remove(this List<DeadCardData> data, CardInfo info)
    {
        DeadCardData item = data.Find(x => x.DeadCardInfo == info);
        return data.Remove(item);
    }
}

[HarmonyPatch]
internal static class DeadCardPilePatches
{
    [HarmonyPatch(typeof(PlayableCard), nameof(PlayableCard.Die))]
    [HarmonyPostfix]
    private static IEnumerator TriggerAddDeadCardToPile(IEnumerator enumerator, PlayableCard __instance, bool wasSacrifice, CardInfo killer)
    {
        yield return enumerator;

        Debug.Log("Graveyard start");
        CardInfo deadInfo = __instance.Info.Clone() as CardInfo;
        deadInfo.Mods = new(__instance.Info.Mods);
        DeadCardData data = new(deadInfo, wasSacrifice);

        // trigger each ability that modifies the deadInfo
        foreach (IModifyDeadCard component in CustomTriggerFinder.FindTriggersOnCard<IModifyDeadCard>(__instance))
        {
            if (component.RespondsToModifyDeadCard(deadInfo, wasSacrifice, __instance.OpponentCard, killer))
            {
                data.DeadCardInfo = component.OnModifyDeadCard(deadInfo, wasSacrifice, __instance.OpponentCard, killer);
            }
        }

        // little way of preventing a card from being added to the graveyard
        // should also prevent any null errors when trying to draw cards from the graveyard
        if (data.DeadCardInfo != null)
        {
            (__instance.OpponentCard ? GraveyardManager.Instance.opponentCards : GraveyardManager.Instance.playerCards).Add(data);

            yield return CustomTriggerFinder.Trigger<IOnCardAddedToGraveyard>(
                __instance.TriggerHandler,
                x => x.RespondsToAddDeadCardToPile(deadInfo, wasSacrifice, __instance.OpponentCard, killer),
                x => x.OnAddDeadCardToPile(deadInfo, wasSacrifice, __instance.OpponentCard, killer));
        }
    }

    [HarmonyPatch(typeof(BoardManager3D), nameof(BoardManager3D.Initialize))]
    [HarmonyPatch(typeof(BoardManager), nameof(BoardManager.Initialize))]
    [HarmonyPrefix]
    private static void AddDeadCardPile(BoardManager __instance)
    {
        // shorthand condensing a null check (the ??) with a method call; AddComponent returns the added component instance btw
        (GraveyardManager.Instance ?? __instance.gameObject.AddComponent<GraveyardManager>()).Initialise();
    }

    [HarmonyPatch(typeof(BoardManager3D), nameof(BoardManager3D.CleanUp))]
    [HarmonyPatch(typeof(PixelBoardManager), nameof(PixelBoardManager.CleanUp))]
    [HarmonyPrefix]
    private static void ClearDeadCardPile() =>  GraveyardManager.Instance.CleanUp();
}

public interface IModifyDeadCard
{
    public bool RespondsToModifyDeadCard(CardInfo deadCard, bool wasSacrifice, bool opponentCard, CardInfo killer);
    public CardInfo OnModifyDeadCard(CardInfo deadCard, bool wasSacrifice, bool opponentCard, CardInfo killer);
}

public interface IOnCardAddedToGraveyard
{
    public bool RespondsToAddDeadCardToPile(CardInfo deadCard, bool wasSacrifice, bool opponentCard, CardInfo killer);
    public IEnumerator OnAddDeadCardToPile(CardInfo deadCard, bool wasSacrifice, bool opponentCard, CardInfo killer);
}
