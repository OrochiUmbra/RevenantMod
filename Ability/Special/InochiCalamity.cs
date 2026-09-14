using DiskCardGame;
using InscryptionAPI.Card;
using System.Collections;
using UnityEngine;

namespace RevenantMod;

public class InochiCalamity : SpecialCardBehaviour//, IOnHammerStrike
{
    const string PluginGUID = "orochiumbra.inscryption.revenantmod";
    const string abilityName = nameof(InochiCalamity);
    public SpecialTriggeredAbility SpecialAbility => specialAbility;
    public static SpecialTriggeredAbility specialAbility = SpecialTriggeredAbilityManager.Add(PluginGUID, abilityName, typeof(InochiCalamity)).Id;

    //private static bool hex = false;
    private static bool trigger = false;
    //private static bool trigger2 = false;

    /*public override bool RespondsToOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target) => attacker == null && amount >= 90 && amount <= 110 && target == PlayableCard;

    public override IEnumerator OnOtherCardDealtDamage(PlayableCard attacker, int amount, PlayableCard target)
    {
        hex = true;
        yield return TextDisplayer.Instance.ShowUntilInput("You... you murderous monster.");
    }
    
    public bool RespondsToHammerStrike(CardSlot targetSlot, CardInfo slotCardInfo) => slotCardInfo.HasSpecialAbility(specialAbility);

    public IEnumerator OnHammerStrike(CardSlot targetSlot, CardInfo slotCardInfo)
    {
        hex = true;
        RevenantMod.Log.LogInfo("Hex : Calamity Struck");
        yield return TextDisplayer.Instance.ShowUntilInput("You... you will pay for this.", -2.5f, 0.5f, Emotion.Anger);
    }*/

    public override bool RespondsToDie(bool wasSacrifice, PlayableCard killer) => true;

    public override IEnumerator OnDie(bool wasSacrifice, PlayableCard killer)
    {
        if (ViewManager.Instance.CurrentView != View.Default)
        {
            yield return new WaitForSeconds(0.1f);
            ViewManager.Instance.SwitchToView(View.Default);
            yield return new WaitForSeconds(0.1f);
        }

        if (wasSacrifice/* || killer == null && PlayableCard.LacksAbility(Ability.Brittle)*/)
        {
            yield return TextDisplayer.Instance.ShowUntilInput("You... you murderous monster.", -2.5f, 0.5f, Emotion.Anger);

            if (Singleton<BoardManager>.Instance is BoardManager3D)
            {
                //hex = true;
                Singleton<UIManager>.Instance.Effects.GetEffect<EyelidMaskEffect>().SetIntensity(0.5f, 0.75f);
                yield return new WaitForSeconds(0.5f);
                Singleton<FirstPersonController>.Instance.AnimController.PlayOneShotAnimation("PliersAnimation", null);
                AudioController.Instance.PlaySound2D("whoosh2", MixerGroup.None, 1f, 0.4f, null, null, null, null, false);
                yield return new WaitForSeconds(0.35f);
                AudioController.Instance.PlaySound2D("consumable_pliers_use", MixerGroup.None, 1f, 0f, null, null, null, null, false);
                yield return new WaitForSeconds(0.75f);
                AudioController.Instance.FadeBGMMixerParam("BGMLowpassFreq", 50f, 0.1f);
                Singleton<UIManager>.Instance.Effects.GetEffect<EyelidMaskEffect>().SetIntensity(0f, 0.025f);
                Singleton<CameraEffects>.Instance.Shake(0.1f, 0.5f);
                Singleton<UIManager>.Instance.Effects.GetEffect<ScreenColorEffect>().SetColor(GameColors.Instance.red);
                Singleton<UIManager>.Instance.Effects.GetEffect<ScreenColorEffect>().SetIntensity(1f, 50f);
                Singleton<CameraEffects>.Instance.TweenBlur(4f, 0.03f);
                yield return new WaitForSeconds(0.03f);
                Singleton<UIManager>.Instance.Effects.GetEffect<ScreenColorEffect>().SetIntensity(0f, 0.2f);
                Singleton<CameraEffects>.Instance.TweenBlur(0f, 4f);
                yield return new WaitForSeconds(1f);
                yield return Singleton<LifeManager>.Instance.ShowDamageSequence(1, 1, true, 0.25f, ResourceBank.Get<GameObject>("Prefabs/Environment/ScaleWeights/Weight_RealTooth"), 0f, true);
                AudioController.Instance.FadeBGMMixerParam("BGMLowpassFreq", 5000f, 5f);
            }
            else
                yield return Singleton<LifeManager>.Instance.ShowDamageSequence(1, 1, true);
        }

        if (!trigger && !wasSacrifice/* && !hex*/)
        {
            trigger = true;
            yield return TextDisplayer.Instance.ShowUntilInput("Oh Dear... The flow of Calamity will pursue you.", -2.5f, 0.5f, Emotion.Surprise);
        }

        CardInfo info = CardLoader.GetCardByName($"{PluginGUID}_InochiUmbra");
        info.Mods = new(PlayableCard.Info.Mods);

        Singleton<ViewManager>.Instance.SwitchToView(View.Hand, lockAfter: true);
        yield return CardSpawner.Instance.SpawnCardToHand(info);
        yield return new WaitForSeconds(0.5f);
        Singleton<ViewManager>.Instance.Controller.LockState = ViewLockState.Unlocked;

        /*if (hex)
        {
            yield return new WaitForSeconds(0.2f);
            List<PlayableCard> list = Singleton<PlayerHand>.Instance.CardsInHand.FindAll((PlayableCard x) => x != Singleton<PlayerHand>.Instance.ChoosingSlotCard);
            while (list.Count > 0)
            {
                list[0].SetInteractionEnabled(false);
                list[0].Anim.PlayDeathAnimation(true);
                Destroy(list[0].gameObject, 1f);
                Singleton<PlayerHand>.Instance.RemoveCardFromHand(list[0]);
                list.RemoveAt(0);
            }
            hex = false;
        }*/
    }

    /*public override bool RespondsToUpkeep(bool playerUpkeep) => PlayableCard.OpponentCard != playerUpkeep;

    public override IEnumerator OnUpkeep(bool playerUpkeep)
    {
        if (PlayableCard.Health >= 7)
        {
            Singleton<ViewManager>.Instance.SwitchToView(View.Board);
            PlayableCard.Anim.StrongNegationEffect();
            if (PlayableCard.Health == 7 && !trigger2)
            {
                trigger2 = true;
                yield return TextDisplayer.Instance.ShowUntilInput("Careful... Try not to let his health go pass the tippping point.", -2.5f, 0.5f, Emotion.Quiet);
            }
            else
                yield return PlayableCard.Die(false, PlayableCard);
        }
    }*/
}
