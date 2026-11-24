using CropInstantPickup;
using HarmonyLib;
using StardewValley;
using StardewValley.TerrainFeatures;

namespace CropInstantPickup;
[HarmonyPatch(typeof(GiantCrop), nameof(GiantCrop.performToolAction))]
internal static class GiantCropPatch
{
    private static void Prefix(GiantCrop __instance, out int __state)
    {
        __state = __instance.Location?.debris.Count ?? -1;
    }
    private static void Postfix(GiantCrop __instance, Tool t, int __state)
    {
        if(!ModEntry.Config.InstantResourceClumpPickup || __state < 0)
        {
            return;
        }
        DebrisSweep.CollectNew(__instance.Location, t?.getLastFarmerToUse(), __state);
    }
}