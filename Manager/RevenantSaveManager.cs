using InscryptionAPI.Saves;

namespace RevenantMod;

public static partial class RevenantSaveManager
{
    public static bool UnlockRitualDeck
    {
        get => GetSaveBool("UnlockRitualDeck");
        set => SetSaveBool("UnlockRitualDeck", value);
    }
    public static bool TriggeredCivilianHint
    {
        get => GetRunBool("TriggeredCivilianHint");
        set => SetRunBool("TriggeredCivilianHint", value);
    }
    public static bool TriggeredVillageHint
    {
        get => GetRunBool("TriggeredVillageHint");
        set => SetRunBool("TriggeredVillageHint", value);
    }
    public static bool TriggeredTakerHint
    {
        get => GetRunBool("TriggeredTakerHint");
        set => SetRunBool("TriggeredTakerHint", value);
    }

    private static bool GetRunBool(string id) => ModdedSaveManager.RunState.GetValueAsBoolean(RevenantMod.PluginGUID, id);
    private static void SetRunBool(string id, object value) => ModdedSaveManager.RunState.SetValue(RevenantMod.PluginGUID, id, value);
    private static bool GetSaveBool(string id) => ModdedSaveManager.SaveData.GetValueAsBoolean(RevenantMod.PluginGUID, id);
    private static void SetSaveBool(string id, object value) => ModdedSaveManager.SaveData.SetValue(RevenantMod.PluginGUID, id, value);
}
