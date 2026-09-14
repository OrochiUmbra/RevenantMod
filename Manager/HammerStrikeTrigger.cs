using DiskCardGame;
using GrimoraMod;
using HarmonyLib;
using InscryptionAPI.Triggers;
using System.Collections;
using static HammerMod.HammerMod;

namespace RevenantMod;

internal static class HammerStrikePatch
{
    internal static void PatchHammer()
    {
        RevenantMod.harmony.PatchAll(typeof(HammerStrikePatch));
    }

    [HarmonyPatch(typeof(HammerItem), nameof(HammerItem.OnValidTargetSelected))]
    [HarmonyPostfix]
    private static IEnumerator DetectHammerOther(IEnumerator enumerator, CardSlot targetSlot)
    {
        RevenantMod.Log.LogInfo("Hammer Detected Other");
        CardInfo slotCardInfo = targetSlot.Card.Info.Clone() as CardInfo;
        slotCardInfo.Mods = new(targetSlot.Card.Info.Mods);

        if (slotCardInfo != null)
        {
            yield return CustomTriggerFinder.Trigger<IOnHammerStrike>(
                targetSlot.Card.TriggerHandler,
                x => x.RespondsToHammerStrike(targetSlot, slotCardInfo),
                x => x.OnHammerStrike(targetSlot, slotCardInfo));
        }
        yield return enumerator;
    }
}

internal static class HammerStrikePatchGrimora
{
    internal static void PatchHammer()
    {
        RevenantMod.harmony.PatchAll(typeof(HammerStrikePatchGrimora));
    }

    [HarmonyPatch(typeof(HammerItemExt), nameof(HammerItemExt.OnValidTargetSelected))]
    [HarmonyPostfix]
    private static IEnumerator DetectHammerGrimora(IEnumerator enumerator, CardSlot targetSlot)
    {
        RevenantMod.Log.LogInfo("Hammer Detected Grimora");
        CardInfo slotCardInfo = targetSlot.Card.Info.Clone() as CardInfo;
        slotCardInfo.Mods = new(targetSlot.Card.Info.Mods);

        if (slotCardInfo != null)
        {
            yield return CustomTriggerFinder.Trigger<IOnHammerStrike>(
                targetSlot.Card.TriggerHandler,
                x => x.RespondsToHammerStrike(targetSlot, slotCardInfo),
                x => x.OnHammerStrike(targetSlot, slotCardInfo));
        }
        yield return enumerator;
    }
}

internal static class HammerStrikePatchKopie
{
    internal static void PatchHammer()
    {
        RevenantMod.harmony.PatchAll(typeof(HammerStrikePatchKopie));
    }

    [HarmonyPatch(typeof(NonItemHammerBehaviour), nameof(HammerItemExt.OnValidTargetSelected))]
    [HarmonyPostfix]
    private static IEnumerator DetectHammerKopie(IEnumerator enumerator, CardSlot targetSlot)
    {
        RevenantMod.Log.LogInfo("Hammer Detected Kopie");
        CardInfo slotCardInfo = targetSlot.Card.Info.Clone() as CardInfo;
        slotCardInfo.Mods = new(targetSlot.Card.Info.Mods);

        if (slotCardInfo != null)
        {
            yield return CustomTriggerFinder.Trigger<IOnHammerStrike>(
                targetSlot.Card.TriggerHandler,
                x => x.RespondsToHammerStrike(targetSlot, slotCardInfo),
                x => x.OnHammerStrike(targetSlot, slotCardInfo));
        }
        yield return enumerator;
    }
}

public interface IOnHammerStrike
{
    public bool RespondsToHammerStrike(CardSlot targetSlot, CardInfo slotCardInfo);
    public IEnumerator OnHammerStrike(CardSlot targetSlot, CardInfo slotCardInfo);
}
