using System.Runtime.CompilerServices;
using HarmonyLib;
using StardewValley;
using StardewValley.TerrainFeatures;

namespace CropInstantPickup;
[HarmonyPatch(typeof(Bush), nameof(Bush.performUseAction))]
internal static class BushPatch
{
    private static void Prefix(Bush __instance, out int __state)
    {
        __state = __instance.Location?.debris.Count ?? -1;
    }

    private static void Postfix(Bush __instance, int __state)
    {
        if (!ModEntry.Config.InstantBushPickup || __state < 0)
        {
            return;
        }
        DebrisSweep.CollectNew(__instance.Location, Game1.player, __state);
    }
    
}

[HarmonyPatch(typeof(Bush), nameof(Bush.performToolAction))]
internal static class AnotherBushPatch
{
    private static void Prefix(Bush __instance, out int __state)
    {
        __state = __instance.Location?.debris.Count ?? -1;
    }

    private static void Postfix(Bush __instance, Tool t, int __state)
    {
        if (!ModEntry.Config.InstantBushPickup || __state < 0)
        {
            return;
        }
        DebrisSweep.CollectNew(__instance.Location, t?.getLastFarmerToUse(), __state);
    }
}