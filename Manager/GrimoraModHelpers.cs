using BepInEx.Bootstrap;
using BepInEx;
using DiskCardGame;
using System.Collections.Generic;
using System.Reflection;

namespace RevenantMod;
//Debug Menu Port
public static class GrimoraModHelpers
{
    private static Assembly GrimoraModAssembly = null;
    private static PropertyInfo IsGrimoraModRunProperty = null;

    public static RunState GetRunState()
    {
        return RunState.Run;
    }

    private static void Initialize(Assembly assembly)
    {
        GrimoraModAssembly = assembly;

        IsGrimoraModRunProperty = GrimoraModAssembly.GetType("GrimoraMod.GrimoraSaveUtil")?.GetProperty("IsGrimoraModRun");
    }

    public static bool GrimoraModIsActive()
    {
        if (GrimoraModAssembly == null)
        {
            foreach (KeyValuePair<string, PluginInfo> pair in Chainloader.PluginInfos)
            {
                if (pair.Value.Metadata.GUID == "arackulele.inscryption.grimoramod")
                {
                    RevenantMod.Log.LogInfo($"[GrimoraModHelpers] GrimoraMod found!");
                    Initialize(pair.Value.Instance.GetType().Assembly);
                }
            }

            if (GrimoraModAssembly == null)
            {
                return false;
            }
        }

        return (bool)IsGrimoraModRunProperty.GetValue(null);
    }

    public static Ability GetRandomElectricAbility()
    {
        return GrimoraMod.CardRelatedExtension.GetRandomItem(GrimoraMod.ElectricChairLever.AbilitiesMajorRisk);
    }
}
