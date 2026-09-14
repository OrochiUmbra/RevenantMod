using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Card;
using InscryptionAPI.Guid;
using Sirenix.Utilities;
using System.Collections;
using UnityEngine;

namespace RevenantMod;

public class ActivatedBloodDrawMain : ExtendedActivatedAbilityBehaviour
{
    public static Ability ability;
    public override Ability Ability => ability;

    private const int BLOOD_COST = 1;
    public override int StartingBloodCost => BLOOD_COST;

    public override bool CanActivate() => Singleton<Deck>.Instance.Cards.Count > 0;

    public override IEnumerator Activate()
    {
        yield return base.PreSuccessfulTriggerSequence();
        base.Card.Anim.StrongNegationEffect();
        //yield return Singleton<LifeManager>.Instance.ShowDamageSequence(2, 2, !Card.OpponentCard);
        if (Singleton<ViewManager>.Instance.CurrentView != View.Default)
        {
            yield return new WaitForSeconds(0.2f);
            Singleton<ViewManager>.Instance.SwitchToView(View.Default, false, false);
            yield return new WaitForSeconds(0.2f);
        }
        if (Singleton<CardDrawPiles3D>.Instance && Singleton<CardDrawPiles3D>.Instance.Pile)
            Singleton<CardDrawPiles3D>.Instance.Pile.Draw();
        yield return Singleton<CardDrawPiles>.Instance.DrawCardFromDeck();
        yield return new WaitForSeconds(0.2f);
        yield return base.LearnAbility(0.5f);
    }

    [HarmonyPatch(typeof(BoardManager3D), "OffsetCardNearBoard", 0)]
    public class OffsetSacrificeDemanderPatch
    {
        [HarmonyPrefix]
        public static bool OffsetPatch(BoardManager3D __instance, bool offsetCard)
        {
            if (__instance.CurrentSacrificeDemandingCard && __instance.CurrentSacrificeDemandingCard.OnBoard) { return false; }
            else return true;
        }
    }
}

public partial class RevenantMod
{
    public void Add_Ability_ActivatedBloodDrawMain()
    {
        const string rulebookName = "LifeDraw";
        const string rulebookDescription = "Pay 1 Blood to draw a card from your main deck.";

        AbilityInfo ability = AbilityManager.New(
            PluginGUID,
            rulebookName.SplitPascalCase(),
            rulebookDescription,
            typeof(ActivatedBloodDrawMain),
            Loader.LoadIcon(rulebookName)
            )
            .SetDefaultPart1Ability()
            .SetActivated()
            .AddMetaCategories(GuidManager.GetEnumValue<AbilityMetaCategory>("arackulele.inscryption.grimoramod", "ElectricChairLevel3"))
            //.SetPixelAbilityIcon(Asset.LoadTexture("pixelability_activated_" + rulebookName), FilterMode.Point)
            ;
        ability.powerLevel = 3;
        ability.pixelIcon = Loader.LoadSprite("pixelability_activated_" + rulebookName);
        ActivatedBloodDrawMain.ability = ability.ability;
    }
}
