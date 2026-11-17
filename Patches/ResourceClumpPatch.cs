using HarmonyLib;
using StardewValley;
using StardewValley.TerrainFeatures;

namespace CropInstantPickup;
[HarmonyPatch(typeof(ResourceClump), nameof(ResourceClump.destroy))]
internal static class ResourceClumpPatch
{

    private static void Prefix(GameLocation location, out int __state)
    {
        __state = location?.debris.Count ?? -1;
    }
    private static void Postfix(GameLocation location, Tool t, int __state)
    {
     if (!ModEntry.Config.InstantResourceClumpPickup || __state < 0){return;}   
     DebrisSweep.CollectNew(location, t?.getLastFarmerToUse(), __state);
    }
}