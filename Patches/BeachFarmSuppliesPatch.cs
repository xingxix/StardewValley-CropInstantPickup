using HarmonyLib;
using StardewValley;
using StardewValley.Tools;

namespace CropInstantPickup;
[HarmonyPatch(typeof(StardewValley.Object), nameof(StardewValley.Object.performToolAction))]
internal static class BeachFarmSuppliesPatch
{
private static void Prefix(StardewValley.Object __instance, Tool t, out int __state)
    {
        __state = -1;
        if (__instance.Name?.Contains("SupplyCrate") == true && t?.isHeavyHitter() == true)
        {
            __state = __instance.Location?.debris.Count ?? -1;
        }

    }   

private static void Postfix(StardewValley.Object __instance, Tool t, int __state)
    {
     if (!ModEntry.Config.InstantSupplyCratePickup || __state < 0)
        {
            return;
        }   
    DebrisSweep.CollectNew(__instance.Location, t?.getLastFarmerToUse(), __state);
    } 
}

