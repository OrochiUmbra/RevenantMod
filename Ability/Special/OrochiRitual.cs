using DiskCardGame;
using InscryptionAPI.Card;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RevenantMod;

public class OrochiRitual : SpecialCardBehaviour
{
    const string PluginGUID = "orochiumbra.inscryption.revenantmod";
    const string abilityName = nameof(OrochiRitual);
    public SpecialTriggeredAbility SpecialAbility => specialAbility;
    public static SpecialTriggeredAbility specialAbility = SpecialTriggeredAbilityManager.Add(PluginGUID, abilityName, typeof(OrochiRitual)).Id;
    //public static SpecialTriggeredAbilityManager.FullSpecialTriggeredAbility FullSpecial;

    //public static readonly string rName = "Three Birds";
    //public static readonly string rDesc = "Gain a special card when Punishing Bird, Judgement Bird, and Big Bird are on the same side of the board.";
    public override bool RespondsToResolveOnBoard() => true;
    public override bool RespondsToOtherCardResolve(PlayableCard otherCard) => otherCard.OpponentCard == base.PlayableCard.OpponentCard;
    public override IEnumerator OnResolveOnBoard() => CheckForOtherCards();
    public override IEnumerator OnOtherCardResolve(PlayableCard otherCard) => CheckForOtherCards();
    public static bool OwnsOrochi => RunState.Run.playerDeck.Cards.Exists((CardInfo x) => x.name == $"{PluginGUID}_Orochi");

    public static List<CardSlot> GetSlotsCopy(bool isOpponentCard)
    {
        return isOpponentCard ? Singleton<BoardManager>.Instance.OpponentSlotsCopy : Singleton<BoardManager>.Instance.PlayerSlotsCopy;
    }

    private IEnumerator CheckForOtherCards()
    {
        // Break if already have Orochi
        if (OwnsOrochi)
        {
            RevenantMod.Log.LogDebug("Player already has Orochi.");
            yield break;
        }

        CardSlot takerSlot = null;
        CardSlot civilianSlot = null;
        CardSlot villageSlot = null;

        foreach (CardSlot slot in GetSlotsCopy(base.PlayableCard.OpponentCard).Where((CardSlot s) => s.Card != null))
            if (slot != base.PlayableCard.Slot)
            {
                if (slot.Card.Info.name == $"{PluginGUID}_Undertaker")
                {
                    RevenantMod.Log.LogDebug("Player has Undertaker.");
                    if (RevenantSaveManager.TriggeredTakerHint != true)
                    {
                        yield return TextDisplayer.Instance.ShowUntilInput("Hecatomb the village you once lived.");
                        RevenantSaveManager.TriggeredTakerHint = true;
                    }
                    takerSlot = slot;
                    continue;
                }
                if (slot.Card.Info.name == $"{PluginGUID}_UndeadCivilian")
                {
                    RevenantMod.Log.LogDebug("Player has UndeadCivilian.");
                    if (RevenantSaveManager.TriggeredCivilianHint != true)
                    {
                        yield return TextDisplayer.Instance.ShowUntilInput("Seak for the shovel that digs forever.");
                        RevenantSaveManager.TriggeredCivilianHint = true;
                    }
                    civilianSlot = slot;
                    continue;
                }
                if (slot.Card.Info.name == $"{PluginGUID}_UndeadVillage")
                {
                    RevenantMod.Log.LogDebug("Player has UndeadVillage.");
                    if (RevenantSaveManager.TriggeredVillageHint != true)
                    {
                        yield return TextDisplayer.Instance.ShowUntilInput("Bring me the lost one with a spark in hand.");
                        RevenantSaveManager.TriggeredVillageHint = true;
                    }
                    villageSlot = slot;
                    continue;
                }
            }
        if (takerSlot == null || civilianSlot == null || villageSlot == null)
            yield break;

        yield return RitualSequence(takerSlot.Card, civilianSlot.Card, villageSlot.Card);
    }

    private IEnumerator RitualSequence(PlayableCard taker, PlayableCard civilian, PlayableCard village)
    {
        yield return new WaitForSeconds(0.5f);

        // Exposit story of the Black Forest
        //yield return DialogueEventsManager.PlayDialogueEvent("ApocalypseBirdIntro");

        /*
        AudioController.Instance.SetLoopVolume(0.5f * (Singleton<GameFlowManager>.Instance as Part1GameFlowManager).GameTableLoopVolume, 0.5f);
        AudioController.Instance.SetLoopAndPlay("red_noise", 1);
        AudioController.Instance.SetLoopVolumeImmediate(0.3f, 1);
        */

        if (!OwnsOrochi)
        {
            yield return new WaitForSeconds(0.4f);
            Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, true);
            yield return TextDisplayer.Instance.ShowUntilInput("Compelled by the beckoning of an ancient entity,");
            yield return new WaitForSeconds(0.5f);
            //yield return DialogueEventsManager.PlayDialogueEvent("ApocalypseBirdStory1");

            // Look down at the board
            Singleton<ViewManager>.Instance.SwitchToView(View.Board);
            yield return TextDisplayer.Instance.ShowUntilInput("a forbidden ritual has been initiated.");
            yield return new WaitForSeconds(0.2f);

            // Remove cards
            RunState.Run.playerDeck.RemoveCard(taker.Info);
            RunState.Run.playerDeck.RemoveCard(civilian.Info);
            RunState.Run.playerDeck.RemoveCard(village.Info);
            RunState.Run.playerDeck.RemoveCard(base.PlayableCard.Info);

            taker.Anim.StrongNegationEffect();
            yield return new WaitForSeconds(0.2f);
            yield return taker.Die(false, null, true);
            yield return new WaitForSeconds(0.2f);

            civilian.Anim.StrongNegationEffect();
            yield return new WaitForSeconds(0.2f);
            yield return civilian.Die(false, null, true);
            yield return new WaitForSeconds(0.2f);
            //yield return DialogueEventsManager.PlayDialogueEvent("ApocalypseBirdStorySmall");

            village.Anim.StrongNegationEffect();
            yield return new WaitForSeconds(0.2f);
            yield return village.Die(false, null, true);
            yield return new WaitForSeconds(0.2f);
            //yield return DialogueEventsManager.PlayDialogueEvent("ApocalypseBirdStoryLong");

            base.PlayableCard.Anim.StrongNegationEffect();
            yield return new WaitForSeconds(0.2f);
            yield return base.PlayableCard.Die(false, null, true);
            yield return new WaitForSeconds(0.2f);
            //yield return DialogueEventsManager.PlayDialogueEvent("ApocalypseBirdStoryBig");

            yield return TextDisplayer.Instance.ShowUntilInput("The pain reaches the ancient great serpent of death.");
            //yield return DialogueEventsManager.PlayDialogueEvent("ApocalypseBirdStory2");

            //Singleton<ViewManager>.Instance.SwitchToView(View.Default);
            //Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Locked;

            //yield return BoardEffects.ApocalypseTableEffects();
            yield return new WaitForSeconds(0.2f);

            // More text
            yield return TextDisplayer.Instance.ShowUntilInput("Sacrifices must be made.", -2.5f, 0.5f, Emotion.Laughter);
            //yield return DialogueEventsManager.PlayDialogueEvent("ApocalypseBirdStory3");
            yield return new WaitForSeconds(0.2f);

            // Give player Apocalypse in their deck and their hand
            Singleton<ViewManager>.Instance.SwitchToView(View.Hand);

            CardInfo info = CardLoader.GetCardByName($"{PluginGUID}_Orochi");
            RunState.Run.playerDeck.AddCard(info);

            // set cost to 0 for this fight (can play immediately that way)
            info.bonesCost = 0;
            info.energyCost = 0;
            Singleton<UIManager>.Instance.Effects.GetEffect<ScreenGlitchEffect>().SetIntensity(1f, 1f);
            Singleton<CameraEffects>.Instance.Shake(0.1f, 1f);
            AudioController.Instance.PlaySound2D("broken_hum", MixerGroup.None, 1f, 0f, null, null, null, null, false);
            yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(info, null, 0.25f, null);
            RevenantSaveManager.UnlockRitualDeck = true;
            yield return new WaitForSeconds(0.2f);

            // Li'l text blurb
            /*
            yield return DialogueEventsManager.PlayDialogueEvent("ApocalypseBirdStory4");

            yield return new WaitForSeconds(0.2f);
            Singleton<ViewManager>.Instance.SwitchToView(View.Default);
            yield return new WaitForSeconds(0.15f);
            yield return DialogueEventsManager.PlayDialogueEvent("ApocalypseBirdOutro");
            */
            Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;
        }
    }
}
/*
public class RulebookEntryThreeBirds : AbilityBehaviour
{
    public static Ability ability;
    public override Ability Ability => ability;
}
public partial class LobotomyPlugin
{
    private void Rulebook_ThreeBirds()
    {
        RulebookEntryThreeBirds.ability = LobotomyAbilityHelper.CreateRulebookAbility<RulebookEntryThreeBirds>(OrochiRitual.rName, OrochiRitual.rDesc).Id;
    }
    private void SpecialAbility_ThreeBirds()
    {
        OrochiRitual.specialAbility = AbilityHelper.CreateSpecialAbility<OrochiRitual>(pluginGuid, OrochiRitual.rName).Id;
    }
}
*/
